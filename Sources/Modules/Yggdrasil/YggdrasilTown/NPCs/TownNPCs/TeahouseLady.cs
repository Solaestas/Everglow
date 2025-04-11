using Everglow.Commons.Coroutines;
using Everglow.Yggdrasil.YggdrasilTown.Projectiles.TownNPCAttack;
using SubworldLibrary;
using Terraria.DataStructures;
using Terraria.GameContent.Personalities;
using Terraria.Localization;
using static Everglow.Commons.Utilities.NPCUtils;

namespace Everglow.Yggdrasil.YggdrasilTown.NPCs.TownNPCs;

[AutoloadHead]

// Schorl, The Teahouse Lady
public class TeahouseLady : ModNPC
{
	private bool canDespawn = false;
	private int aiMainCount = 0;

	/// <summary>
	/// Control specific behaviors.
	/// </summary>
	public CoroutineManager _townNPCBehaviorCoroutine = new CoroutineManager();

	/// <summary>
	/// Manage behavior pool.
	/// </summary>
	public CoroutineManager _townNPCGeneralCoroutine = new CoroutineManager();
	public Queue<Coroutine> BehaviorsCoroutines = new Queue<Coroutine>();
	public bool Idle = true;
	public bool Talking = false;
	public bool Sit = false;
	public bool Attacking0 = false;
	public int FrameHeight = 56;
	public int Attack0Target = -1;
	public int Attack1Target = -1;
	public int ThreatenTarget = -1;

	/// <summary>
	/// A scalars for the total threaten value;
	/// </summary>
	public float TotalThreaten = 0f;

	/// <summary>
	/// A vector2 for total threaten direction;
	/// </summary>
	public Vector2 TotalThreatenDirection = Vector2.zeroVector;

	public override string HeadTexture => ModAsset.TeahouseLady_Head_Mod;

	public override void SetStaticDefaults()
	{
		Main.npcFrameCount[NPC.type] = 12;
	}

	public override void SetDefaults()
	{
		NPC.townNPC = true;
		NPC.width = 18;
		NPC.height = 40;
		NPC.aiStyle = -1;
		NPC.damage = 100;
		NPC.defense = 100;
		NPC.lifeMax = 250;
		NPC.HitSound = SoundID.NPCHit1;
		NPC.DeathSound = SoundID.NPCDeath6;
		NPC.boss = false;
		NPC.friendly = true;
		NPC.knockBackResist = 0;

		NPCHappiness NH = NPC.Happiness;
		NH.SetBiomeAffection<ForestBiome>((AffectionLevel)50);
		NH.SetBiomeAffection<SnowBiome>((AffectionLevel)70);
		NH.SetBiomeAffection<CrimsonBiome>((AffectionLevel)90);
		NH.SetBiomeAffection<CorruptionBiome>((AffectionLevel)90);
		NH.SetBiomeAffection<UndergroundBiome>((AffectionLevel)(-20));
		NH.SetBiomeAffection<DesertBiome>((AffectionLevel)20);
		NH.SetBiomeAffection<DungeonBiome>((AffectionLevel)(-50));
		NH.SetBiomeAffection<OceanBiome>((AffectionLevel)50);
		NH.SetBiomeAffection<JungleBiome>((AffectionLevel)30);
	}

	public override float SpawnChance(NPCSpawnInfo spawnInfo)
	{
		return 0;
	}

	public override void OnSpawn(IEntitySource source)
	{
		_townNPCGeneralCoroutine.StartCoroutine(new Coroutine(BehaviorControl()));
		base.OnSpawn(source);
	}

	public override bool PreAI()
	{
		aiMainCount = 0;
		return base.PreAI();
	}

