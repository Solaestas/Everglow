using Everglow.Commons.Coroutines;
using Everglow.Commons.DataStructures;
using Everglow.Yggdrasil.YggdrasilTown.Projectiles;
using SubworldLibrary;
using Terraria.DataStructures;
using Terraria.GameContent.Personalities;
using Terraria.Localization;
using static Everglow.Commons.Utilities.NPCUtils;

namespace Everglow.Yggdrasil.YggdrasilTown.NPCs.TownNPCs;

[AutoloadHead]
public class Guard_of_YggdrasilTown : ModNPC
{
	private bool canDespawn = false;
	private int aiMainCount = 0;

	public CoroutineManager _townNPCBehaviorCoroutine = new CoroutineManager();
	public CoroutineManager _townNPCGeneralCoroutine = new CoroutineManager();
	public Queue<Coroutine> BehaviorsCoroutines = new Queue<Coroutine>();
	public bool Idle = true;
	public bool Talking = false;
	public bool Sit = false;
	public int ThreatenTarget = -1;
	public int FrameHeight = 56;

	/// <summary>
	/// A scalars for the total threaten value;
	/// </summary>
	public float TotalThreaten = 0f;

	/// <summary>
	/// A vector2 for total threaten direction;
	/// </summary>
	public Vector2 TotalThreatenDirection = Vector2.zeroVector;

	/// <summary>
	/// 0 means non-attacking, while 1 means attack style 0, 2 means style 1.
	/// </summary>
	public int TextureStyle = 0;
	public int MySlimyWhoAmI = -1;

	public int Attack0Cooling = 0;
	public int Attack1Cooling = 0;

	/// <summary>
	/// Spear direction during attack style 0. 0 means forward, 1 means backward.
	/// </summary>
	public int Attack0Direction = 0;
	public Projectile SpearProjectile = null;
	public Projectile FistProjectile = null;
	public Vector2 LockCenter = Vector2.Zero;

	public Point AnchorForBehaviorPos => YggdrasilTownCentralSystem.TownTopLeftWorldCoord.ToTileCoordinates() + new Point(665, 154);

	public override string HeadTexture => ModAsset.Guard_of_YggdrasilTown_Head_Mod;

