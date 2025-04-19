using Everglow.Commons.Coroutines;
using Everglow.SubSpace;
using Everglow.Yggdrasil.YggdrasilTown.Projectiles.TownNPCAttack;
using SubworldLibrary;
using Terraria.DataStructures;
using Terraria.GameContent.Personalities;
using Terraria.Localization;
using static Everglow.Commons.Utilities.NPCUtils;

namespace Everglow.Yggdrasil.YggdrasilTown.NPCs.TownNPCs;

[AutoloadHead]
public class CanteenMaid : ModNPC
{
	/// <summary>
	/// Control specific behaviors; Control by _townNPCGeneralCoroutine.
	/// </summary>
	public CoroutineManager _townNPCBehaviorCoroutine = new CoroutineManager();

	/// <summary>
	/// Manage behavior pool.
	/// </summary>
	public CoroutineManager _townNPCGeneralCoroutine = new CoroutineManager();

	/// <summary>
	/// Behaviors pool, waiting for _townNPCBehaviorCoroutine to execute.
	/// </summary>
	public Queue<Coroutine> BehaviorsCoroutines = new Queue<Coroutine>();

	/// <summary>
	/// A scalars for the total threaten value;
	/// </summary>
	public float TotalThreaten = 0f;

	/// <summary>
	/// A vector2 for total threaten direction;
	/// </summary>
	public Vector2 TotalThreatenDirection = Vector2.zeroVector;

	public Point AnchorForBehaviorPos => new Point(146, 148);

	public bool Idle = true;
	public bool Talking = false;
	public bool Sit = false;
	public int FrameHeight = 52;
	public int ThreatenTarget = -1;
	public int AttackTimer = -1;
	private int aiMainCount = 0;

	public Vector2 AttackVelocity = Vector2.zeroVector;

	public override string HeadTexture => ModAsset.CanteenMaid_Head_Mod;