	public override void AI()
	{
		_townNPCBehaviorCoroutine.Update();
		_townNPCGeneralCoroutine.Update();
		if (!Talking)
		{
			CheckDangers();
			if (BehaviorsCoroutines.Count <= 1)
			{
				if (Peace())
				{
					if (Main.rand.NextBool())
					{
						BehaviorsCoroutines.Enqueue(new Coroutine(Stand(Main.rand.Next(60, 90))));
					}
					else
					{
						BehaviorsCoroutines.Enqueue(new Coroutine(Walk(Main.rand.Next(60, 90))));
					}
				}
				else if (CanEscape())
				{
					if (TotalThreaten < 1 && Main.rand.NextBool(2))
					{
						TryAttack();
					}
					else
					{
						BehaviorsCoroutines.Enqueue(new Coroutine(Walk(Main.rand.Next(60, 90))));
					}
				}
				else
				{
					TryAttack();
				}
			}
		}
		else
		{
			if (!CheckTalkingPlayer())
			{
				Talking = false;
			}
		}

		if (aiMainCount == 0)
		{
			Idle = true;
			_townNPCGeneralCoroutine.StartCoroutine(new Coroutine(BehaviorControl()));
		}

		// ai[0] = 5 is a magic number represent to a sitting town npc.
		// so we should prevent other npc from sitting the same chair.
		if (Sit)
		{
			NPC.aiStyle = 7;
			NPC.ai[0] = 5;
		}
		else
		{
			NPC.aiStyle = -1;
			NPC.ai[0] = 0;
		}
	}

	public void TryAttack()
	{
		float index = Main.rand.NextFloat(100);
		if (index < 75)
		{
			if (CanAttack0())
			{
				BehaviorsCoroutines.Enqueue(new Coroutine(Attack0()));
			}
			else if (CanAttack1())
			{
				BehaviorsCoroutines.Enqueue(new Coroutine(Attack1()));
			}
		}
		else
		{
			if (CanAttack1())
			{
				BehaviorsCoroutines.Enqueue(new Coroutine(Attack1()));
			}
			else if (CanAttack0())
			{
				BehaviorsCoroutines.Enqueue(new Coroutine(Attack0()));
			}
		}
	}

	public void CheckDangers()
	{
		ThreatenTarget = -1;
		TotalThreatenDirection = Vector2.zeroVector;
		TotalThreaten = 0f;
		float minDis = 300;
		foreach (var npc in Main.npc)
		{
			if (!npc.friendly && !npc.dontTakeDamage && npc.active && npc.life > 0 && npc.type != NPCID.TargetDummy && !npc.CountsAsACritter && npc != NPC)
			{
				Vector2 distance = npc.Center - NPC.Center;
				if (distance.Length() < 300 && npc.damage > 0)
				{
					TotalThreatenDirection += (-distance.NormalizeSafe() / (distance.Length() + 1)) * 1f * npc.damage;
					TotalThreaten += (-distance.NormalizeSafe() / (distance.Length() + 1)).Length() * 1f * npc.damage;
					if (distance.Length() < minDis)
					{
						minDis = distance.Length();
						ThreatenTarget = npc.whoAmI;
					}
				}
			}
		}
	}

	public bool CheckTalkingPlayer()
	{
		for (int j = 0; j < 255; j++)
		{
			if (Main.player[j].active && Main.player[j].talkNPC == NPC.whoAmI)
			{
				if (Main.player[j].position.X + Main.player[j].width / 2 < NPC.position.X + NPC.width / 2)
				{
					NPC.direction = -1;
				}
				else
				{
					NPC.direction = 1;
				}
				return true;
			}
		}
		return false;
	}

	public bool Peace()
	{
		if (TotalThreaten > 0.005)
		{
			return false;
		}
		return true;
	}

	public bool CanEscape()
	{
		if (TotalThreaten > 0.1f && MathF.Abs(TotalThreatenDirection.X) < 0.05f)
		{
			return false;
		}
		return true;
	}