	public override void SetStaticDefaults()
	{
		Main.npcFrameCount[NPC.type] = 15;
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
		NH.SetBiomeAffection<YggdrasilTownBiome>((AffectionLevel)50);
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
			if (BehaviorsCoroutines.Count <= 0)
			{
				Idle = true;
				if (Peace())
				{
					if (Sit)
					{
						BehaviorsCoroutines.Enqueue(new Coroutine(SitDown(Main.rand.Next(60, 90))));
					}
					else
					{
						if (Main.rand.NextBool())
						{
							BehaviorsCoroutines.Enqueue(new Coroutine(Stand(Main.rand.Next(60, 90))));
						}
						else
						{
							BehaviorsCoroutines.Enqueue(new Coroutine(Walk(Main.rand.Next(60, 900))));
						}
					}
				}
				else if (CanEscape())
				{
					Sit = false;
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
					Sit = false;
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

	/// <summary>
	/// Analysis surrounding threaten sources.
	/// </summary>
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
		// Main.NewText(TotalThreaten);
		// Main.NewText(MathF.Abs(TotalThreatenDirection.X), Color.Red);
		if(!CanContinueWalk(NPC))
		{
			return false;
		}
		if (TotalThreaten > MathF.Abs(TotalThreatenDirection.X) * 3f)
		{
			return false;
		}
		var npcTilePos = NPC.Center.ToTileCoordinates();
		if (npcTilePos.X < AnchorForBehaviorPos.X - 54 && TotalThreatenDirection.X < 0)
		{
			return false;
		}
		if (npcTilePos.X > AnchorForBehaviorPos.X + 54 && TotalThreatenDirection.X > 0)
		{
			return false;
		}
		return true;
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

	public override bool CheckConditions(int left, int right, int top, int bottom)
	{
		int score = ((right - left) * (bottom - top)) / 2;

		return score > float.PositiveInfinity;
	}

	/// <summary>
	/// This NPC will always be with her pet slime.
	/// </summary>
	public void CheckSlimy()
	{
		if (MySlimyWhoAmI == -1 || !Main.npc[MySlimyWhoAmI].active || Main.npc[MySlimyWhoAmI].type != ModContent.NPCType<Guard_Slimy>())
		{
			var slimy = NPC.NewNPCDirect(NPC.GetSource_FromAI(), NPC.Center, ModContent.NPCType<Guard_Slimy>());
			MySlimyWhoAmI = slimy.whoAmI;
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

	public bool CanAttack0()
	{
		if (Attack0Cooling > 0)
		{
			return false;
		}
		if (BehaviorsCoroutines.Count > 1)
		{
			return false;
		}
		foreach (var npc in Main.npc)
		{
			if (!npc.friendly && !npc.dontTakeDamage && npc.active && npc.life > 0 && npc.type != NPCID.TargetDummy && !npc.CountsAsACritter)
			{
				Vector2 distance = npc.Center - NPC.Center;
				if (MathF.Abs(distance.X) < 120 && MathF.Abs(distance.Y) < 120)
				{
					float angle = MathF.Atan2(distance.Y, distance.X);
					if ((angle < MathF.PI / 4 && angle > -MathF.PI * 2 / 9) || angle > MathF.PI * 3 / 4 || angle < -MathF.PI * 7 / 9)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public Vector2 ChooseAttack0Direction()
	{
		Vector2 nearestDir = Vector2.zeroVector;
		float minDis = 300;
		foreach (var npc in Main.npc)
		{
			if (!npc.friendly && !npc.dontTakeDamage && npc.active && npc.life > 0 && npc.type != NPCID.TargetDummy && !npc.CountsAsACritter)
			{
				Vector2 distance = npc.Center - NPC.Center;
				if (MathF.Abs(distance.X) < 120 && MathF.Abs(distance.Y) < 120 && distance.Length() < minDis)
				{
					float angle = MathF.Atan2(distance.Y, distance.X);
					if ((angle < MathF.PI / 4 && angle > -MathF.PI * 2 / 9) || angle > MathF.PI * 3 / 4 || angle < -MathF.PI * 7 / 9)
					{
						minDis = distance.Length();
						nearestDir = distance;
					}
				}
			}
		}
		return nearestDir;
	}

	public bool CanAttack1()
	{
		if (Attack1Cooling > 0)
		{
			return false;
		}
		if (BehaviorsCoroutines.Count > 1)
		{
			return false;
		}
		foreach (var npc in Main.npc)
		{
			if (!npc.friendly && !npc.dontTakeDamage && npc.active && npc.type != NPCID.TargetDummy && npc.life > 0 && !npc.CountsAsACritter)
			{
				Vector2 distance = npc.Center - NPC.Center;
				if (MathF.Abs(distance.X) < 120 && MathF.Abs(distance.Y) < 50)
				{
					return true;
				}
			}
		}
		return false;
	}

	public int ChooseAttack1Direction()
	{
		int direction = -1;
		float minDis = 120;
		foreach (var npc in Main.npc)
		{
			if (!npc.friendly && !npc.dontTakeDamage && npc.active && npc.type != NPCID.TargetDummy && npc.life > 0 && !npc.CountsAsACritter)
			{
				Vector2 distance = npc.Center - NPC.Center;
				if (MathF.Abs(distance.X) < 120 && MathF.Abs(distance.Y) < 50 && distance.Length() < minDis)
				{
					direction = MathF.Sign(distance.X);
					minDis = distance.Length();
				}
			}
		}
		return direction;
	}

	public void CheckInSuitableArea()
	{
		bool safe = false;
		if (SubworldSystem.Current is not YggdrasilWorld)
		{
			NPC.active = false;
			return;
		}
		var homePoint = AnchorForBehaviorPos;
		NPC.homeless = false;
		NPC.homeTileX = homePoint.X + 4;
		NPC.homeTileY = homePoint.Y;

		if (YggdrasilTownCentralSystem.TownPos(NPC.Center).X is > 9904 and < 11286)
		{
			if (YggdrasilTownCentralSystem.TownPos(NPC.Center).Y is > 2171 and < 2850)
			{
				safe = true;
			}
		}
		if (!safe)
		{
			NPC.Center = AnchorForBehaviorPos.ToWorldCoordinates();
		}
	}

	public void CheckDuplicatedSelf()
	{
		foreach (NPC npc in Main.npc)
		{
			if (npc != null && npc.type == Type && npc.active && npc != NPC)
			{
				npc.active = false;
			}
			if (NPC.CountNPCS(Type) <= 1)
			{
				break;
			}
		}
	}

	public IEnumerator<ICoroutineInstruction> BehaviorControl()
	{
		while (true)
		{
			CheckInSuitableArea();
			CheckSlimy();
			CheckDuplicatedSelf();
			aiMainCount++;
			if (BehaviorsCoroutines.Count > 0 && Idle)
			{
				_townNPCBehaviorCoroutine.StartCoroutine(BehaviorsCoroutines.First());
				Idle = false;
			}
			if (Attack0Cooling > 0)
			{
				Attack0Cooling--;
			}
			if (Attack1Cooling > 0)
			{
				Attack1Cooling--;
			}
			if (aiMainCount >= 2)
			{
				yield break;
			}
			yield return new SkipThisFrame();
		}
	}

	/// <summary>
	/// Sitting silently; Inheritable
	/// </summary>
	/// <param name="time"></param>
	/// <returns></returns>
	public IEnumerator<ICoroutineInstruction> SitDown(int time)
	{
		Sit = true;
		for (int t = 0; t < time; t++)
		{
			NPC.frame = new Rectangle(0, 560, 48, 56);
			NPC.velocity.X = 0;
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
				break;
			}
			yield return new SkipThisFrame();
		}
		Sit = false;
		EndAIPiece();
	}

	/// <summary>
	/// Standing silently; Inheritable
	/// </summary>
	/// <param name="time"></param>
	/// <returns></returns>
	public IEnumerator<ICoroutineInstruction> Stand(int time)
	{
		TextureStyle = 0;
		for (int t = 0; t < time; t++)
		{
			NPC.spriteDirection = NPC.direction;
			NPC.frame = new Rectangle(0, 616, 48, FrameHeight);
			NPC.velocity.X = 0;
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
				break;
			}
			yield return new SkipThisFrame();
		}
		EndAIPiece();
	}

	/// <summary>
	/// Walking, include escape under duress, jump in front of a protruding tile, door opening(closing), check chair for sitting; Inheritable
	/// </summary>
	/// <param name="time"></param>
	/// <returns></returns>
	public IEnumerator<ICoroutineInstruction> Walk(int time)
	{
		TextureStyle = 0;
		NPC.direction = ChooseDirection(NPC);
		for (int t = 0; t < time; t++)
		{
			// Too far from home will trigger teleportation.
			var npcTilePos = NPC.Center.ToTileCoordinates();
			if (npcTilePos.X < AnchorForBehaviorPos.X - 54)
			{
				NPC.direction = 1;
			}
			if (npcTilePos.X > AnchorForBehaviorPos.X + 54)
			{
				NPC.direction = -1;
			}
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

			if (Main.rand.NextBool(24))
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
	/// Spear
	/// </summary>
	/// <returns></returns>
	public IEnumerator<ICoroutineInstruction> Attack0()
	{
		TextureStyle = 1;
		Attack0Cooling = 26;
		Vector2 distance = ChooseAttack0Direction();
		if (distance.X == 0 && distance.Y == 0)
		{
			distance.X = 1f;
		}
		NPC.direction = distance.X > 0 ? 1 : -1;
		NPC.spriteDirection = NPC.direction;
		NPC.frame = new Rectangle(0, 0, 76, 62);
		NPC.frameCounter = 0;
		var proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0, 8), distance, ModContent.ProjectileType<Guard_Attack_Spear>(), NPC.damage, 2, Main.myPlayer, NPC.whoAmI);
		SpearProjectile = proj;
		for (int t = 0; t < 22; t++)
		{
			NPC.spriteDirection = NPC.direction;
			NPC.velocity *= 0;
			NPC.frameCounter++;
			if (NPC.frame.Y == 124)
			{
				if (NPC.frameCounter == 7)
				{
					NPC.frame.Y -= 62;
					NPC.frameCounter = 0;
					Attack0Direction = 1;
				}
			}
			else if (NPC.frameCounter == 5)
			{
				if (Attack0Direction == 1)
				{
					NPC.frame.Y -= 62;
				}
				else
				{
					NPC.frame.Y += 62;
				}
				NPC.frameCounter = 0;
			}

			if (NPC.frame.Y == 0 && Attack0Direction == 1)
			{
				Attack0Direction = 0;
				break;
			}
			yield return new SkipThisFrame();
		}
		NPC.frame = new Rectangle(0, 0, 40, 56);
		SpearProjectile = null;
		TextureStyle = 0;
		yield return new WaitForFrames(16);
		EndAIPiece();
	}

	/// <summary>
	/// punch
	/// </summary>
	/// <returns></returns>
	public IEnumerator<ICoroutineInstruction> Attack1()
	{
		TextureStyle = 2;
		Attack1Cooling = 1;
		int direction = ChooseAttack1Direction();
		NPC.direction = direction;
		NPC.spriteDirection = direction;
		NPC.frame = new Rectangle(0, 0, 46, 46);
		for (int t = 0; t < 12; t++)
		{
			if(t == 4)
			{
				var proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, new Vector2(direction, 0), ModContent.ProjectileType<Guard_Attack_Fist>(), NPC.damage, 2, Main.myPlayer, NPC.whoAmI);
				NPC.frame = new Rectangle(0, 46, 46, 46);
			}
			if (t == 8)
			{
				NPC.frame = new Rectangle(0, 92, 46, 46);
			}
			NPC.direction = direction;
			NPC.spriteDirection = NPC.direction;
			NPC.velocity *= 0;
			yield return new SkipThisFrame();
		}
		FistProjectile = null;
		NPC.frame = new Rectangle(0, 0, 40, 56);
		TextureStyle = 0;
		EndAIPiece();
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
				NPC.frame.Y = 784;
			}
			else
			{
				NPC.frame.Y = 0;
			}
		}
		base.FindFrame(frameHeight);
	}

	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		Vector2 drawPos = NPC.Center - screenPos + new Vector2(0, NPC.height - NPC.frame.Height + 8) * 0.5f;
		if (TextureStyle == 0)
		{
			Texture2D texMain = ModAsset.Guard_of_YggdrasilTown.Value;
			Main.spriteBatch.Draw(texMain, drawPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() * 0.5f, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
		}
		else if (TextureStyle == 1)
		{
			Texture2D texBody = ModAsset.Guard_of_YggdrasilTown_attack_body.Value;
			Texture2D texArm = ModAsset.Guard_of_YggdrasilTown_attack_arm.Value;
			Texture2D spear = ModAsset.Guard_Attack_Spear.Value;
			var spearRect = new Rectangle(0, 0, 10, 64);
			Vector2 spearPos = SpearProjectile.Center - screenPos - Vector2.Normalize(SpearProjectile.velocity) * 32f * 1.2f;

			Main.spriteBatch.Draw(texBody, drawPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() * 0.5f, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

			// 长枪要放到 arm 渲染之前渲染
			Main.spriteBatch.Draw(spear, spearPos, spearRect, drawColor, SpearProjectile.rotation, spearRect.Size() * 0.5f, 1.2f, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
			spearPos = SpearProjectile.Center + Vector2.Normalize(SpearProjectile.velocity) * 20f;
			float timeValue = 1f;
			int duration = 22;
			float halfDuration = duration * 0.5f;
			if (SpearProjectile.timeLeft < halfDuration)
			{
				timeValue = SpearProjectile.timeLeft / 20f;
			}
			if (SpearProjectile.timeLeft < 2)
			{
				timeValue = 0f;
			}
			if (SpearProjectile.timeLeft >= halfDuration)
			{
				LockCenter = spearPos;
			}
			var vel = Vector2.Normalize(SpearProjectile.velocity);
			Vector2 width = vel.RotatedBy(MathF.PI * 0.5) * 90;
			Color color = drawColor;
			color.A = 0;
			int trailLength = 16;
			float timeEffectValue = (float)(Main.time * 0.10f);
			var bars = new List<Vertex2D>();
			for (int x = 0; x < trailLength; x++)
			{
				float value = 0.8f;
				if (x > 10)
				{
					value = (15 - x) / 5f;
				}
				else if (x <= 6)
				{
					value = 0.8f + x / 5f;
				}
				else if (x > 6 && x <= 10)
				{
					value = 0.8f + (10 - x) / 4f;
				}
				color.A = (byte)(255 * timeValue);
				bars.Add(LockCenter - vel * 6 * x + width, color * value, new Vector3(x / 15f - timeEffectValue, 1, MathF.Sin(x / 16f)));
				bars.Add(LockCenter - vel * 6 * x - width, color * value, new Vector3(x / 15f - timeEffectValue, 0, MathF.Sin(x / 16f)));
			}

			color = drawColor;
			var barsHighLight = new List<Vertex2D>();
			for (int x = 0; x < trailLength; x++)
			{
				float value = 1;
				if (x > trailLength - 12)
				{
					value = (trailLength - 1 - x) / 11f;
				}
				color.A = (byte)(value * 255 * timeValue);
				barsHighLight.Add(LockCenter - vel * 6 * x + width * 0.4f, color, new Vector3(x / 30f - timeEffectValue, 1, MathF.Sin(x / 8f)));
				barsHighLight.Add(LockCenter - vel * 6 * x - width * 0.4f, color, new Vector3(x / 30f - timeEffectValue, 0, MathF.Sin(x / 8f)));
			}

			SpriteBatchState sBS = Main.spriteBatch.GetState().Value;
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
			Effect effect = Commons.ModAsset.StabSwordEffect.Value;
			var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
			var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition, 0)) * Main.GameViewMatrix.TransformationMatrix;
			effect.Parameters["uTransform"].SetValue(model * projection);
			effect.Parameters["uProcession"].SetValue(0.5f);
			effect.CurrentTechnique.Passes[0].Apply();

			Main.graphics.graphicsDevice.Textures[0] = Commons.ModAsset.Trail_3.Value;
			Main.graphics.graphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
			Main.graphics.graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
			effect = Commons.ModAsset.Trailing.Value;
			effect.Parameters["uTransform"].SetValue(model * projection);
			effect.CurrentTechnique.Passes["HeatMap"].Apply();

			Main.graphics.graphicsDevice.Textures[0] = Commons.ModAsset.Trail_1.Value;
			Main.graphics.graphicsDevice.Textures[1] = ModAsset.Guard_Attack_Spear_heatMap.Value;
			Main.graphics.graphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
			Main.graphics.graphicsDevice.SamplerStates[1] = SamplerState.PointClamp;
			Main.graphics.graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, barsHighLight.ToArray(), 0, barsHighLight.Count - 2);

			Main.spriteBatch.End();

			Main.spriteBatch.Begin(sBS);
			Main.spriteBatch.Draw(texArm, drawPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() * 0.5f, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
		}
		else if (TextureStyle == 2)
		{
			Texture2D texMain = ModAsset.Guard_of_YggdrasilTown_Punch.Value;
			Main.spriteBatch.Draw(texMain, drawPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() * 0.5f, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
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