using Everglow.Commons.Coroutines;
using Terraria.DataStructures;
using Terraria.GameContent.Personalities;
using Terraria.Localization;
using static Everglow.Commons.Utilities.NPCUtils;

namespace Everglow.Yggdrasil.YggdrasilTown.NPCs;

[AutoloadHead]
public class CanteenMaid : ModNPC
{
	private bool canDespawn = false;
	private int aiMainCount = 0;

	public CoroutineManager _townNPCBehaviorCoroutine = new CoroutineManager();
	public CoroutineManager _townNPCGeneralCoroutine = new CoroutineManager();
	public Queue<Coroutine> AICoroutines = new Queue<Coroutine>();
	public bool Idle = true;
	public bool Talking = false;
	public bool Sit = false;
	public int FrameHeight = 52;

	public int MySlimyWhoAmI = -1;

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
		_townNPCGeneralCoroutine.StartCoroutine(new Coroutine(AI_Main()));
		base.OnSpawn(source);
	}

	public override bool PreAI()
	{
		aiMainCount = 0;
		return base.PreAI();
	}

	public override void AI()
	{
		Idle = true;
		_townNPCBehaviorCoroutine.Update();
		_townNPCGeneralCoroutine.Update();
		if (!Talking)
		{
			if (Main.rand.NextBool(120) && AICoroutines.Count <= 1)
			{
				AICoroutines.Enqueue(new Coroutine(Walk(Main.rand.Next(60, 900))));
			}
			if (Main.rand.NextBool(120) && AICoroutines.Count <= 1)
			{
				AICoroutines.Enqueue(new Coroutine(Stand(Main.rand.Next(60, 900))));
			}
			if ((int)(Main.time * 0.1f) % 6 == 0)
			{
				if (AICoroutines.Count <= 1)
				{
					if (CanAttack0())
					{
						AICoroutines.Enqueue(new Coroutine(Attack0()));
					}
					if (CanAttack1())
					{
						AICoroutines.Enqueue(new Coroutine(Attack1()));
					}
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
			_townNPCGeneralCoroutine.StartCoroutine(new Coroutine(AI_Main()));
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
		if (AICoroutines.Count > 1)
		{
			return false;
		}
		foreach (var npc in Main.npc)
		{
			if (!npc.friendly && !npc.dontTakeDamage && npc.active && npc.life > 0 && npc.type != NPCID.TargetDummy && !npc.CountsAsACritter)
			{
				Vector2 distance = npc.Center - NPC.Center;
				if (MathF.Abs(distance.X) < 240 && distance.Y < 0 && distance.Y > -300)
				{
					if (npc.Center.X > NPC.Center.X)
					{
						NPC.direction = 1;
					}
					else
					{
						NPC.direction = -1;
					}
					NPC.spriteDirection = NPC.direction;
					return true;
				}
			}
		}
		return false;
	}

	public int ChooseAttack0Direction()
	{
		int nearestDir = 1;
		float minDis = 300;
		foreach (var npc in Main.npc)
		{
			if (!npc.friendly && !npc.dontTakeDamage && npc.active && npc.life > 0 && npc.type != NPCID.TargetDummy && !npc.CountsAsACritter)
			{
				Vector2 distance = npc.Center - NPC.Center;
				if (MathF.Abs(distance.X) < 240 && distance.Y < 0 && distance.Y > -300 && distance.Length() < minDis)
				{
					minDis = distance.Length();
					if (npc.Center.X > NPC.Center.X)
					{
						nearestDir = 1;
					}
					else
					{
						nearestDir = -1;
					}
				}
			}
		}
		return nearestDir;
	}

	public bool CanAttack1()
	{
		foreach (var npc in Main.npc)
		{
			if (!npc.friendly && !npc.dontTakeDamage && npc.active && npc.life > 0 && npc.type != NPCID.TargetDummy && !npc.CountsAsACritter)
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

	public IEnumerator<ICoroutineInstruction> AI_Main()
	{
		while (true)
		{
			aiMainCount++;
			if (AICoroutines.Count > 0 && Idle)
			{
				_townNPCBehaviorCoroutine.StartCoroutine(AICoroutines.First());
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
			NPC.spriteDirection = NPC.direction;
			NPC.velocity.X = NPC.direction * 1.2f;
			Idle = false;
			NPC.frameCounter += Math.Abs(NPC.velocity.X);
			if (NPC.frameCounter > 4)
			{
				NPC.frame.Y += FrameHeight;
				NPC.frameCounter = 0;
			}
			if (NPC.frame.Y > 12 * FrameHeight)
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
			if (CanAttack0())
			{
				AICoroutines.Enqueue(new Coroutine(Attack0()));
				break;
			}
			if (CanAttack1())
			{
				AICoroutines.Enqueue(new Coroutine(Attack1()));
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

	public IEnumerator<ICoroutineInstruction> Attack0()
	{
		NPC.direction = ChooseAttack0Direction();
		for (int t = 0; t < 40; t++)
		{
			NPC.spriteDirection = NPC.direction;
			NPC.velocity *= 0;
			yield return new SkipThisFrame();
		}
		NPC.frame = new Rectangle(0, 0, 32, FrameHeight);
		yield return new WaitForFrames(16);
		EndAIPiece();
	}

	public IEnumerator<ICoroutineInstruction> Attack1()
	{
		for (int t = 0; t < 60; t++)
		{
			NPC.spriteDirection = NPC.direction;
			NPC.velocity *= 0;
			Idle = false;
			yield return new SkipThisFrame();
		}
		NPC.direction *= -1;
		NPC.spriteDirection = NPC.direction;
		NPC.frame = new Rectangle(0, 0, 38, FrameHeight);
		EndAIPiece();
	}

	public IEnumerator<ICoroutineInstruction> Stand(int time)
	{
		for (int t = 0; t < time; t++)
		{
			NPC.spriteDirection = NPC.direction;
			NPC.frame = new Rectangle(0, 0, 38, FrameHeight);
			NPC.velocity.X = 0;
			Idle = false;
			if (CanAttack0())
			{
				AICoroutines.Enqueue(new Coroutine(Attack0()));
				break;
			}
			if (CanAttack1())
			{
				AICoroutines.Enqueue(new Coroutine(Attack1()));
				break;
			}
			yield return new SkipThisFrame();
		}
		EndAIPiece();
	}

	public void EndAIPiece()
	{
		AICoroutines.Dequeue();
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
		Texture2D texMain = ModAsset.CanteenMaid.Value;
		Vector2 drawPos = NPC.Center - screenPos + new Vector2(0, NPC.height - NPC.frame.Height + 8) * 0.5f;
		Main.spriteBatch.Draw(texMain, drawPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() * 0.5f, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

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