	public bool CanAttack0()
	{
		float minDis = 1000;
		Attack0Target = -1;
		foreach (var npc in Main.npc)
		{
			if (!npc.friendly && !npc.dontTakeDamage && npc.active && npc.life > 0 && npc.type != NPCID.TargetDummy && !npc.CountsAsACritter && npc != NPC)
			{
				Vector2 distance = npc.Center - NPC.Center;
				if (distance.Length() < minDis && CanHitNPCLine(npc))
				{
					minDis = distance.Length();
					if (npc.Center.X > NPC.Center.X)
					{
						NPC.direction = 1;
					}
					else
					{
						NPC.direction = -1;
					}
					NPC.spriteDirection = NPC.direction;
					Attack0Target = npc.whoAmI;
				}
			}
		}
		if (Attack0Target != -1)
		{
			return true;
		}
		return false;
	}

	public bool CanAttack1()
	{
		float minDis = 450;
		Attack1Target = -1;
		foreach (var npc in Main.npc)
		{
			if (!npc.friendly && !npc.dontTakeDamage && npc.active && npc.life > 0 && npc.type != NPCID.TargetDummy && !npc.CountsAsACritter && npc != NPC)
			{
				Vector2 distance = npc.Center - NPC.Center;
				if (distance.Length() < minDis)
				{
					minDis = distance.Length();
					if (npc.Center.X > NPC.Center.X)
					{
						NPC.direction = 1;
					}
					else
					{
						NPC.direction = -1;
					}
					NPC.spriteDirection = NPC.direction;
					Attack1Target = npc.whoAmI;
				}
			}
		}
		if (Attack1Target != -1)
		{
			return true;
		}
		return false;
	}

	public Point AnchorForBehaviorPos => YggdrasilTownCentralSystem.TownTopLeftWorldCoord.ToTileCoordinates() + new Point(405, 157);

	public void CheckInSuitableArea()
	{
		if (SubworldSystem.Current is not YggdrasilWorld)
		{
			return;
		}
		bool safe = false;
		var homePoint = AnchorForBehaviorPos;
		NPC.homeless = false;
		NPC.homeTileX = homePoint.X;
		NPC.homeTileY = homePoint.Y;

		var npcTilePos = NPC.Center.ToTileCoordinates();
		if (npcTilePos.X > AnchorForBehaviorPos.X - 100 && npcTilePos.X < AnchorForBehaviorPos.X + 100)
		{
			if (npcTilePos.Y > AnchorForBehaviorPos.Y - 20 && npcTilePos.Y < AnchorForBehaviorPos.Y + 20)
			{
				safe = true;
			}
		}
		if (!safe)
		{
			TeleportHome();
		}
	}

	public void TeleportHome()
	{
		NPC.Center = AnchorForBehaviorPos.ToWorldCoordinates() + new Vector2(0, 48);
	}

	public IEnumerator<ICoroutineInstruction> BehaviorControl()
	{
		while (true)
		{
			CheckInSuitableArea();
			aiMainCount++;
			if (BehaviorsCoroutines.Count > 0 && Idle)
			{
				_townNPCBehaviorCoroutine.StartCoroutine(BehaviorsCoroutines.First());
				Sit = false;
				Idle = false;
			}
			if (aiMainCount >= 2)
			{
				yield break;
			}
			yield return new SkipThisFrame();
		}
	}

	public IEnumerator<ICoroutineInstruction> Walk(int time)
	{
		NPC.direction = ChooseDirection(NPC);
		for (int t = 0; t < time; t++)
		{
			if (TotalThreatenDirection.X > 0)
			{
				NPC.direction = 1;
			}
			if (TotalThreatenDirection.X < 0)
			{
				NPC.direction = -1;
			}
			NPC.spriteDirection = NPC.direction;
			float speed = 1.2f;
			if (TotalThreatenDirection.Length() > 0.5f)
			{
				speed += TotalThreatenDirection.Length() - 0.5f;
			}
			if (!CanEscape())
			{
				break;
			}
			NPC.velocity.X = NPC.direction * speed;
			Idle = false;
			NPC.frameCounter += Math.Abs(NPC.velocity.X);
			if (NPC.frameCounter > 4)
			{
				NPC.frame.Y += FrameHeight;
				NPC.frameCounter = 0;
			}
			if (NPC.frame.Y > 9 * FrameHeight)
			{
				NPC.frame.Y = FrameHeight;
			}
			if (!CanContinueWalk(NPC))
			{
				break;
			}
			if (Talking)
			{
				break;
			}
			if (Main.rand.NextBool(240))
			{
				if (CheckSit(NPC))
				{
					Sit = true;
					break;
				}
			}
			TryOpenDoor(NPC);
			TryCloseDoor(NPC);
			yield return new SkipThisFrame();
		}
		NPC.velocity.X *= 0;
		EndAIPiece();
	}