	public override void SetStaticDefaults()
	{
		Main.npcFrameCount[NPC.type] = 13;
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
				for (int i = 0; i < 8; i++)
				{
					Vector2 checkPos = npc.Center;
					switch (i)
					{
						case 0:
							checkPos = npc.TopLeft;
							break;
						case 1:
							checkPos = npc.Top;
							break;
						case 2:
							checkPos = npc.TopRight;
							break;
						case 3:
							checkPos = npc.Left;
							break;
						case 4:
							checkPos = npc.Right;
							break;
						case 5:
							checkPos = npc.BottomLeft;
							break;
						case 6:
							checkPos = npc.Bottom;
							break;
						case 7:
							checkPos = npc.BottomRight;
							break;
					}
					if ((checkPos - NPC.Center).Length() < distance.Length())
					{
						distance = checkPos - NPC.Center;
					}
				}
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

	public void TryAttack()
	{
		int attackIndex = Main.rand.Next(4);
		for (int i = -1; i < attackIndex; i++)
		{
			if (CanAttack(attackIndex))
			{
				BehaviorsCoroutines.Enqueue(new Coroutine(Attack(attackIndex)));
				return;
			}
		}
	}

	public bool CanAttack(int type)
	{
		AttackVelocity = Vector2.zeroVector;
		int size = 10;
		switch (type)
		{
			case 0:
				size = 28;
				break;
			case 1:
				size = 18;
				break;
			case 2:
				size = 16;
				break;
			case 3:
				size = 14;
				break;
			default:
				break;
		}
		if (ThreatenTarget is >= 0 and < 200)
		{
			AttackVelocity = GetShootVelocity(Main.npc[ThreatenTarget], new Vector2(size));
			if (AttackVelocity != Vector2.zeroVector)
			{
				return true;
			}
		}
		return false;
	}

	public Vector2 GetShootVelocity(NPC target, Vector2 projSize)
	{
		Vector2 bestVel = Vector2.zeroVector;
		float minDis = 1000;
		for (int r = -5; r < 20; r++)
		{
			int dir = 1;
			if(target.Center.X < NPC.Center.X)
			{
				dir = -1;
			}
			Vector2 shootPoint = NPC.Center + new Vector2(-8 * dir, -16);
			Vector2 distance = target.Center - shootPoint;
			float speed = distance.Length() / 20f;
			speed = Math.Clamp(speed, 6, 30);
			Vector2 vel = distance.NormalizeSafe() * speed;
			if (vel.X < 0)
			{
				vel = vel.RotatedBy(r / 20f);
			}
			else
			{
				vel = vel.RotatedBy(-r / 20f);
			}
			int maxStep = 90;
			var checkPos = shootPoint;
			var checkVel = vel;
			for (int t = 0; t < maxStep; t++)
			{
				if (checkVel.Y <= 21)
				{
					checkVel.Y += 0.6f;
				}
				checkVel *= 0.99f;
				checkPos += checkVel;
				if (Rectangle.Intersect(target.Hitbox, new Rectangle((int)checkPos.X, (int)checkPos.Y, (int)projSize.X, (int)projSize.Y)) != Rectangle.emptyRectangle)
				{
					if((checkPos - target.Center).Length() < minDis)
					{
						minDis = (checkPos - target.Center).Length();
						bestVel = vel;
					}
				}
				if (Collision.SolidCollision(checkPos, (int)projSize.X, (int)projSize.Y))
				{
					break;
				}
			}
		}
		return bestVel;
	}

	public IEnumerator<ICoroutineInstruction> Attack(int type)
	{
		bool safe = false;
		if (ThreatenTarget is >= 0 and < 200 && AttackVelocity != Vector2.zeroVector)
		{
			safe = true;
		}
		if (!safe)
		{
			EndAIPiece();
			yield break;
		}
		NPC target = Main.npc[ThreatenTarget];
		if (target.Center.X > NPC.Center.X)
		{
			NPC.direction = 1;
		}
		if (target.Center.X < NPC.Center.X)
		{
			NPC.direction = -1;
		}
		NPC.spriteDirection = NPC.direction;
		for (int t = 0; t < 10; t++)
		{
			AttackTimer = t;
			NPC.frame = new Rectangle(0, 0, 38, FrameHeight);
			NPC.velocity.X *= 0;
			Vector2 shootPos = NPC.Center + new Vector2(-8 * NPC.direction, -16);
			if (t == 4)
			{
				switch (type)
				{
					case 0:
						Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), shootPos, AttackVelocity, ModContent.ProjectileType<Betty_Watermelon>(), 15, 4f);
						break;
					case 1:
						Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), shootPos, AttackVelocity, ModContent.ProjectileType<Betty_Plate>(), 6, 1f);
						break;
					case 2:
						Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), shootPos, AttackVelocity, ModContent.ProjectileType<Betty_Fork>(), 42, 0.05f);
						break;
					case 3:
						Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), shootPos, AttackVelocity, ModContent.ProjectileType<Betty_Apple>(), 4, 0.4f);
						break;
					default:
						break;
				}
			}
			yield return new SkipThisFrame();
		}
		AttackTimer = -1;
		EndAIPiece();
	}

	public bool CanEscape()
	{
		// Main.NewText(TotalThreaten);
		// Main.NewText(MathF.Abs(TotalThreatenDirection.X), Color.Red);
		if (!CanContinueWalk(NPC))
		{
			return false;
		}
		if (TotalThreaten > MathF.Abs(TotalThreatenDirection.X) * 3f)
		{
			return false;
		}
		var npcTilePos = NPC.Center.ToTileCoordinates();
		if (npcTilePos.X < AnchorForBehaviorPos.X - 90 && TotalThreatenDirection.X < 0)
		{
			return false;
		}
		if (npcTilePos.X > AnchorForBehaviorPos.X + 90 && TotalThreatenDirection.X > 0)
		{
			return false;
		}
		return true;
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

	public void CheckInSuitableArea()
	{
		if (SubworldSystem.Current is not RoomWorld)
		{
			return;
		}
		bool safe = false;
		var homePoint = AnchorForBehaviorPos;
		NPC.homeless = false;
		NPC.homeTileX = homePoint.X;
		NPC.homeTileY = homePoint.Y;

		var npcTilePos = NPC.Center.ToTileCoordinates();
		if (npcTilePos.X is > 100 and < 200)
		{
			if (npcTilePos.Y is > 100 and < 200)
			{
				safe = true;
			}
		}
		if (!safe)
		{
			NPC.Center = homePoint.ToWorldCoordinates() + new Vector2(0, 48);
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

	/// <summary>
	/// None-Inheritable
	/// </summary>
	/// <returns></returns>
	public IEnumerator<ICoroutineInstruction> BehaviorControl()
	{
		while (true)
		{
			CheckInSuitableArea();
			CheckDuplicatedSelf();
			aiMainCount++;
			if (BehaviorsCoroutines.Count > 0 && Idle)
			{
				Idle = false;
				_townNPCBehaviorCoroutine.StartCoroutine(BehaviorsCoroutines.First());
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
			NPC.frame = new Rectangle(0, 676, 38, 52);
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
		for (int t = 0; t < time; t++)
		{
			NPC.spriteDirection = NPC.direction;
			NPC.frame = new Rectangle(0, 0, 38, FrameHeight);
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
		NPC.direction = ChooseDirection(NPC);
		for (int t = 0; t < time; t++)
		{
			// Too far from home will trigger teleportation.
			var npcTilePos = NPC.Center.ToTileCoordinates();
			if (npcTilePos.X < AnchorForBehaviorPos.X - 90)
			{
				NPC.direction = 1;
			}
			if (npcTilePos.X > AnchorForBehaviorPos.X + 90)
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
		NPC.frame = new Rectangle(0, 0, 38, FrameHeight);
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
		base.FindFrame(frameHeight);
	}

	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		Texture2D texMain = ModAsset.CanteenMaid_walk.Value;
		Vector2 drawPos = NPC.Center - screenPos + new Vector2(0, NPC.height - NPC.frame.Height + 4) * 0.5f;
		if (AttackTimer > 0)
		{
			var attackFrame = new Rectangle(0, 728, 38, 52);
			Main.spriteBatch.Draw(texMain, drawPos, attackFrame, drawColor, NPC.rotation, attackFrame.Size() * 0.5f, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
			var armFrame = new Rectangle(0, 782, 12, 20);
			if (NPC.spriteDirection == 1)
			{
				armFrame = new Rectangle(14, 782, 12, 20);
			}
			var armRot = NPC.rotation + AttackTimer * 0.2f * NPC.spriteDirection + MathF.PI;
			Main.spriteBatch.Draw(texMain, drawPos, armFrame, drawColor, armRot, new Vector2(6, 4), NPC.scale, SpriteEffects.None, 0);
		}
		else
		{
			Main.spriteBatch.Draw(texMain, drawPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() * 0.5f, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
		}
		return false;
	}

	public override bool CheckActive()
	{
		return false;
	}
}