	/// <summary>
	/// Laser
	/// </summary>
	/// <returns></returns>
	public IEnumerator<ICoroutineInstruction> Attack0()
	{
		if (Attack0Target == -1)
		{
			EndAIPiece();
			yield break;
		}
		NPC target = Main.npc[Attack0Target];
		if (target == null || !CanHitNPCLine(target))
		{
			EndAIPiece();
			yield break;
		}
		NPC.spriteDirection = NPC.direction;
		Attacking0 = true;
		for (int t = 0; t < 180; t++)
		{
			Vector2 toTarget = target.Center - NPC.Center;
			if (toTarget.X > 0)
			{
				NPC.direction = 1;
			}
			else
			{
				NPC.direction = -1;
			}
			if (t == 0)
			{
				Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, (target.Center - NPC.Center).NormalizeSafe() * 6, ModContent.ProjectileType<Schorl_Laser>(), 150, 1.2f, Main.myPlayer, target.whoAmI);
			}
			NPC.spriteDirection = NPC.direction;
			NPC.velocity *= 0;
			if (target == null || !target.active)
			{
				break;
			}
			yield return new SkipThisFrame();
		}
		Attacking0 = false;
		NPC.frame = new Rectangle(0, 0, 48, FrameHeight);
		yield return new WaitForFrames(16);
		EndAIPiece();
	}

	public IEnumerator<ICoroutineInstruction> Attack1()
	{
		if (Attack1Target == -1)
		{
			EndAIPiece();
			yield break;
		}
		NPC target = Main.npc[Attack1Target];
		if (target == null)
		{
			EndAIPiece();
			yield break;
		}
		Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), target.Center, Vector2.zeroVector, ModContent.ProjectileType<Schorl_Mark>(), 0, 0, Main.myPlayer, target.whoAmI);
		for (int t = 0; t < 141; t++)
		{
			Vector2 toTarget = target.Center - NPC.Center;
			if (toTarget.X > 0)
			{
				NPC.direction = 1;
			}
			else
			{
				NPC.direction = -1;
			}
			if (t >= 20 && t % 20 == 0)
			{
				Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), target.Center, Vector2.zeroVector, ModContent.ProjectileType<Schorl_Explosion>(), 30, 0.15f, Main.myPlayer, target.whoAmI);
			}
			NPC.spriteDirection = NPC.direction;
			NPC.velocity *= 0;
			Idle = false;
			if (target == null || !target.active)
			{
				break;
			}
			yield return new SkipThisFrame();
		}
		NPC.direction *= -1;
		NPC.spriteDirection = NPC.direction;
		NPC.frame = new Rectangle(0, 0, 48, FrameHeight);
		EndAIPiece();
	}

	public IEnumerator<ICoroutineInstruction> Stand(int time)
	{
		for (int t = 0; t < time; t++)
		{
			NPC.spriteDirection = NPC.direction;
			NPC.frame = new Rectangle(0, 616, 48, FrameHeight);
			NPC.velocity.X = 0;
			Idle = false;
			NPC.spriteDirection = NPC.direction;
			if (!Peace())
			{
				if (TotalThreatenDirection.X > 0)
				{
					NPC.direction = 1;
				}
				else
				{
					NPC.direction = -1;
				}
				BehaviorsCoroutines.Enqueue(new Coroutine(Walk(60)));
				break;
			}
			yield return new SkipThisFrame();
		}
		EndAIPiece();
	}

	public bool CanHitNPCLine(NPC target)
	{
		bool canHit = true;
		var toTarget = target.Center - NPC.Center;
		float distance = toTarget.Length() / 6f;
		if (distance < 2)
		{
			return true;
		}
		var step = toTarget.NormalizeSafe() * 6f;
		var checkPos = NPC.Center;
		for (int i = 0; i < distance; i++)
		{
			checkPos += step;
			if (Collision.SolidCollision(checkPos - new Vector2(8), 16, 16))
			{
				canHit = false;
				break;
			}
		}
		return canHit;
	}

	public void EndAIPiece()
	{
		BehaviorsCoroutines.Dequeue();
		Idle = true;
	}

	public override bool CanChat()
	{
		return true;
	}

	public override string GetChat()
	{
		Talking = true;

		return base.GetChat();
	}

	public override void SetChatButtons(ref string button, ref string button2)
	{
		// TODO Hjson
		if (Language.ActiveCulture.Name == "zh-Hans")
		{
			button = Language.GetTextValue("挑战");
			button2 = Language.GetTextValue("帮助");
		}
		else
		{
			button = Language.GetTextValue("Challenge");
			button2 = Language.GetTextValue("Help");
		}
	}

	public override void OnChatButtonClicked(bool firstButton, ref string shopName)
	{
	}

	public override void FindFrame(int frameHeight)
	{
		if (Idle)
		{
			if (Sit)
			{
				NPC.frame.Y = 560;
			}
			else
			{
				NPC.frame.Y = 0;
			}
		}
		if (Attacking0)
		{
			NPC.frame.Y = 672;
		}
		base.FindFrame(frameHeight);
	}

	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		Texture2D texMain = ModAsset.TeahouseLady.Value;
		Vector2 drawPos = NPC.Center - screenPos + new Vector2(0, NPC.height - NPC.frame.Height + 8) * 0.5f;
		Main.spriteBatch.Draw(texMain, drawPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() * 0.5f, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

		if (Attacking0)
		{
			Rectangle armFrame = new Rectangle(0, 730, 8, 18);
			if (NPC.spriteDirection == 1)
			{
				armFrame = new Rectangle(10, 730, 8, 18);
			}
			if (Attack0Target is >= 0 and < 200)
			{
				NPC target = Main.npc[Attack0Target];

				// float rotationAttack = (Main.MouseWorld - NPC.Center).ToRotationSafe() - MathHelper.PiOver2;
				float rotationAttack = (target.Center - NPC.Center).ToRotationSafe() - MathHelper.PiOver2;
				Main.spriteBatch.Draw(texMain, drawPos + new Vector2(0, 0), armFrame, drawColor, rotationAttack, new Vector2(4, 4), NPC.scale, SpriteEffects.None, 0);
				//Texture2D mark = ModAsset.Schorl_Mark.Value;
				//Main.EntitySpriteDraw(mark, target.Center - Main.screenPosition, null, new Color(1f, 1f, 1f, 0), target.rotation, mark.Size() * 0.5f, target.scale, SpriteEffects.None, 0);
			}
		}

		// Point checkPoint = (NPC.Bottom + new Vector2(8 * NPC.direction, 8)).ToTileCoordinates() + new Point(NPC.direction, -1);
		// Tile tile = Main.tile[checkPoint];
		// Texture2D block = Commons.ModAsset.TileBlock.Value;
		// Main.spriteBatch.Draw(block, checkPoint.ToWorldCoordinates() - Main.screenPosition, null, new Color(1f, 0f, 0f, 0.5f), 0, block.Size() * 0.5f, 1, SpriteEffects.None, 0);
		return false;
	}

	public override bool CheckActive()
	{
		return canDespawn;
	}
}