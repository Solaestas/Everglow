using Everglow.Commons.VFX.CommonVFXDusts;
using Everglow.Myth.TheTusk.Items;
using Everglow.Myth.TheTusk.Items.Accessories;
using Everglow.Myth.TheTusk.Items.BossDrop;
using Everglow.Myth.TheTusk.Items.Weapons;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;

namespace Everglow.Myth.TheTusk.NPCs.Bosses.BloodTusk;

[AutoloadBossHead]
public class BloodTusk : ModNPC
{
	public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
	{
		string tex = "It was just a wolf tooth, dropped to the Crimson when its owner was defeated by a hero, gradually corrupted by the power of Cthulhu and granted mentality.";
		if (Language.ActiveCulture.Name == "zh-Hans")
		{
			tex = "原本只是一颗狼牙,在它的主人被勇士讨伐时掉落至猩红之地,逐渐为克苏鲁的力量所沾染,有了自己的意识";
		}

		// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
		bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
		{
			// Sets the spawning conditions of this NPC that is listed in the bestiary.
			BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
			BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson,

			// Sets the description of this NPC that is listed in the bestiary.
			new FlavorTextBestiaryInfoElement(tex),
		});
	}

	private Vector2[] subTuskPosition = new Vector2[10];
	private Vector2[] subTuskMaxDistance = new Vector2[10];
	private int[] isTimeForChangeSubTuskPosition = new int[10];
	private bool subTuskOnSquzze = false;
	private bool subTuskOnStretch = false;
	private bool transparent = false;
	private int firstIcon = 9999999;
	private int nPCDamageBuffer = 0;
	public NPC[] FlyingTentacleTusks = new NPC[4];
	public Vector2 LookingCenter;
	public int secondStageHeadSlot = -1;
	private int phaseChange = 200;
	private Vector2 aimPosHeight = new Vector2(0, -250);
	private Vector2 originalPos = new Vector2(0, 0);
	private Vector2 originalPosII = new Vector2(0, 0);
	private Vector2 basePos = Vector2.Zero;
	private Vector2 firstPos = Vector2.Zero;
	private int pullTime = 0; // 拉动围墙向中夹的倒计时
	private int startTime = 0;
	private int spawnNPCPointX = 0;
	private bool[] back = new bool[4];
	public bool NoTentacle = true;
	private int maxFlyingTentaclesCount = 0;
	private bool hasbeenKilled = false;
	public int Killing = 0;
	private bool startFight = false;
	private bool reallyStart = false;
	private int fightT = 0;
	private int hasTranSkin = 240;
	private int[] eye = new int[7];
	private int[] eyeMotivate = new int[7];
	private float[] eyeRot = new float[7];
	private Vector2[] eyeMove = new Vector2[7];
	private Vector2 tuskHitMove = Vector2.Zero;
	private float sprK = 0.9f;
	private Vector2[] hangItem = new Vector2[8];
	private Vector2[] mouth1 = new Vector2[400];
	private Vector2[] mouth2 = new Vector2[400];
	private Vector2[] oldMouth1 = new Vector2[400];
	private Vector2[] oldMouth2 = new Vector2[400];
	private Vector2 mouth1Vel = new Vector2(1, 0);
	private Vector2 mouth2Vel = new Vector2(-1, 0);
	private float[] hangMaxL1 = new float[400];
	private float[] hangMaxL2 = new float[400];
	public Vector2 DarkCenter;
	public Vector2[] FogSpace = new Vector2[20];
	public bool CheckUpdate = false;

	public override void SetDefaults()
	{
		NPC.behindTiles = true;
		NPC.damage = 0;
		NPC.width = 40;
		NPC.height = 142;
		NPC.defense = 15;
		NPC.lifeMax = 7800;
		if (Main.expertMode)
		{
			NPC.lifeMax = 10200;
			NPC.defense = 20;
		}
		if (Main.masterMode)
		{
			NPC.lifeMax = 13400;
			NPC.defense = 25;
		}
		for (int i = 0; i < NPC.buffImmune.Length; i++)
		{
			NPC.buffImmune[i] = true;
		}
		NPC.knockBackResist = 0f;
		NPC.value = Item.buyPrice(0, 2, 0, 0);
		NPC.color = new Color(0, 0, 0, 0);
		NPC.alpha = 0;
		NPC.aiStyle = -1;
		NPC.boss = true;
		NPC.lavaImmune = true;
		NPC.noGravity = false;
		NPC.noTileCollide = false;
		NPC.HitSound = SoundID.DD2_SkeletonHurt;
		NPC.DeathSound = SoundID.DD2_SkeletonDeath;
		NPC.dontTakeDamage = true;
		Music = Common.MythContent.QuickMusic("TuskBiome");
	}

	public override void OnKill()
	{
		NPC.SetEventFlagCleared(ref DownedBossSystem.downedTusk, -1);
		if (Main.netMode == NetmodeID.Server)
		{
			NetMessage.SendData(MessageID.WorldData);
		}
	}

	public override void Load()
	{
		// We want to give it a second boss head icon, so we register one
		string texture = BossHeadTexture + "_Void"; // Our texture is called "ClassName_Head_Boss_SecondStage"
		secondStageHeadSlot = Mod.AddBossHeadTexture(texture, -1); // -1 because we already have one registered via the [AutoloadBossHead] attribute, it would overwrite it otherwise
	}

	public override void BossHeadSlot(ref int index)
	{
		if (firstIcon == 9999999)
		{
			firstIcon = index;
		}

		int slot = secondStageHeadSlot;
		if (transparent && slot != -1)
		{
			// If the boss is in its second stage, display the other head icon instead
			index = slot;
		}

		if (!transparent)
		{
			// If the boss is in its second stage, display the other head icon instead
			index = firstIcon;
		}
	}

	public override void AI()
	{
		if (hasTranSkin == 239)
		{
			SoundEngine.PlaySound(new SoundStyle("Everglow/Myth/Sounds/TuskCrack") , NPC.Bottom);
		}

		if (DarkCenter == Vector2.Zero)
		{
			int Cx = (int)(NPC.Center.X / 16f);
			int Cy = (int)(NPC.Center.X / 16f);
			for (int i = -100; i < 100; i++)
			{
				for (int j = -100; j < 100; j++)
				{
					if (i + Cx > 20 && i + Cx < Main.maxTilesX - 20 && j + Cy > 20 && j + Cy < Main.maxTilesY - 20)
					{
						if (Main.tile[i + Cx, j + Cy].TileType == ModContent.TileType<Tiles.AbTuskFlesh>())
						{
							DarkCenter = new Vector2(i + Cx, j + Cy);
							break;
						}
					}
				}
				if (DarkCenter != Vector2.Zero)
				{
					break;
				}
			}
			if (DarkCenter == Vector2.Zero)
			{
				DarkCenter = NPC.Bottom;
			}
		}
		/*联机情况下错误排查*/
		if (NPC.Bottom.Y > DarkCenter.Y && Collision.SolidCollision(NPC.Bottom + new Vector2(0, -10), 1, 1))
		{
			NPC.position.Y -= 5f;
		}

		if (firstPos == Vector2.Zero)
		{
			firstPos = NPC.position;
		}

		if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
		{
			NPC.TargetClosest();
		}

		Player player = Main.player[NPC.target];
		startTime++;
		subTuskMaxDistance[0] = new Vector2(14, 18);
		subTuskMaxDistance[1] = new Vector2(-8, 24);
		subTuskMaxDistance[2] = new Vector2(0, 38);
		subTuskMaxDistance[3] = new Vector2(6, 30);
		subTuskMaxDistance[4] = new Vector2(-30, 30);
		subTuskMaxDistance[5] = new Vector2(-10, 34);
		subTuskMaxDistance[6] = new Vector2(12, 48);
		subTuskMaxDistance[7] = new Vector2(16, 32);
		subTuskMaxDistance[8] = new Vector2(0, 132);
		subTuskMaxDistance[9] = new Vector2(0, 30);
		for (int n = 0; n < 10; n++)
		{
			if (fightT < 60)
			{
				subTuskPosition[n] = subTuskPosition[n] * fightT / 60f + subTuskMaxDistance[n] * (60 - fightT) / 60f;
			}
			else
			{
				if (NPC.life >= NPC.lifeMax * 0.5)
				{
					if (NPC.localAI[0] >= 600)
					{
						if (isTimeForChangeSubTuskPosition[n] == 0)
						{
							if (subTuskPosition[n].Length() > 0.5f && n < 8)
							{
								subTuskPosition[n] = subTuskPosition[n] * 0.95f;
							}
							else
							{
								if (Main.rand.NextBool(600) && n < 8)
								{
									isTimeForChangeSubTuskPosition[n] = 1;
								}
							}
						}
						if (isTimeForChangeSubTuskPosition[n] == 1)
						{
							if ((subTuskPosition[n] - subTuskMaxDistance[n]).Length() > 0.5f && n < 8)
							{
								subTuskPosition[n] = subTuskPosition[n] * 0.95f + subTuskMaxDistance[n] * 0.05f;
							}
							else
							{
								if (Main.rand.NextBool(240) && n < 8)
								{
									isTimeForChangeSubTuskPosition[n] = 0;
								}
							}
						}
					}
					else
					{
						if (subTuskOnSquzze)
						{
							subTuskPosition[n] = subTuskPosition[n] * 0.85f + subTuskMaxDistance[n] * 0.15f;
						}

						if (subTuskOnStretch)
						{
							subTuskPosition[n] = subTuskPosition[n] * 0.85f;
						}
					}
				}
				else
				{
					if (NPC.localAI[0] >= 1150 || NPC.localAI[0] <= 850)
					{
						if (isTimeForChangeSubTuskPosition[n] == 0)
						{
							if (subTuskPosition[n].Length() > 0.5f && n < 8)
							{
								subTuskPosition[n] = subTuskPosition[n] * 0.95f;
							}
							else
							{
								if (Main.rand.NextBool(600) && n < 8)
								{
									isTimeForChangeSubTuskPosition[n] = 1;
								}
							}
						}
						if (isTimeForChangeSubTuskPosition[n] == 1)
						{
							if ((subTuskPosition[n] - subTuskMaxDistance[n]).Length() > 0.5f && n < 8)
							{
								subTuskPosition[n] = subTuskPosition[n] * 0.95f + subTuskMaxDistance[n] * 0.05f;
							}
							else
							{
								if (Main.rand.NextBool(240) && n < 8)
								{
									isTimeForChangeSubTuskPosition[n] = 0;
								}
							}
						}
					}
					else
					{
						if (subTuskOnSquzze)
						{
							subTuskPosition[n] = subTuskPosition[n] * 0.85f + subTuskMaxDistance[n] * 0.15f;
						}

						if (subTuskOnStretch)
						{
							subTuskPosition[n] = subTuskPosition[n] * 0.85f;
						}
					}
				}
			}
		}
		if (NPC.velocity.Length() <= 0.5f && startTime > 4 && !startFight)
		{
			for (int i = -3; i < 4; i++)
			{
				Tile tilev = Main.tile[(int)(NPC.Bottom.X / 16f) + i, (int)(NPC.Bottom.Y / 16f)];
				if (tilev.TileType == 19)
				{
					NPC.position.Y += 16;
					return;
				}
			}
			startFight = true;
			NPC.dontTakeDamage = false;
		}
		for (int i = -3; i < 4; i++)
		{
			Tile tilev = Main.tile[(int)(NPC.Bottom.X / 16f) + i, (int)(NPC.Bottom.Y / 16f)];
			if (tilev.TileType == 19)
			{
				NPC.position.Y += 16;
			}
		}
		if (startFight)
		{
			if (reallyStart)
			{
				NPC.damage = 80;
				if (Main.expertMode)
				{
					NPC.damage = 120;
				}

				if (Main.masterMode)
				{
					NPC.damage = 160;
				}

				if (fightT < 60)
				{
					fightT += 1;
					NPC.localAI[0] = 215;
				}
				else
				{
					fightT = 60;
					if (NPC.life >= NPC.lifeMax * 0.5f)
					{
						NPC.localAI[0] += 1;
					}
				}
			}
		}
		if (NPC.life < NPC.lifeMax * 0.5f)
		{
			if (NPC.localAI[0] < 89)
			{
				NPC.localAI[0] += 1;
			}

			if (NPC.localAI[0] >= 89)
			{
				if (pullTime <= 0)
				{
					NPC.localAI[0] += 1;
				}
				else
				{
					pullTime--;
				}
			}

			// 核心AI转化
			if (NPC.localAI[0] < 850)
			{
				if (fightT >= 60)
				{
					if (NPC.localAI[0] > 6000)
					{
						NPC.localAI[0] = 0;
					}
				}
				if (phaseChange > 0)
				{
					NPC.localAI[0] = 0;
					subTuskOnStretch = true;
					phaseChange--;
				}
				else
				{
					subTuskOnStretch = false;
				}

				// Main.NewText(NPC.localAI[0]);
				if (NPC.localAI[0] == 30)
				{
					aimPosHeight = new Vector2(0, -250);
					NPC.noGravity = true;
					originalPos = NPC.position;
					basePos = NPC.position;
					mouth1 = new Vector2[400];
					mouth2 = new Vector2[400];
					oldMouth1 = new Vector2[400];
					oldMouth2 = new Vector2[400];
					mouth2 = new Vector2[400];
					mouth1Vel = new Vector2(1, 0);
					mouth2Vel = new Vector2(-1, 0);
				}
				if (NPC.localAI[0] < 33 && NPC.localAI[0] > 30)// 调整基座高度
				{
					basePos = NPC.position;
				}

				if (NPC.localAI[0] < 90 && NPC.localAI[0] > 30)// 升起来
				{
					float k = (NPC.localAI[0] - 30f) / 60f;

					NPC.position = originalPos + aimPosHeight * (float)Math.Sqrt(k);
				}
				if (NPC.localAI[0] == 90)// 锁定
				{
					originalPosII = NPC.position;
					for (int f = 0; f < 400; f++)
					{
						hangMaxL1[f] = Main.rand.Next(50, 170);
						hangMaxL2[f] = Main.rand.Next(50, 170); // 控制獠牙长短
					}
				}

				if (NPC.localAI[0] < 750 && NPC.localAI[0] > 90)// 伸长，CLAMP
				{
					NPC.knockBackResist = 0.5f;
					NPC.velocity.Y += (float)Math.Sin(Main.time / 70f) * 0.15f;
					NPC.position = NPC.position * 0.8f + originalPosII * 0.2f;
					NPC.velocity *= 0.95f;
					if (NPC.localAI[0] > 655)
					{
						for (int f = 0; f < 400; f++)
						{
							hangMaxL1[f] *= 0.9f;
							hangMaxL2[f] *= 0.9f; // 控制獠牙长短
						}
					}

					double fx0 = (NPC.localAI[0] - 490) / 115f * Math.PI;
					if (NPC.localAI[0] > 720)
					{
						fx0 = Math.PI;
					}

					double fx1 = Math.Sin(fx0) * fx0 / 9d * Math.PI / 2d;
					if (NPC.localAI[0] > 605)
					{
						fx1 = Math.Sin(fx0) * Math.PI / 2d;
					}

					if (NPC.localAI[0] > 674)
					{
						fx1 = Math.Sin(1.6 * Math.PI) * Math.PI / 2d;
					}

					mouth1[0] = NPC.Center + new Vector2(0, 75);
					int maxCountMouth1 = 0; // 最大数量

					for (int f = mouth1.Length - 1; f > 0; f--)// 逐级检测
					{
						if (mouth1[f - 1] != Vector2.Zero)// 空值时,记录长度
						{
							maxCountMouth1 = f;
							break;
						}
					}
					if (maxCountMouth1 < 399)
					{
						for (int f = mouth1.Length - 1; f > 0; f--)// 逐级伸展
						{
							if (mouth1[f - 1] != Vector2.Zero)// 空值时,向前发展一步
							{
								mouth1[f] = mouth1[f - 1] + mouth1Vel;
								break;
							}
						}
						for (int i = 0; i < maxCountMouth1; i++)// 摇摆
						{
							float Shake = (float)((Math.Cos(i / (float)maxCountMouth1 * Math.PI) + 1) * Math.Sin(i / 50f + Main.time * 0.05));
							mouth1[i] += Shake * new Vector2(0, -0.15f);
						}
					}
					else
					{
						if (oldMouth1[0] == Vector2.Zero && NPC.localAI[0] <= 491)
						{
							for (int i = 0; i < maxCountMouth1; i++)
							{
								oldMouth1[i] = mouth1[i];
							}
						}
						for (int i = 0; i < maxCountMouth1; i++)
						{
							mouth1[i] = NPC.Center + new Vector2(0, 75) + (oldMouth1[i] - (NPC.Center + new Vector2(0, 75))).RotatedBy(-fx1);
						}
					}

					if (maxCountMouth1 > 380 && maxCountMouth1 != 399)// 上翘
					{
						mouth1Vel = mouth1Vel.RotatedBy(-0.1);
					}

					if (maxCountMouth1 > 300)
					{
						if (mouth1Vel.Length() < 3)
						{
							mouth1Vel *= 1.02f;
						}
					}
					if (mouth1[4] != Vector2.Zero)// 平衡
					{
						mouth1[2] = mouth1[2] * 0.9f + (mouth1[4] + mouth1[0]) * 0.05f;
					}

					if (mouth1[2] != Vector2.Zero)
					{
						mouth1[1] = mouth1[1] * 0.9f + (mouth1[2] + mouth1[0]) * 0.05f;
					}

					mouth2[0] = NPC.Center + new Vector2(0, 75);
					int maxCountMouth2 = 0; // 最大数量

					for (int f = mouth2.Length - 1; f > 0; f--)// 逐级检测
					{
						if (mouth2[f - 1] != Vector2.Zero)// 空值时,记录长度
						{
							maxCountMouth2 = f;
							break;
						}
					}
					if (maxCountMouth2 < 399)
					{
						for (int f = mouth2.Length - 1; f > 0; f--)// 逐级伸展
						{
							if (mouth2[f - 1] != Vector2.Zero)// 空值时,向前发展一步
							{
								mouth2[f] = mouth2[f - 1] + mouth2Vel;
								break;
							}
						}
						for (int i = 0; i < maxCountMouth2; i++)
						{
							float Shake = (float)((Math.Cos(i / (float)maxCountMouth2 * Math.PI) + 1) * Math.Sin(i / 50f + Main.time * 0.05));
							mouth2[i] += Shake * new Vector2(0, -0.15f); // 摇摆
						}
					}
					else
					{
						if (oldMouth2[0] == Vector2.Zero && NPC.localAI[0] <= 491)
						{
							for (int i = 0; i < maxCountMouth1; i++)
							{
								oldMouth2[i] = mouth2[i];
							}
						}
						for (int i = 0; i < maxCountMouth1; i++)// 旋转
						{
							mouth2[i] = NPC.Center + new Vector2(0, 75) + (oldMouth2[i] - (NPC.Center + new Vector2(0, 75))).RotatedBy(fx1);
						}
					}
					if (maxCountMouth2 > 380 && maxCountMouth1 != 399)// 上翘
					{
						mouth2Vel = mouth2Vel.RotatedBy(0.1);
					}

					if (maxCountMouth2 > 300)
					{
						if (mouth2Vel.Length() < 3)
						{
							mouth2Vel *= 1.02f;
						}
					}
					if (mouth2[4] != Vector2.Zero)// 平衡
					{
						mouth2[2] = mouth2[2] * 0.9f + (mouth2[4] + mouth2[0]) * 0.05f;
					}

					if (mouth2[2] != Vector2.Zero)
					{
						mouth2[1] = mouth2[1] * 0.9f + (mouth2[2] + mouth2[0]) * 0.05f;
					}
				}

				if (NPC.localAI[0] == 750)// 降落
				{
					NPC.knockBackResist = 0;
					NPC.position = originalPosII;
					originalPos = NPC.position;
					aimPosHeight = new Vector2(0, 250);
				}
				if (NPC.localAI[0] < 810 && NPC.localAI[0] > 750)
				{
					float k = (NPC.localAI[0] - 750f) / 60f;
					NPC.position = originalPos + aimPosHeight * k * k;
					NPC.velocity *= 0.5f;
				}
				if (NPC.localAI[0] == 810)
				{
					NPC.velocity *= 0f;
					NPC.noGravity = false;
					originalPos = NPC.position;
					aimPosHeight = new Vector2(0, 250);
					basePos = NPC.position;
				}
			}
			if (NPC.localAI[0] >= 850 && NPC.localAI[0] < 1150)// 收缩,归中
			{
				for (int f = 0; f < NPC.buffType.Length; f++)
				{
					NPC.buffImmune[f] = true;
				}
				if (NPC.localAI[0] % 850 == 60)
				{
					subTuskOnSquzze = true;
				}

				if (NPC.localAI[0] % 850 == 80)
				{
					subTuskOnSquzze = false;
				}

				if (NPC.localAI[0] % 850 >= 80 && NPC.localAI[0] % 850 < 100)
				{
					if (NPC.localAI[0] % 850 == 80)
					{
						nPCDamageBuffer = NPC.damage;
						NPC.damage = 0;
						NPC.dontTakeDamage = true;
						transparent = true;
					}

					if (NPC.alpha >= 255)
					{
						NPC.alpha = 255;
						if (!player.active || player.dead)
						{
							NPC.active = false;
						}
					}
					else
					{
						NPC.alpha += 13;
					}
				}
				if (NPC.localAI[0] % 850 == 105)
				{
					NPC.position += new Vector2(0, -400).RotatedBy(Main.rand.NextFloat(-1.4f, -0.8f) * Math.Sign(NPC.Center.X - player.Center.X));
					if (NPC.position.X > firstPos.X)
					{
						NPC.position.X = firstPos.X;
					}

					if (NPC.position.X < firstPos.X)
					{
						NPC.position.X = firstPos.X;
					}

					NPC.velocity.Y = 10f;
				}
				if (NPC.localAI[0] % 850 == 250)
				{
					subTuskOnStretch = true;
				}

				if (NPC.localAI[0] % 850 == 270)
				{
					subTuskOnStretch = false;
				}

				if (NPC.localAI[0] % 850 >= 200 && NPC.localAI[0] % 850 < 220)
				{
					NPC.dontTakeDamage = false;
					NPC.damage = nPCDamageBuffer;
					transparent = false;
					if (NPC.alpha <= 0)
					{
						NPC.alpha = 0;
						NPC.damage = nPCDamageBuffer;
					}
					else
					{
						NPC.alpha -= 13;
					}
				}
			}
			if (NPC.localAI[0] >= 1150 && NPC.localAI[0] < 1750)
			{
				if (NPC.localAI[0] <= 1300)
				{
					if (NPC.localAI[0] % 30 == 0)
					{
						float Nx = (NPC.localAI[0] - 1225) * 9;
						NPC.NewNPC(null, (int)(NPC.Bottom.X + Nx), (int)(NPC.Bottom.Y - 16), ModContent.NPCType<CrimsonTuskFireWork>());
					}
				}
			}
			if (NPC.localAI[0] >= 1450 && NPC.localAI[0] < 2000)
			{

			}
			if (NPC.localAI[0] >= 2000 && NPC.localAI[0] < 2800)
			{
				if (NPC.localAI[0] % 7 == 0)
				{
					NPC.NewNPC(null, (int)(NPC.Bottom.X + 800 - NPC.localAI[0] % 400 * 4), (int)(NPC.Bottom.Y - 16), ModContent.NPCType<LargeTusk2>());
					NPC.NewNPC(null, (int)(NPC.Bottom.X - 800 + NPC.localAI[0] % 400 * 4), (int)(NPC.Bottom.Y - 16), ModContent.NPCType<LargeTusk2>());
					float dx = (NPC.localAI[0] - 2000) / 400f;
					float sinx = (float)(Math.Sin(dx * Math.PI) * 800f);
					float y = (float)(-Math.Cos(dx * Math.PI) * 40f);
					var v = new Vector2(sinx, y);
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + v + new Vector2(0, -20), Vector2.Zero, ModContent.ProjectileType<Projectiles.EarthTuskHostile>(), NPC.damage / 12, 0, Main.myPlayer, 0);
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - v + new Vector2(0, -20), Vector2.Zero, ModContent.ProjectileType<Projectiles.EarthTuskHostile>(), NPC.damage / 12, 0, Main.myPlayer, 0);
				}
			}
			if (NPC.localAI[0] >= 2600 && NPC.localAI[0] < 3000)
			{
				if (NPC.localAI[0] % 60 == 0)
				{
					NPC.NewNPC(null, (int)(NPC.Center.X + (NPC.localAI[0] - 2780) * 8), (int)(player.Center.Y - 10), ModContent.NPCType<BloodyMouth1>(), 0, 1, 0);
					NPC.NewNPC(null, (int)(NPC.Center.X + (NPC.localAI[0] - 2780) * 8), (int)(player.Center.Y - 10), ModContent.NPCType<BloodyMouth2>(), 0, -1, 0);
				}
			}
			if (NPC.localAI[0] >= 3200 && NPC.localAI[0] < 3800)// 扇面
			{
				int Freq = 60;
				if (Main.expertMode)
				{
					Freq = 50;
				}

				if (Main.masterMode)
				{
					Freq = 40;
				}

				int times = 8;
				if (Main.expertMode)
				{
					times = 10;
				}

				if (Main.masterMode)
				{
					times = 12;
				}

				if (NPC.localAI[0] % Freq == 0)
				{
					for (int f = 0; f < 12; f++)
					{
						var v0 = new Vector2(0, -16f);
						Vector2 v1 = v0.RotatedBy((f - (times / 2 - 0.5)) / (double)(times + 5d));
						Vector2 v2 = v1.RotatedBy((NPC.localAI[0] % 7 - 3.5) * 0.1);
						v2.Y -= 6f;
						Projectile.NewProjectile(null, new Vector2(-3, 80) + NPC.Center, v2, ModContent.ProjectileType<Projectiles.CrimsonTuskProjGra>(), NPC.damage / 9, 1);
					}
				}
			}
			if (NPC.localAI[0] >= 3800 && NPC.localAI[0] < 4400)// 扫射
			{
				int Freq = 10;
				if (Main.expertMode)
				{
					Freq = 7;
				}

				if (Main.masterMode)
				{
					Freq = 4;
				}

				if (NPC.localAI[0] % Freq == 0)
				{
					var v0 = new Vector2(0, -16f);
					Vector2 v2 = v0.RotatedBy((NPC.localAI[0] % 41 - 20) * 0.04);
					v2.Y -= 3f;
					Projectile.NewProjectile(null, new Vector2(-3, 80) + NPC.Center, v2, ModContent.ProjectileType<Projectiles.CrimsonTuskProjGra>(), NPC.damage / 9, 1);
				}
			}
			if (NPC.localAI[0] >= 4400 && NPC.localAI[0] <= 5400)// 层层落下
			{
				int Freq = 180;
				if (Main.expertMode)
				{
					Freq = 150;
				}

				if (Main.masterMode)
				{
					Freq = 99;
				}

				if (NPC.localAI[0] % Freq == 0)
				{
					for (int ec = 0; ec < 9; ec++)
					{
						for (int f = 0; f < 4; f++)
						{
							var v0 = new Vector2((NPC.localAI[0] + 4600 + f * 27 + ec * 222) % 2000 - 1000, -20f);
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + v0, Vector2.Zero, ModContent.ProjectileType<Projectiles.EarthTuskHostile>(), NPC.damage / 12, 0, Main.myPlayer, 0);
						}
					}
				}
				if (NPC.localAI[0] % Freq == Freq / 3)
				{
					for (int ec = 0; ec < 9; ec++)
					{
						for (int f = 0; f < 4; f++)
						{
							var v0 = new Vector2((NPC.localAI[0] + 4600 + f * 27 + ec * 222 + 67) % 2000 - 1000, -20f);
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + v0, Vector2.Zero, ModContent.ProjectileType<Projectiles.EarthTuskHostile>(), NPC.damage / 12, 0, Main.myPlayer, 0);
						}
					}
				}
				if (NPC.localAI[0] % Freq == Freq / 3 * 2)
				{
					for (int ec = 0; ec < 9; ec++)
					{
						for (int f = 0; f < 4; f++)
						{
							var v0 = new Vector2((NPC.localAI[0] + 4600 + f * 27 + ec * 222 + 133) % 2000 - 1000, -20f);
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + v0, Vector2.Zero, ModContent.ProjectileType<Projectiles.EarthTuskHostile>(), NPC.damage / 12, 0, Main.myPlayer, 0);
						}
					}
				}
			}
			if (NPC.localAI[0] >= 5400)
			{
				NPC.localAI[0] = 0;
			}
		}// Phase2
		else
		{
			basePos = NPC.position;

			// 核心AI转化
			if (fightT >= 60)
			{
				if (NPC.localAI[0] > 6000)
				{
					NPC.localAI[0] = 0;
				}
			}
			if (NPC.localAI[0] < 600)
			{
				for (int f = 0; f < NPC.buffType.Length; f++)
				{
					NPC.buffImmune[f] = true;
				}
				if (NPC.localAI[0] % 300 == 60)
				{
					subTuskOnSquzze = true;
				}

				if (NPC.localAI[0] % 300 == 80)
				{
					subTuskOnSquzze = false;
				}

				if (NPC.localAI[0] % 300 >= 80 && NPC.localAI[0] % 300 < 100)
				{
					if (NPC.localAI[0] % 300 == 80)
					{
						nPCDamageBuffer = NPC.damage;
						NPC.damage = 0;
						NPC.dontTakeDamage = true;
						transparent = true;
					}

					if (NPC.alpha >= 255)
					{
						NPC.alpha = 255;
						if (!player.active || player.dead)
						{
							NPC.active = false;
						}
					}
					else
					{
						NPC.alpha += 13;
					}
				}
				if (NPC.localAI[0] % 300 == 105)
				{
					NPC.position += new Vector2(0, -400).RotatedBy(Main.rand.NextFloat(-1.4f, -0.8f) * Math.Sign(NPC.Center.X - player.Center.X));
					if (NPC.position.X > firstPos.X + 600)
					{
						NPC.position.X = firstPos.X + 600;
					}

					if (NPC.position.X < firstPos.X - 600)
					{
						NPC.position.X = firstPos.X - 600;
					}

					NPC.velocity.Y = 10f;
				}
				if (NPC.localAI[0] % 300 == 250)
				{
					subTuskOnStretch = true;
				}

				if (NPC.localAI[0] % 300 == 270)
				{
					subTuskOnStretch = false;
				}

				if (NPC.localAI[0] % 300 >= 200 && NPC.localAI[0] % 300 < 220)
				{
					NPC.dontTakeDamage = false;
					NPC.damage = nPCDamageBuffer;
					transparent = false;
					if (NPC.alpha <= 0)
					{
						NPC.alpha = 0;
						NPC.damage = nPCDamageBuffer;
					}
					else
					{
						NPC.alpha -= 13;
					}
				}
			}
			if (NPC.localAI[0] > 600 && NPC.localAI[0] <= 1200)
			{
				if (NPC.localAI[0] % 200 == 10)
				{
					int X = Main.rand.Next(-50, 50);
					int Dist = 15;
					if (!Main.expertMode && !Main.masterMode)
					{
						Dist = 15;
					}

					if (Main.expertMode && !Main.masterMode)
					{
						Dist = 20;
					}

					if (Main.masterMode)
					{
						Dist = 24;
					}

					for (int i = 1; i < Dist; i++)
					{
						int dX = Main.rand.Next(-5, 5);
						if (!Main.rand.NextBool(3))
						{
							NPC.NewNPC(null, (int)NPC.Center.X + (int)(96 * i * 15f / Dist) + X + dX, (int)player.Center.Y - 20, ModContent.NPCType<LittleTusk>());
						}
						else
						{
							NPC.NewNPC(null, (int)NPC.Center.X + (int)(96 * i * 15f / Dist) + X + dX, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
						}
						dX = Main.rand.Next(-5, 5);

						if (!Main.rand.NextBool(3))
						{
							NPC.NewNPC(null, (int)NPC.Center.X - (int)(96 * i * 15f / Dist) + X + dX, (int)player.Center.Y - 20, ModContent.NPCType<LittleTusk>());
						}
						else
						{
							NPC.NewNPC(null, (int)NPC.Center.X - (int)(96 * i * 15f / Dist) + X + dX, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
						}
					}
				}
			}
			if (NPC.localAI[0] > 1200 && NPC.localAI[0] < 1800)
			{
				int Dist = 10;
				if (!Main.expertMode && !Main.masterMode)
				{
					Dist = 10;
				}

				if (Main.expertMode && !Main.masterMode)
				{
					Dist = 8;
				}

				if (Main.masterMode)
				{
					Dist = 6;
				}

				if (NPC.localAI[0] % (Dist * 2) == Dist)
				{
					int X = Main.rand.Next(-15, 15);
					if (!Main.rand.NextBool(3))
					{
						NPC.NewNPC(null, (int)NPC.Center.X + (int)(NPC.localAI[0] % 200) * 5 + X, (int)player.Center.Y - 20, ModContent.NPCType<LittleTusk>());
					}
					else
					{
						NPC.NewNPC(null, (int)NPC.Center.X + (int)(NPC.localAI[0] % 200) * 5 + X, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
					}
					X = Main.rand.Next(-15, 15);
					if (!Main.rand.NextBool(3))
					{
						NPC.NewNPC(null, (int)NPC.Center.X - (int)(NPC.localAI[0] % 200) * 5 + X, (int)player.Center.Y - 20, ModContent.NPCType<LittleTusk>());
					}
					else
					{
						NPC.NewNPC(null, (int)NPC.Center.X - (int)(NPC.localAI[0] % 200) * 5 + X, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
					}
				}
				if (Main.expertMode || Main.masterMode)
				{
					if (NPC.localAI[0] > 1200 && NPC.localAI[0] < 1400)
					{
						if (NPC.localAI[0] % Dist == 0)
						{
							var v = new Vector2((NPC.localAI[0] - 1300) * 12, -20 + (NPC.localAI[0] - 1300) * 0.04f);
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + v, Vector2.Zero, ModContent.ProjectileType<Projectiles.EarthTuskHostile>(), NPC.damage / 12, 0, Main.myPlayer, 0);
						}
					}
					if (NPC.localAI[0] > 1400 && NPC.localAI[0] < 1600)
					{
						if (NPC.localAI[0] % Dist == 0)
						{
							var v = new Vector2(-(NPC.localAI[0] - 1500) * 12, -20 + (NPC.localAI[0] - 1500) * 0.04f);
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + v, Vector2.Zero, ModContent.ProjectileType<Projectiles.EarthTuskHostile>(), NPC.damage / 12, 0, Main.myPlayer, 0);
						}
					}
					if (NPC.localAI[0] > 1600 && NPC.localAI[0] < 1800)
					{
						if (NPC.localAI[0] % Dist == 0)
						{
							var v = new Vector2((NPC.localAI[0] - 1700) * 12, -20 + (NPC.localAI[0] - 1700) * 0.04f);
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + v, Vector2.Zero, ModContent.ProjectileType<Projectiles.EarthTuskHostile>(), NPC.damage / 12, 0, Main.myPlayer, 0);
						}
					}
				}
			}
			if (NPC.localAI[0] > 1800 && NPC.localAI[0] < 2400)
			{
				if (NPC.localAI[0] < 1830)
				{
					subTuskPosition[8] = subTuskPosition[8] * 0.85f + new Vector2(0, 180) * 0.15f;
				}

				if (NPC.localAI[0] > 1850 && NPC.localAI[0] < 2350)
				{
					for (int i = 0; i < 2; i++)
					{
						Vector2 velocity = new Vector2(0, Main.rand.NextFloat(5f)).RotatedByRandom(MathHelper.TwoPi) + new Vector2(0, -8);
						var blood = new BloodSplash
						{
							velocity = velocity,
							Active = true,
							Visible = true,
							position = NPC.Bottom + new Vector2(Main.rand.NextFloat(-6f, 0), 0).RotatedByRandom(6.283) + new Vector2(0, -10),
							maxTime = Main.rand.Next(42, 78),
							scale = Main.rand.NextFloat(6f, 18f),
							ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0, Main.rand.NextFloat(20.0f, 40.0f) },
						};
						Ins.VFXManager.Add(blood);
						for (int g = 0; g < 2; g++)
						{
							Vector2 afterVelocity = new Vector2(0, Main.rand.NextFloat(5f)).RotatedByRandom(MathHelper.TwoPi) + new Vector2(0, -17);
							float mulScale = Main.rand.NextFloat(6f, 25f);
							var bloodDrop = new BloodDrop
							{
								velocity = afterVelocity,
								Active = true,
								Visible = true,
								position = NPC.Bottom + new Vector2(Main.rand.NextFloat(-6f, 0), 0).RotatedByRandom(6.283) + new Vector2(0, -10),
								maxTime = Main.rand.Next(82, 164),
								scale = mulScale,
								rotation = Main.rand.NextFloat(6.283f),
								ai = new float[] { 0f, Main.rand.NextFloat(0.0f, 4.93f) },
							};
							Ins.VFXManager.Add(bloodDrop);
						}
					}
					int Freq = 18;
					if (Main.expertMode && !Main.masterMode)
					{
						Freq = 12;
					}

					if (Main.masterMode)
					{
						Freq = 10;
					}

					if (NPC.localAI[0] % Freq == 1)
					{
						int r = Projectile.NewProjectile(null, new Vector2(-3, 80) + NPC.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.CrimsonTuskProjGra>(), NPC.damage / 9, 1);
						Main.projectile[r].velocity = new Vector2(0, Main.rand.NextFloat(-26f, -12f)).RotatedBy(Main.rand.NextFloat(-0.5f, 0.5f));
					}
				}
				if (NPC.localAI[0] > 2370)
				{
					subTuskPosition[8] = subTuskPosition[8] * 0.85f;
				}
			}
			if (NPC.localAI[0] > 2400 && NPC.localAI[0] <= 2600)
			{
				int Freq = 15;
				if (Main.expertMode && !Main.masterMode)
				{
					Freq = 10;
				}

				if (Main.masterMode)
				{
					Freq = 5;
				}

				if (NPC.localAI[0] % Freq == 0)
				{
					Vector2 v = new Vector2(0, -28).RotatedBy(Math.Sin(NPC.localAI[0] / 100d * Math.PI));
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + v, Vector2.Zero, ModContent.ProjectileType<Projectiles.EarthTuskHostile>(), NPC.damage / 12, 0, Main.myPlayer, 0);
				}
				if (Main.masterMode)
				{
					if (NPC.localAI[0] % Freq == 2)
					{
						Vector2 v = new Vector2(0, -20).RotatedBy(Math.Sin(NPC.localAI[0] / 100d * Math.PI) + 0.84);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + v, Vector2.Zero, ModContent.ProjectileType<Projectiles.EarthTuskHostile>(), NPC.damage / 12, 0, Main.myPlayer, 0);
					}
					if (NPC.localAI[0] % Freq == 4)
					{
						Vector2 v = new Vector2(0, -20).RotatedBy(Math.Sin(NPC.localAI[0] / 100d * Math.PI) - 0.84);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + v, Vector2.Zero, ModContent.ProjectileType<Projectiles.EarthTuskHostile>(), NPC.damage / 12, 0, Main.myPlayer, 0);
					}
				}
			}
			if (NPC.localAI[0] > 2600 && NPC.localAI[0] <= 2800)
			{
				int Freq = 15;
				if (Main.expertMode && !Main.masterMode)
				{
					Freq = 10;
				}

				if (Main.masterMode)
				{
					Freq = 5;
				}

				if (NPC.localAI[0] % Freq == 0)
				{
					Vector2 v = new Vector2(0, -360).RotatedBy(Math.Sin(NPC.localAI[0] / 100d * Math.PI));
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + v, Vector2.Zero, ModContent.ProjectileType<Projectiles.EarthTuskHostile>(), NPC.damage / 12, 0, Main.myPlayer, 0);
				}
				if (NPC.localAI[0] % Freq == 0)
				{
					var v = new Vector2((NPC.localAI[0] - 2700) * 6, -220 + (NPC.localAI[0] - 2700) * 0.4f);
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + v, Vector2.Zero, ModContent.ProjectileType<Projectiles.EarthTuskHostile>(), NPC.damage / 12, 0, Main.myPlayer, 0);
				}
			}
			if (NPC.localAI[0] > 2800 && NPC.localAI[0] <= 3000)
			{
				int Freq = 15;
				if (Main.expertMode && !Main.masterMode)
				{
					Freq = 10;
				}

				if (Main.masterMode)
				{
					Freq = 5;
				}

				if (NPC.localAI[0] % Freq == 0)
				{
					Vector2 v = new Vector2(0, -500).RotatedBy(Math.Sin(NPC.localAI[0] / 100d * Math.PI));
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + v, Vector2.Zero, ModContent.ProjectileType<Projectiles.EarthTuskHostile>(), NPC.damage / 12, 0, Main.myPlayer, 0);
				}
				if (Main.masterMode)
				{
					if (NPC.localAI[0] % Freq == 2)
					{
						Vector2 v = new Vector2(0, -720).RotatedBy(Math.Sin(NPC.localAI[0] / 100d * Math.PI) + 0.84);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + v, Vector2.Zero, ModContent.ProjectileType<Projectiles.EarthTuskHostile>(), NPC.damage / 12, 0, Main.myPlayer, 0);
					}
					if (NPC.localAI[0] % Freq == 4)
					{
						Vector2 v = new Vector2(0, -720).RotatedBy(Math.Sin(NPC.localAI[0] / 100d * Math.PI) - 0.84);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + v, Vector2.Zero, ModContent.ProjectileType<Projectiles.EarthTuskHostile>(), NPC.damage / 12, 0, Main.myPlayer, 0);
					}
				}
			}
			if (NPC.localAI[0] > 3000 && NPC.localAI[0] <= 3200)
			{
				if (!Main.expertMode && !Main.masterMode)
				{
					if (NPC.localAI[0] % 24 == 0)
					{
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 100), new Vector2(0, -15).RotatedBy(-Math.Sin((NPC.localAI[0] - 3000) / 40f)), ModContent.ProjectileType<Projectiles.TuskSpice>(), NPC.damage / 9, 3f, Main.myPlayer, Math.Abs(((NPC.localAI[0] - 3000) / 4f % 70 - 35) * 18) + 240);
					}

					if (NPC.localAI[0] % 24 == 12)
					{
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 100), new Vector2(0, -15).RotatedBy(Math.Sin((NPC.localAI[0] - 3000) / 40f)), ModContent.ProjectileType<Projectiles.TuskSpice>(), NPC.damage / 9, 3f, Main.myPlayer, -Math.Abs(((NPC.localAI[0] - 3000) / 4f % 70 - 35) * 18) - 240);
					}
				}
				if (Main.expertMode && !Main.masterMode)
				{
					if (NPC.localAI[0] % 12 == 0)
					{
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 100), new Vector2(0, -15).RotatedBy(-Math.Sin((NPC.localAI[0] - 3000) / 40f)), ModContent.ProjectileType<Projectiles.TuskSpice>(), NPC.damage / 9, 3f, Main.myPlayer, Math.Abs(((NPC.localAI[0] - 3000) / 4f % 70 - 35) * 18) + 240);
					}

					if (NPC.localAI[0] % 12 == 6)
					{
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 100), new Vector2(0, -15).RotatedBy(Math.Sin((NPC.localAI[0] - 3000) / 40f)), ModContent.ProjectileType<Projectiles.TuskSpice>(), NPC.damage / 9, 3f, Main.myPlayer, -Math.Abs(((NPC.localAI[0] - 3000) / 4f % 70 - 35) * 18) - 240);
					}
				}
				if (Main.masterMode)
				{
					if (NPC.localAI[0] % 6 == 0)
					{
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 100), new Vector2(0, -15).RotatedBy(-Math.Sin((NPC.localAI[0] - 3000) / 40f)), ModContent.ProjectileType<Projectiles.TuskSpice>(), NPC.damage / 9, 3f, Main.myPlayer, Math.Abs(((NPC.localAI[0] - 3000) / 4f % 70 - 35) * 18) + 240);
					}

					if (NPC.localAI[0] % 6 == 3)
					{
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 100), new Vector2(0, -15).RotatedBy(Math.Sin((NPC.localAI[0] - 3000) / 40f)), ModContent.ProjectileType<Projectiles.TuskSpice>(), NPC.damage / 9, 3f, Main.myPlayer, -Math.Abs(((NPC.localAI[0] - 3000) / 4f % 70 - 35) * 18) - 240);
					}
				}
			}
			if (NPC.localAI[0] > 3200 && NPC.localAI[0] <= 3400)
			{
				if (!Main.expertMode && !Main.masterMode)
				{
					if (NPC.localAI[0] % 18 == 0)
					{
						for (int i = 0; i < 2; i++)
						{
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 100), new Vector2(0, -15).RotatedBy(-Math.Sin((NPC.localAI[0] - 3000) / 40f + i)), ModContent.ProjectileType<Projectiles.TuskSpice>(), NPC.damage / 9, 3f, Main.myPlayer, Math.Abs(((NPC.localAI[0] - 3000) / 4f % 70 - 35) * 18) + 240);
						}
						for (int i = 0; i < 2; i++)
						{
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 100), new Vector2(0, -15).RotatedBy(-Math.Sin((NPC.localAI[0] - 3000) / 40f - i)), ModContent.ProjectileType<Projectiles.TuskSpice>(), NPC.damage / 9, 3f, Main.myPlayer, -Math.Abs(((NPC.localAI[0] - 3000) / 4f % 70 - 35) * 18) - 240);
						}
					}
				}
				if (Main.expertMode && !Main.masterMode)
				{
					if (NPC.localAI[0] % 12 == 0)
					{
						for (int i = 0; i < 2; i++)
						{
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 100), new Vector2(0, -15).RotatedBy(-Math.Sin((NPC.localAI[0] - 3000) / 40f + i)), ModContent.ProjectileType<Projectiles.TuskSpice>(), NPC.damage / 9, 3f, Main.myPlayer, Math.Abs(((NPC.localAI[0] - 3000) / 4f % 70 - 35) * 18) + 240);
						}
						for (int i = 0; i < 2; i++)
						{
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 100), new Vector2(0, -15).RotatedBy(-Math.Sin((NPC.localAI[0] - 3000) / 40f - i)), ModContent.ProjectileType<Projectiles.TuskSpice>(), NPC.damage / 9, 3f, Main.myPlayer, -Math.Abs(((NPC.localAI[0] - 3000) / 4f % 70 - 35) * 18) - 240);
						}
					}
				}
				if (Main.masterMode)
				{
					if (NPC.localAI[0] % 9 == 0)
					{
						for (int i = 0; i < 3; i++)
						{
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 100), new Vector2(0, -15).RotatedBy(-Math.Sin((NPC.localAI[0] - 3000) / 40f + i)), ModContent.ProjectileType<Projectiles.TuskSpice>(), NPC.damage / 9, 3f, Main.myPlayer, Math.Abs(((NPC.localAI[0] - 3000) / 4f % 70 - 35) * 18) + 240);
						}
						for (int i = 0; i < 3; i++)
						{
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 100), new Vector2(0, -15).RotatedBy(-Math.Sin((NPC.localAI[0] - 3000) / 40f - i)), ModContent.ProjectileType<Projectiles.TuskSpice>(), NPC.damage / 9, 3f, Main.myPlayer, -Math.Abs(((NPC.localAI[0] - 3000) / 4f % 70 - 35) * 18) - 240);
						}
					}
				}
			}
			if (NPC.localAI[0] > 3400 && NPC.localAI[0] <= 3600)
			{
				if (!Main.expertMode && !Main.masterMode)
				{
					if (NPC.localAI[0] % 18 == 0)
					{
						for (int i = 0; i < 2; i++)
						{
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 100) + new Vector2(Main.rand.Next(-100, 100), 0), new Vector2(0, -15), ModContent.ProjectileType<Projectiles.TuskSpice>(), NPC.damage / 9, 3f, Main.myPlayer, Math.Abs(((NPC.localAI[0] - 3000) / 4f % 70 - 35) * 18) + 240);
						}
						for (int i = 0; i < 2; i++)
						{
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 100) + new Vector2(Main.rand.Next(-100, 100), 0), new Vector2(0, -15), ModContent.ProjectileType<Projectiles.TuskSpice>(), NPC.damage / 9, 3f, Main.myPlayer, -Math.Abs(((NPC.localAI[0] - 3000) / 4f % 70 - 35) * 18) - 240);
						}
					}
				}
				if (Main.expertMode && !Main.masterMode)
				{
					if (NPC.localAI[0] % 12 == 0)
					{
						for (int i = 0; i < 2; i++)
						{
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 100) + new Vector2(Main.rand.Next(-100, 100), 0), new Vector2(0, -15), ModContent.ProjectileType<Projectiles.TuskSpice>(), NPC.damage / 9, 3f, Main.myPlayer, Math.Abs(((NPC.localAI[0] - 3000) / 4f % 70 - 35) * 18) + 240);
						}
						for (int i = 0; i < 2; i++)
						{
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 100) + new Vector2(Main.rand.Next(-100, 100), 0), new Vector2(0, -15), ModContent.ProjectileType<Projectiles.TuskSpice>(), NPC.damage / 9, 3f, Main.myPlayer, -Math.Abs(((NPC.localAI[0] - 3000) / 4f % 70 - 35) * 18) - 240);
						}
					}
				}
				if (Main.masterMode)
				{
					if (NPC.localAI[0] % 9 == 0)
					{
						for (int i = 0; i < 3; i++)
						{
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 100) + new Vector2(Main.rand.Next(-100, 100), 0), new Vector2(0, -15), ModContent.ProjectileType<Projectiles.TuskSpice>(), NPC.damage / 9, 3f, Main.myPlayer, Math.Abs(((NPC.localAI[0] - 3000) / 4f % 70 - 35) * 18) + 240);
						}
						for (int i = 0; i < 3; i++)
						{
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 100) + new Vector2(Main.rand.Next(-100, 100), 0), new Vector2(0, -15), ModContent.ProjectileType<Projectiles.TuskSpice>(), NPC.damage / 9, 3f, Main.myPlayer, -Math.Abs(((NPC.localAI[0] - 3000) / 4f % 70 - 35) * 18) - 240);
						}
					}
				}
			}
			if (NPC.localAI[0] > 3600 && NPC.localAI[0] <= 4200)
			{
				if (NPC.localAI[0] == 3605)
				{
					spawnNPCPointX = (int)(player.Center.X - 3);
					NPC.NewNPC(null, spawnNPCPointX, (int)(player.Center.Y - 10), ModContent.NPCType<BloodyMouth1>(), 0, 1, 0);
					NPC.NewNPC(null, spawnNPCPointX, (int)(player.Center.Y - 10), ModContent.NPCType<BloodyMouth2>(), 0, -1, 0);
				}
				if (NPC.localAI[0] % 10 == 0 && NPC.localAI[0] >= 3605 && NPC.localAI[0] <= 3720)
				{
					NPC.NewNPC(null, spawnNPCPointX + Main.rand.Next(260, 800), (int)player.Center.Y - 20, ModContent.NPCType<LittleTusk>());
				}

				if (NPC.localAI[0] % 10 == 2 && NPC.localAI[0] >= 3605 && NPC.localAI[0] <= 3720)
				{
					NPC.NewNPC(null, spawnNPCPointX - Main.rand.Next(260, 800), (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
				}

				if (NPC.localAI[0] % 10 == 5 && NPC.localAI[0] >= 3605 && NPC.localAI[0] <= 3720)
				{
					NPC.NewNPC(null, spawnNPCPointX - Main.rand.Next(260, 800), (int)player.Center.Y - 20, ModContent.NPCType<LittleTusk>());
				}

				if (NPC.localAI[0] % 10 == 8 && NPC.localAI[0] >= 3605 && NPC.localAI[0] <= 3720)
				{
					NPC.NewNPC(null, spawnNPCPointX + Main.rand.Next(260, 800), (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
				}
			}
			if (NPC.localAI[0] > 4200 && NPC.localAI[0] <= 4800)
			{
				if (NPC.localAI[0] == 4205)
				{
					Vector2 v0 = NPC.Center - player.Center;
					if (v0.X > 0)
					{
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 10), new Vector2(-20, -36), ModContent.ProjectileType<Projectiles.TuskCurse>(), 0, 3f, player.whoAmI, 0, 0f);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 10), new Vector2(16, -26), ModContent.ProjectileType<Projectiles.TuskCurse>(), 0, 3f, player.whoAmI, 0, 0f);
						if (Main.masterMode || Main.masterMode)
						{
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 10), new Vector2(4, -6), ModContent.ProjectileType<Projectiles.TuskCurse>(), 0, 3f, player.whoAmI, 0, 0f);
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 10), new Vector2(2, -36), ModContent.ProjectileType<Projectiles.TuskCurse>(), 0, 3f, player.whoAmI, 0, 0f);
						}
						if (Main.masterMode)
						{
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 10), new Vector2(6, -37), ModContent.ProjectileType<Projectiles.TuskCurse>(), 0, 3f, player.whoAmI, 0, 0f);
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 10), new Vector2(3, -32), ModContent.ProjectileType<Projectiles.TuskCurse>(), 0, 3f, player.whoAmI, 0, 0f);
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 10), new Vector2(-15, -29), ModContent.ProjectileType<Projectiles.TuskCurse>(), 0, 3f, player.whoAmI, 0, 0f);
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 10), new Vector2(10, -29), ModContent.ProjectileType<Projectiles.TuskCurse>(), 0, 3f, player.whoAmI, 0, 0f);
						}
					}
					else
					{
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 10), new Vector2(-16, -26), ModContent.ProjectileType<Projectiles.TuskCurse>(), 0, 3f, player.whoAmI, 0, 0f);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 10), new Vector2(20, -36), ModContent.ProjectileType<Projectiles.TuskCurse>(), 0, 3f, player.whoAmI, 0, 0f);
						if (Main.masterMode || Main.masterMode)
						{
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 10), new Vector2(-2, -36), ModContent.ProjectileType<Projectiles.TuskCurse>(), 0, 3f, player.whoAmI, 0, 0f);
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 10), new Vector2(-4, -6), ModContent.ProjectileType<Projectiles.TuskCurse>(), 0, 3f, player.whoAmI, 0, 0f);
						}
						if (Main.masterMode)
						{
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 10), new Vector2(-3, -32), ModContent.ProjectileType<Projectiles.TuskCurse>(), 0, 3f, player.whoAmI, 0, 0f);
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 10), new Vector2(-8, -27), ModContent.ProjectileType<Projectiles.TuskCurse>(), 0, 3f, player.whoAmI, 0, 0f);
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 10), new Vector2(-10, -29), ModContent.ProjectileType<Projectiles.TuskCurse>(), 0, 3f, player.whoAmI, 0, 0f);
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 10), new Vector2(15, -29), ModContent.ProjectileType<Projectiles.TuskCurse>(), 0, 3f, player.whoAmI, 0, 0f);
						}
					}
				}
			}
			if (NPC.localAI[0] > 4800 && NPC.localAI[0] <= 5400)
			{
				int MaxTusk = 6;
				if (Main.expertMode && !Main.masterMode)
				{
					MaxTusk = 10;
				}

				if (Main.masterMode)
				{
					MaxTusk = 15;
				}

				if (NPC.localAI[0] == 4801)
				{
					for (int i = 0; i < MaxTusk; i++)
					{
						NPC.NewNPC(null, (int)NPC.Center.X - i * 18 - 900, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
						NPC.NewNPC(null, (int)NPC.Center.X - i * 18 - 300, (int)player.Center.Y - 20, ModContent.NPCType<LittleTusk>());
						NPC.NewNPC(null, (int)NPC.Center.X - i * 18 + 300, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
						NPC.NewNPC(null, (int)NPC.Center.X - i * 18 + 900, (int)player.Center.Y - 20, ModContent.NPCType<LittleTusk>());
					}
				}
				if (NPC.localAI[0] == 4901)
				{
					for (int i = 0; i < MaxTusk; i++)
					{
						var v = new Vector2(-i * 18 - 1200, -20);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + v, Vector2.Zero, ModContent.ProjectileType<Projectiles.EarthTuskHostile>(), NPC.damage / 12, 0, Main.myPlayer, 0);
						v = new Vector2(-i * 18 - 600, -20);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + v, Vector2.Zero, ModContent.ProjectileType<Projectiles.EarthTuskHostile>(), NPC.damage / 12, 0, Main.myPlayer, 0);
						v = new Vector2(-i * 18 + 0, -20);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + v, Vector2.Zero, ModContent.ProjectileType<Projectiles.EarthTuskHostile>(), NPC.damage / 12, 0, Main.myPlayer, 0);
						v = new Vector2(-i * 18 + 600, -20);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + v, Vector2.Zero, ModContent.ProjectileType<Projectiles.EarthTuskHostile>(), NPC.damage / 12, 0, Main.myPlayer, 0);
						v = new Vector2(-i * 18 + 1200, -20);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + v, Vector2.Zero, ModContent.ProjectileType<Projectiles.EarthTuskHostile>(), NPC.damage / 12, 0, Main.myPlayer, 0);
					}
				}
				if (NPC.localAI[0] == 5001)
				{
					for (int i = 0; i < MaxTusk; i++)
					{
						NPC.NewNPC(null, (int)NPC.Center.X - i * 18 - 1200, (int)player.Center.Y - 20, ModContent.NPCType<LittleTusk>());
						NPC.NewNPC(null, (int)NPC.Center.X - i * 18 - 600, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
						NPC.NewNPC(null, (int)NPC.Center.X - i * 18 + 0, (int)player.Center.Y - 20, ModContent.NPCType<LittleTusk>());
						NPC.NewNPC(null, (int)NPC.Center.X - i * 18 + 600, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
						NPC.NewNPC(null, (int)NPC.Center.X - i * 18 + 1200, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
					}
				}
				if (NPC.localAI[0] == 5101)
				{
					for (int i = 0; i < MaxTusk; i++)
					{
						var v = new Vector2(-i * 18 - 900, -20);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + v, Vector2.Zero, ModContent.ProjectileType<Projectiles.EarthTuskHostile>(), NPC.damage / 12, 0, Main.myPlayer, 0);
						v = new Vector2(-i * 18 - 300, -20);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + v, Vector2.Zero, ModContent.ProjectileType<Projectiles.EarthTuskHostile>(), NPC.damage / 12, 0, Main.myPlayer, 0);
						v = new Vector2(-i * 18 + 300, -20);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + v, Vector2.Zero, ModContent.ProjectileType<Projectiles.EarthTuskHostile>(), NPC.damage / 12, 0, Main.myPlayer, 0);
						v = new Vector2(-i * 18 + 900, -20);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + v, Vector2.Zero, ModContent.ProjectileType<Projectiles.EarthTuskHostile>(), NPC.damage / 12, 0, Main.myPlayer, 0);
					}
				}
				if (NPC.localAI[0] == 5201)
				{
					for (int i = 0; i < MaxTusk; i++)
					{
						NPC.NewNPC(null, (int)NPC.Center.X - i * 18 - 900, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
						NPC.NewNPC(null, (int)NPC.Center.X - i * 18 - 300, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
						NPC.NewNPC(null, (int)NPC.Center.X - i * 18 + 300, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
						NPC.NewNPC(null, (int)NPC.Center.X - i * 18 + 900, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
					}
				}
				if (NPC.localAI[0] == 5301)
				{
					for (int i = 0; i < MaxTusk; i++)
					{
						var v = new Vector2(-i * 18 - 1200, -20);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + v, Vector2.Zero, ModContent.ProjectileType<Projectiles.EarthTuskHostile>(), NPC.damage / 12, 0, Main.myPlayer, 0);
						v = new Vector2(-i * 18 - 600, -20);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + v, Vector2.Zero, ModContent.ProjectileType<Projectiles.EarthTuskHostile>(), NPC.damage / 12, 0, Main.myPlayer, 0);
						v = new Vector2(-i * 18 + 0, -20);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + v, Vector2.Zero, ModContent.ProjectileType<Projectiles.EarthTuskHostile>(), NPC.damage / 12, 0, Main.myPlayer, 0);
						v = new Vector2(-i * 18 + 600, -20);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + v, Vector2.Zero, ModContent.ProjectileType<Projectiles.EarthTuskHostile>(), NPC.damage / 12, 0, Main.myPlayer, 0);
						v = new Vector2(-i * 18 + 1200, -20);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + v, Vector2.Zero, ModContent.ProjectileType<Projectiles.EarthTuskHostile>(), NPC.damage / 12, 0, Main.myPlayer, 0);
					}
				}
			}
			if (NPC.localAI[0] > 5400 && NPC.localAI[0] <= 6000)
			{
				int Freq = 12;
				if (Main.expertMode && !Main.masterMode)
				{
					Freq = 9;
				}

				if (Main.masterMode)
				{
					Freq = 6;
				}

				if (NPC.localAI[0] % Freq == 0)
				{
					NPC.NewNPC(null, (int)(NPC.Center.X - 1300 + (NPC.localAI[0] - 5400) % 200 * 13f), (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk2>());
					NPC.NewNPC(null, (int)(NPC.Center.X + 1300 - (NPC.localAI[0] - 5400) % 200 * 13f), (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk2>());
				}
			}
		}
		if (NPC.life < NPC.lifeMax * 0.999f)
		{
			maxFlyingTentaclesCount = 2;
			if (Main.expertMode)
			{
				maxFlyingTentaclesCount = 3;
			}

			if (Main.masterMode)
			{
				maxFlyingTentaclesCount = 4;
			}

			LookingCenter = NPC.Center + new Vector2(-3, 90);
			NPC.localAI[1] += 1;
			if (NoTentacle)
			{
				if (NPC.localAI[0] > 600)
				{
					for (int i = 0; i < maxFlyingTentaclesCount; i++)
					{
						FlyingTentacleTusks[i] = NPC.NewNPCDirect(null, (int)(NPC.Center.X + 12), (int)(NPC.Center.Y + 90), ModContent.NPCType<CrimsonTuskControlable>(), 0, -1, i, (i - 1.5f) * 0.7f);
						if (FlyingTentacleTusks[i] != null)
						{
							CrimsonTuskControlable ctct = FlyingTentacleTusks[i].ModNPC as CrimsonTuskControlable;
							if (ctct != null)
							{
								ctct.Owner = NPC;
							}
						}
					}
					NoTentacle = false;
				}
			}
			if (NPC.life >= NPC.lifeMax * 0.5)
			{
				if (NPC.localAI[0] > 600)
				{
					for (int i = 0; i < maxFlyingTentaclesCount; i++)
					{
						if (NPC.localAI[1] % 200 > i * 50 && NPC.localAI[1] % 200 < (i + 1) * 50)
						{
							Vector2 v = player.Center - FlyingTentacleTusks[i].Center;
							if (i % 2 == 0)
							{
								v = new Vector2(-v.X, v.Y);
							}

							v = v / v.Length() * 7f;
							FlyingTentacleTusks[i].velocity = FlyingTentacleTusks[i].velocity * 0.95f + v * 0.05f;
							int k = i + 1;
							if (k >= 4)
							{
								k = 0;
							}

							if (FlyingTentacleTusks[k] != null && FlyingTentacleTusks[k].active && FlyingTentacleTusks[k].type == ModContent.NPCType<CrimsonTuskControlable>())
							{
								v = player.Center - FlyingTentacleTusks[k].Center;
								if (i % 2 == 0)
								{
									v = new Vector2(-v.X, v.Y);
								}

								v = v / v.Length() * 7f;
								FlyingTentacleTusks[k].velocity = FlyingTentacleTusks[k].velocity * 0.975f + v * 0.025f;
							}
						}
						if (FlyingTentacleTusks.Length > i || FlyingTentacleTusks[i] != null || !FlyingTentacleTusks[i].active)
						{
							if (FlyingTentacleTusks[i].Center.Y > NPC.Center.Y - 240)
							{
								FlyingTentacleTusks[i].velocity.Y -= 0.25f;
							}
						}

						back[i] = false;
					}
					if (NPC.localAI[1] > 600)
					{
						NPC.localAI[1] = 0;
					}
				}
				else
				{
					if (!NoTentacle)
					{
						for (int i = 0; i < maxFlyingTentaclesCount; i++)
						{
							if (FlyingTentacleTusks[i] != null && FlyingTentacleTusks[i].active && FlyingTentacleTusks[i].type == ModContent.NPCType<CrimsonTuskControlable>())
							{
								Vector2 v = LookingCenter - FlyingTentacleTusks[i].Center;
								if (v.Length() > 10 && !back[i])
								{
									v = v / v.Length() * 10f;
									FlyingTentacleTusks[i].velocity = FlyingTentacleTusks[i].velocity * 0.95f + v * 0.05f;
								}
								else
								{
									if (!back[i])
									{
										back[i] = true;
									}

									FlyingTentacleTusks[i].position = LookingCenter - new Vector2(15, 20);
									FlyingTentacleTusks[i].velocity = FlyingTentacleTusks[i].velocity / FlyingTentacleTusks[i].velocity.Length() * 0.5f;
								}
							}
						}
					}
				}
			}
			else
			{
				if (NPC.localAI[0] <= 850 || NPC.localAI[0] >= 1150)
				{
					for (int i = 0; i < maxFlyingTentaclesCount; i++)
					{
						if (FlyingTentacleTusks[i] != null && FlyingTentacleTusks[i].active && FlyingTentacleTusks[i].type == ModContent.NPCType<CrimsonTuskControlable>())
						{
							if (NPC.localAI[1] % 200 > i * 50 && NPC.localAI[1] % 200 < (i + 1) * 50)
							{
								Vector2 v = player.Center - FlyingTentacleTusks[i].Center;
								if (i % 2 == 0)
								{
									v = new Vector2(-v.X, v.Y);
								}

								v = v / v.Length() * 7f;
								FlyingTentacleTusks[i].velocity = FlyingTentacleTusks[i].velocity * 0.95f + v * 0.05f;
								int k = i + 1;
								if (k >= 4)
								{
									k = 0;
								}

								if (FlyingTentacleTusks[k] != null && FlyingTentacleTusks[k].active && FlyingTentacleTusks[k].type == ModContent.NPCType<CrimsonTuskControlable>())
								{
									v = player.Center - FlyingTentacleTusks[k].Center;
									if (i % 2 == 0)
									{
										v = new Vector2(-v.X, v.Y);
									}

									v = v / v.Length() * 7f;
									FlyingTentacleTusks[k].velocity = FlyingTentacleTusks[k].velocity * 0.975f + v * 0.025f;
								}
							}
							if (FlyingTentacleTusks.Length > i || FlyingTentacleTusks[i] != null || !FlyingTentacleTusks[i].active)
							{
								if (FlyingTentacleTusks[i].Center.Y > NPC.Center.Y - 240)
								{
									FlyingTentacleTusks[i].velocity.Y -= 0.25f;
								}
							}
							back[i] = false;
						}
					}
					if (NPC.localAI[1] > 600)
					{
						NPC.localAI[1] = 0;
					}
				}
				else
				{
					for (int i = 0; i < maxFlyingTentaclesCount; i++)
					{
						if (FlyingTentacleTusks[i] != null && FlyingTentacleTusks[i].active && FlyingTentacleTusks[i].type == ModContent.NPCType<CrimsonTuskControlable>())
						{
							Vector2 v = LookingCenter - FlyingTentacleTusks[i].Center;
							if (v.Length() > 10 && !back[i])
							{
								v = v / v.Length() * 10f;
								FlyingTentacleTusks[i].velocity = FlyingTentacleTusks[i].velocity * 0.95f + v * 0.05f;
							}
							else
							{
								if (!back[i])
								{
									back[i] = true;
								}

								FlyingTentacleTusks[i].position = LookingCenter - new Vector2(15, 20);
								FlyingTentacleTusks[i].velocity = FlyingTentacleTusks[i].velocity / FlyingTentacleTusks[i].velocity.Length() * 0.5f;
							}
						}
					}
				}
			}
		}
		if (!player.active || player.dead)
		{
			for (int i = 0; i < maxFlyingTentaclesCount; i++)
			{
				FlyingTentacleTusks[i].ai[3] = 30;
			}
			NPC.alpha += 5;
			if (NPC.alpha > 250)
			{
				NPC.active = false;
			}
		}
		if (hasbeenKilled)
		{
		}
		if (reallyStart)
		{
			NPC.localAI[2]++;
			if (NPC.localAI[2] < 600)
			{
				// 测试部分
				if (NPC.localAI[2] == 30)
				{
					int Xz = Main.rand.Next(-5, 5);
					NPC.NewNPC(null, (int)NPC.Center.X + 500 + Xz, (int)player.Center.Y - 20, ModContent.NPCType<LittleTusk>());
					Xz = Main.rand.Next(-5, 5);
					NPC.NewNPC(null, (int)NPC.Center.X - 500 + Xz, (int)player.Center.Y - 20, ModContent.NPCType<LittleTusk>());
				}
				if (NPC.localAI[2] == 35)
				{
					int Xz = Main.rand.Next(-5, 5);
					NPC.NewNPC(null, (int)NPC.Center.X + 460 + Xz, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
					Xz = Main.rand.Next(-5, 5);
					NPC.NewNPC(null, (int)NPC.Center.X - 460 + Xz, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
				}
				if (NPC.localAI[2] == 50)
				{
					int Xz = Main.rand.Next(-5, 5);
					NPC.NewNPC(null, (int)NPC.Center.X + 430 + Xz, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
					Xz = Main.rand.Next(-5, 5);
					NPC.NewNPC(null, (int)NPC.Center.X - 430 + Xz, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
				}
				if (NPC.localAI[2] == 60)
				{
					int Xz = Main.rand.Next(-5, 5);
					NPC.NewNPC(null, (int)NPC.Center.X + 380 + Xz, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
					Xz = Main.rand.Next(-5, 5);
					NPC.NewNPC(null, (int)NPC.Center.X - 380 + Xz, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
				}
				if (NPC.localAI[2] == 75)
				{
					int Xz = Main.rand.Next(-5, 5);
					NPC.NewNPC(null, (int)NPC.Center.X + 400 + Xz, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
					Xz = Main.rand.Next(-5, 5);
					NPC.NewNPC(null, (int)NPC.Center.X - 400 + Xz, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
				}

				if (NPC.localAI[2] == 140)
				{
					int Xz = Main.rand.Next(-5, 5);
					NPC.NewNPC(null, (int)NPC.Center.X + 380 + Xz, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
					Xz = Main.rand.Next(-5, 5);
					NPC.NewNPC(null, (int)NPC.Center.X - 380 + Xz, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
				}
				if (NPC.localAI[2] == 155)
				{
					int Xz = Main.rand.Next(-5, 5);
					NPC.NewNPC(null, (int)NPC.Center.X + 430 + Xz, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
					Xz = Main.rand.Next(-5, 5);
					NPC.NewNPC(null, (int)NPC.Center.X - 430 + Xz, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
				}
				if (NPC.localAI[2] == 163)
				{
					int Xz = Main.rand.Next(-5, 5);
					NPC.NewNPC(null, (int)NPC.Center.X + 470 + Xz, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
					Xz = Main.rand.Next(-5, 5);
					NPC.NewNPC(null, (int)NPC.Center.X - 470 + Xz, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
				}
				if (NPC.localAI[2] == 180)
				{
					int Xz = Main.rand.Next(-5, 5);
					NPC.NewNPC(null, (int)NPC.Center.X + 330 + Xz, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
					Xz = Main.rand.Next(-5, 5);
					NPC.NewNPC(null, (int)NPC.Center.X - 330 + Xz, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
				}
				if (NPC.localAI[2] == 200)
				{
					int Xz = Main.rand.Next(-5, 5);
					NPC.NewNPC(null, (int)NPC.Center.X + 500 + Xz, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
					Xz = Main.rand.Next(-5, 5);
					NPC.NewNPC(null, (int)NPC.Center.X - 500 + Xz, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
				}

				if (NPC.localAI[2] == 235)
				{
					int Xz = Main.rand.Next(-5, 5);
					NPC.NewNPC(null, (int)NPC.Center.X + 530 + Xz, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
					Xz = Main.rand.Next(-5, 5);
					NPC.NewNPC(null, (int)NPC.Center.X - 530 + Xz, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
				}
				if (NPC.localAI[2] == 245)
				{
					int Xz = Main.rand.Next(-5, 5);
					NPC.NewNPC(null, (int)NPC.Center.X + 570 + Xz, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
					Xz = Main.rand.Next(-5, 5);
					NPC.NewNPC(null, (int)NPC.Center.X - 570 + Xz, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
				}
				if (NPC.localAI[2] == 250)
				{
					int Xz = Main.rand.Next(-1, 1);
					NPC.NewNPC(null, (int)NPC.Center.X + 540 + Xz, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
					Xz = Main.rand.Next(-1, 1);
					NPC.NewNPC(null, (int)NPC.Center.X - 540 + Xz, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
				}
				if (NPC.localAI[2] == 266)
				{
					int Xz = Main.rand.Next(-1, 1);
					NPC.NewNPC(null, (int)NPC.Center.X + 520 + Xz, (int)player.Center.Y - 20, ModContent.NPCType<LittleTusk>());
					Xz = Main.rand.Next(-1, 1);
					NPC.NewNPC(null, (int)NPC.Center.X - 520 + Xz, (int)player.Center.Y - 20, ModContent.NPCType<LittleTusk>());
				}
				if (NPC.localAI[2] == 270)
				{
					int Xz = Main.rand.Next(-1, 1);
					NPC.NewNPC(null, (int)NPC.Center.X + 500 + Xz, (int)player.Center.Y - 20, ModContent.NPCType<LittleTusk>());
					Xz = Main.rand.Next(-1, 1);
					NPC.NewNPC(null, (int)NPC.Center.X - 500 + Xz, (int)player.Center.Y - 20, ModContent.NPCType<LittleTusk>());
				}
				if (NPC.localAI[2] == 274)
				{
					int Xz = Main.rand.Next(-1, 1);
					NPC.NewNPC(null, (int)NPC.Center.X + 480 + Xz, (int)player.Center.Y - 20, ModContent.NPCType<LittleTusk>());
					Xz = Main.rand.Next(-1, 1);
					NPC.NewNPC(null, (int)NPC.Center.X - 480 + Xz, (int)player.Center.Y - 20, ModContent.NPCType<LittleTusk>());
				}
				if (NPC.localAI[2] == 278)
				{
					int Xz = Main.rand.Next(-1, 1);
					NPC.NewNPC(null, (int)NPC.Center.X + 460 + Xz, (int)player.Center.Y - 20, ModContent.NPCType<LittleTusk>());
					Xz = Main.rand.Next(-1, 1);
					NPC.NewNPC(null, (int)NPC.Center.X - 460 + Xz, (int)player.Center.Y - 20, ModContent.NPCType<LittleTusk>());
				}
				if (NPC.localAI[2] == 282)
				{
					int Xz = Main.rand.Next(-1, 1);
					NPC.NewNPC(null, (int)NPC.Center.X + 440 + Xz, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
					Xz = Main.rand.Next(-1, 1);
					NPC.NewNPC(null, (int)NPC.Center.X - 440 + Xz, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
				}
			}
		}
	}

	public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
	{
		return base.DrawHealthBar(hbPosition, ref scale, ref position);
	}

	public override bool PreAI()
	{
		NPC.TargetClosest(false);

		if (hasbeenKilled)
		{
			NPC.velocity *= 0;
			Killing--;
			if (Killing == 150)
			{
				for (int h = 0; h < 15; h++)
				{
					// Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 50), Vector2.Zero, ModContent.ProjectileType<Projectiles.TuskKillEffect>(), 0, 0, Main.myPlayer, 0);
				}
			}
			if (Killing <= 0)
			{
				DownedBossSystem.downedTusk = true;
				SoundEngine.PlaySound(SoundID.DD2_SkeletonDeath, NPC.Center);
				for (int u = 0; u < 45; u++)
				{
					float vX = Main.rand.NextFloat(-2f, 2f);
					float vY = (float)Math.Cos(vX / 4d * Math.PI) * 12f;
					int r = Dust.NewDust(new Vector2(NPC.Center.X, NPC.Center.Y + 22f), 0, 0, DustID.Crimstone, vX, -vY, 0, default, 1.1f);
					Main.dust[r].noGravity = false;
				}
				for (int u = 0; u < 25; u++)
				{
					float vX = Main.rand.NextFloat(-2f, 2f);
					float vY = (float)Math.Cos(vX / 8d * Math.PI) * 4f;
					int r = Dust.NewDust(new Vector2(NPC.Center.X, NPC.Center.Y + 22f), 0, 0, ModContent.DustType<Dusts.TuskBreak>(), vX, -vY, 0, default, Main.rand.NextFloat(0.8f, 1.6f));
					Main.dust[r].noGravity = false;
				}
				for (int u = 0; u < 150; u++)
				{
					float vX = Main.rand.Next(-2000, 2000) / 6000f;
					float vY = (float)Math.Cos(vX * Math.PI) * 2f;
					int r = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y + 110f), NPC.width, 30, DustID.Crimstone, vX, -vY, 0, default, 3f);
					Main.dust[r].noGravity = false;
				}
				NPC.position.X = NPC.position.X + NPC.width / 2;
				NPC.position.Y = NPC.position.Y + NPC.height / 2;
				NPC.position.X = NPC.position.X - NPC.width / 2;
				NPC.position.Y = NPC.position.Y - NPC.height / 2;

				// var vF = new Vector2(vFX, -(float)Math.Cos(vFX * Math.PI) * 6f);
				// Gore.NewGore(null, NPC.position, vF, ModContent.Find<ModGore>("Everglow/Myth/TheTusk/Gores/BloodTuskBroken1").Type, 1f);
				// vFX = Main.rand.Next(-2000, 2000) / 5000f;
				// vF = new Vector2(vFX, -(float)Math.Cos(vFX * Math.PI) * 6f);
				// Gore.NewGore(null, NPC.position, vF, ModContent.Find<ModGore>("Everglow/Myth/TheTusk/Gores/BloodTuskBroken2").Type, 1f);
				// vFX = Main.rand.Next(-2000, 2000) / 5000f;
				// vF = new Vector2(vFX, -(float)Math.Cos(vFX * Math.PI) * 6f);
				// Gore.NewGore(null, NPC.position, vF, ModContent.Find<ModGore>("Everglow/Myth/TheTusk/Gores/BloodTuskBroken3").Type, 1f);
				// vFX = Main.rand.Next(-2000, 2000) / 5000f;
				// vF = new Vector2(vFX, -(float)Math.Cos(vFX * Math.PI) * 12f);
				// Gore.NewGore(null, NPC.position, vF, ModContent.Find<ModGore>("Everglow/Myth/TheTusk/Gores/BloodTuskBroken4").Type, 1f);
				// vFX = Main.rand.Next(-2000, 2000) / 5000f;
				// vF = new Vector2(vFX, -(float)Math.Cos(vFX * Math.PI) * 12f);
				// Gore.NewGore(null, NPC.position, vF, ModContent.Find<ModGore>("Everglow/Myth/TheTusk/Gores/BloodTuskBroken5").Type, 1f);
				// vFX = Main.rand.Next(-2000, 2000) / 5000f;
				// vF = new Vector2(vFX, -(float)Math.Cos(vFX * Math.PI) * 12f);
				// Gore.NewGore(null, NPC.position, vF, ModContent.Find<ModGore>("Everglow/Myth/TheTusk/Gores/BloodTuskBroken6").Type, 1f);
				subTuskOnSquzze = false;
				subTuskOnStretch = false;
				transparent = false;
				firstIcon = 9999999;
				nPCDamageBuffer = 0;
				NPC.StrikeNPC(new NPC.HitInfo()
				{
					Damage = 1,
					Knockback = 0,
					HitDirection = 1,
				});
				NPC.active = false;
			}
			return false;
		}
		return true;
	}

	public override void ModifyNPCLoot(NPCLoot npcLoot)
	{
		npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<TuskMirror>()));
		npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BloodyTuskTrophy>(), 10, 1));

		npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<TuskTreasureBag>()));
		npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<TuskRelic>()));

		var rule = new LeadingConditionRule(new Conditions.NotExpert());
		rule.OnSuccess(ItemDropRule.OneFromOptions(1, ModContent.ItemType<ToothStaff>(), ModContent.ItemType<ToothBow>(), ModContent.ItemType<ToothSpear>(), ModContent.ItemType<TuskLace>(), ModContent.ItemType<ToothMagicBall>(), ModContent.ItemType<BloodyBoneYoyo>(), ModContent.ItemType<SpineGun>(), ModContent.ItemType<ToothKnife>()));

		npcLoot.Add(rule);
		base.ModifyNPCLoot(npcLoot);
	}

	public override void HitEffect(NPC.HitInfo hit)
	{
		if (!reallyStart)
		{
			reallyStart = true;
			if (!Main.dedServ)
			{
				Music = Common.MythContent.QuickMusic("TuskFighting");
			}

			Main.NewText("The Tusk has awoken!", 175, 75, 255);
			NPC.NewNPC(null, (int)NPC.Center.X - 800, (int)NPC.Center.Y - 200, ModContent.NPCType<TuskWallLeft>());
			NPC.NewNPC(null, (int)NPC.Center.X + 800, (int)NPC.Center.Y - 200, ModContent.NPCType<TuskWallRight>());
		}
		NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<TuskCooling>());
		if (NPC.life <= 0)
		{
			if (!hasbeenKilled)
			{
				NPC.life = 1;
				NPC.dontTakeDamage = true;
				hasbeenKilled = true;
				NPC.active = true;
				NPC.velocity *= 0;
				Killing = 180;
				for (int i = 0; i < FlyingTentacleTusks.Length; i++)
				{
					if (FlyingTentacleTusks[i] != null && FlyingTentacleTusks[i].ai.Length > 3)
					{
						FlyingTentacleTusks[i].ai[3] = 30;
					}
				}
			}
		}
	}

	public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
	{
		target.AddBuff(BuffID.Bleeding, 120);
	}

	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		if (!startFight)
		{
			return false;
		}

		Color color = Lighting.GetColor((int)(NPC.Center.X / 16d), (int)(NPC.Center.Y / 16d));
		color = NPC.GetAlpha(color) * ((255 - NPC.alpha) / 255f);
		color.R = (byte)Math.Clamp(color.R + tuskHitMove.Length() * tuskHitMove.Length() * 10f, 0f, 255f);
		color.G = (byte)Math.Clamp(color.G - tuskHitMove.Length() * tuskHitMove.Length() * 5f, 0f, 255f);
		color.B = (byte)Math.Clamp(color.B - tuskHitMove.Length() * tuskHitMove.Length() * 5f, 0f, 255f);
		color.A = (byte)Math.Clamp(color.A - tuskHitMove.Length() * tuskHitMove.Length() * 3f, 0f, 255f);
		Texture2D TuskBaseP1 = ModAsset.BloodTuskTeethPhase1.Value;
		Texture2D TuskS2P1 = ModAsset.BloodTuskS2P1.Value;
		Texture2D TuskS3P1 = ModAsset.BloodTuskS3bP1.Value;
		Texture2D TuskS4P1 = ModAsset.BloodTuskS4bP1.Value;
		Texture2D TuskS5P1 = ModAsset.BloodTuskS5P1.Value;
		Texture2D TuskS6P1 = ModAsset.BloodTuskS6P1.Value;
		Texture2D TuskS7P1 = ModAsset.BloodTuskS7P1.Value;
		Texture2D TuskS8P1 = ModAsset.BloodTuskS8P1.Value;

		Texture2D TuskBase = ModAsset.BloodTuskTeeth.Value;
		Texture2D TuskBaseBlack = ModAsset.BloodTuskTeethBlack.Value;
		Texture2D TuskBaseE1 = ModAsset.BloodTuskEye1.Value;
		Texture2D TuskBaseE2 = ModAsset.BloodTuskEye2.Value;
		Texture2D TuskBaseE3 = ModAsset.BloodTuskEye3.Value;
		Texture2D TuskBaseE4 = ModAsset.BloodTuskEye4.Value;
		Texture2D TuskBaseE5 = ModAsset.BloodTuskEye5.Value;
		Texture2D TuskBaseE6 = ModAsset.BloodTuskEye6.Value;
		Texture2D TuskBaseE7 = ModAsset.BloodTuskEye7.Value;
		Texture2D TuskS1 = ModAsset.BloodTuskS1.Value;
		Texture2D TuskS2 = ModAsset.BloodTuskS2.Value;
		Texture2D TuskS3 = ModAsset.BloodTuskS3b.Value;
		Texture2D TuskS4 = ModAsset.BloodTuskS4b.Value;
		Texture2D TuskS5 = ModAsset.BloodTuskS5.Value;
		Texture2D TuskS6 = ModAsset.BloodTuskS6.Value;
		Texture2D TuskS7 = ModAsset.BloodTuskS7.Value;
		Texture2D TuskS8 = ModAsset.BloodTuskS8.Value;

		if (!Main.gamePaused)
		{
			tuskHitMove *= sprK;
		}

		if (NPC.life < NPC.lifeMax / 2f)
		{
			if (!Main.gamePaused)
			{
				for (int i = 0; i < 7; i++)
				{
					if (Main.rand.NextBool(400) && eye[i] == 0 && eyeMove[i] == Vector2.Zero)
					{
						eye[i] = 60;
					}

					if (eye[i] > 0)
					{
						eye[i]--;
					}

					int Eyevalue = 30 - Math.Abs(eye[i] - 30);
					if (Main.rand.NextBool(400) && eyeMove[i] == Vector2.Zero && eye[i] == 0)
					{
						eyeMotivate[i] = 180;
						eyeRot[i] = Main.rand.NextFloat(0, 6.283f);
					}
					float Ran = (float)Math.Sqrt(Math.Clamp(90 - Math.Abs(eyeMotivate[i] - 90), 0f, 25f) / 25f);
					float Rot = 90 - Math.Abs(eyeMotivate[i] - 90) / 30f;
					eyeMove[i] = new Vector2(0, Ran * 1.5f).RotatedBy(Rot * Rot + eyeRot[i]);
					if (eyeMotivate[i] > 0)
					{
						eyeMotivate[i]--;
						if (eyeMotivate[i] == 0)
						{
							eyeMove[i] = Vector2.Zero;
						}
					}
					if (i == 0)
					{
						if (Eyevalue > 6 && Eyevalue <= 12)
						{
							TuskBaseE1 = ModAsset.BloodTuskEye1C.Value;
						}

						if (Eyevalue > 12 && Eyevalue <= 18)
						{
							TuskBaseE1 = ModAsset.BloodTuskEye1L.Value;
						}

						if (Eyevalue > 18 && Eyevalue <= 24)
						{
							TuskBaseE1 = ModAsset.BloodTuskEye1O.Value;
						}

						if (Eyevalue > 24)
						{
							TuskBaseE1 = ModAsset.BloodTusk_Head_Boss_Void.Value;
						}
					}
					if (i == 1)
					{
						if (Eyevalue > 16 && Eyevalue <= 24)
						{
							TuskBaseE2 = ModAsset.BloodTuskEye2C.Value;
						}

						if (Eyevalue > 24)
						{
							TuskBaseE2 = ModAsset.BloodTusk_Head_Boss_Void.Value;
						}
					}
					if (i == 2)
					{
						if (Eyevalue > 16 && Eyevalue <= 24)
						{
							TuskBaseE3 = ModAsset.BloodTuskEye3C.Value;
						}

						if (Eyevalue > 24)
						{
							TuskBaseE3 = ModAsset.BloodTusk_Head_Boss_Void.Value;
						}
					}
					if (i == 3)
					{
						if (Eyevalue > 18)
						{
							TuskBaseE4 = ModAsset.BloodTusk_Head_Boss_Void.Value;
						}
					}
					if (i == 4)
					{
						if (Eyevalue > 18)
						{
							TuskBaseE5 = ModAsset.BloodTusk_Head_Boss_Void.Value;
						}
					}
					if (i == 5)
					{
						if (Eyevalue > 18)
						{
							TuskBaseE6 = ModAsset.BloodTusk_Head_Boss_Void.Value;
						}
					}
					if (i == 6)
					{
						if (Eyevalue > 18)
						{
							TuskBaseE7 = ModAsset.BloodTusk_Head_Boss_Void.Value;
						}
					}
				}
			}

			// 绘制二阶段獠牙
			if (NPC.localAI[0] < 60)
			{
				for (int nm = 0; nm < 10; nm++)
				{
					subTuskPosition[nm] = subTuskPosition[nm] * 0.85f;
				}
				if (NPC.alpha > 0)
				{
					NPC.alpha -= 13;
				}
				else
				{
					NPC.alpha = 0;
				}
			}
			if (NPC.localAI[0] < 750 && NPC.localAI[0] > 90)
			{
				// 向下开口
				var bars1 = new List<Vertex2D>();
				var bars2 = new List<Vertex2D>();

				for (int i = 1; i < 400; ++i)// 右侧
				{
					if (mouth1[i] != Vector2.Zero)
					{
						float width = 9;
						if (i > 380)
						{
							width = 9 + 6 * (float)(Math.Sin((i - 380) / 20d * Math.PI) + 1);
						}

						if (NPC.localAI[0] >= 700)
						{
							width *= Math.Clamp((750 - NPC.localAI[0]) / 50f, 0f, 1f);
						}

						var normalDir = mouth1[i - 1] - mouth1[i];
						normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
						Color colori = Lighting.GetColor((int)(mouth1[i].X / 16d), (int)(mouth1[i].Y / 16d));
						var factor = Math.Abs(i / 30f % 2 - 1);

						var w2 = MathHelper.Lerp(1f, 0.0f, 0);
						bars1.Add(new Vertex2D(mouth1[i] + normalDir * width + new Vector2(0, -10) - Main.screenPosition, colori, new Vector3(factor, 1, w2)));
						bars1.Add(new Vertex2D(mouth1[i] + normalDir * -width + new Vector2(0, -10) - Main.screenPosition, colori, new Vector3(factor, 0, w2)));

						if (i % 24 == 3 && i < 350)
						{
							float Len = (float)(NPC.localAI[0] - 373 - Math.Sqrt(i));
							var Tusk1 = new List<Vertex2D>
							{
								new Vertex2D(mouth1[i] + normalDir.RotatedBy(1.57) * width + new Vector2(0, -10) - Main.screenPosition, colori, new Vector3(0, 0, 0)),
								new Vertex2D(mouth1[i] + normalDir.RotatedBy(1.57) * -width + new Vector2(0, -10) - Main.screenPosition, colori, new Vector3(1, 0, 0)),
								new Vertex2D(mouth1[i] + new Vector2(0, -10) - normalDir * Math.Clamp(Len, 0, hangMaxL1[i]) - Main.screenPosition, colori, new Vector3(0.5f, 1, 0)),
							};
							Texture2D t1 = ModAsset.CrimsonTuskHang.Value;
							Main.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
							Main.graphics.GraphicsDevice.Textures[0] = t1; // GlodenBloodScaleMirror
							Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Tusk1.ToArray(), 0, Tusk1.Count / 3);
						}
						if (i % 8 == 3 && i >= 350)
						{
							float Len = (float)(NPC.localAI[0] - 373 - Math.Sqrt(i));
							var Tusk1 = new List<Vertex2D>
							{
								new Vertex2D(mouth1[i] + normalDir.RotatedBy(1.57) * width + new Vector2(0, -10) - Main.screenPosition, colori, new Vector3(0, 0, 0)),
								new Vertex2D(mouth1[i] + normalDir.RotatedBy(1.57) * -width + new Vector2(0, -10) - Main.screenPosition, colori, new Vector3(1, 0, 0)),
								new Vertex2D(mouth1[i] + new Vector2(0, -10) - normalDir * Math.Clamp(Len, 0, hangMaxL1[i]) - Main.screenPosition, colori, new Vector3(0.5f, 1, 0)),
							};
							Texture2D t1 = ModAsset.CrimsonTuskHang.Value;
							Main.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
							Main.graphics.GraphicsDevice.Textures[0] = t1; // GlodenBloodScaleMirror
							Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Tusk1.ToArray(), 0, Tusk1.Count / 3);
						}
					}
					if (i % 20 == 0)
					{
						foreach (Player player in Main.player)
						{
							if (player != null && player.active && !player.immune)
							{
								if ((player.Center - (mouth1[i] + new Vector2(0, -10))).Length() < 100 && player.Top.Y > mouth1[i].Y - 10)
								{
									player.Hurt(PlayerDeathReason.ByNPC(NPC.whoAmI), NPC.damage, -1, false, false, false, 30);
									player.immune = true;
									player.immuneTime = 30;
									if (player.longInvince)
									{
										player.immuneTime = 45;
									}
								}
							}
						}
					}
				}

				for (int i = 399; i > 0; --i)// 左侧
				{
					if (mouth2[i] != Vector2.Zero)
					{
						float width = 9;
						if (i > 380)
						{
							width = 9 + 6 * (float)(Math.Sin((i - 380) / 20d * Math.PI) + 1);
						}

						if (NPC.localAI[0] >= 700)
						{
							width *= Math.Clamp((750 - NPC.localAI[0]) / 50f, 0f, 1f);
						}

						var normalDir = mouth2[i - 1] - mouth2[i];
						normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
						Color colori = Lighting.GetColor((int)(mouth2[i].X / 16d), (int)(mouth2[i].Y / 16d));
						var factor = Math.Abs(i / 30f % 2 - 1);

						var w2 = MathHelper.Lerp(1f, 0.0f, 0);
						bars2.Add(new Vertex2D(mouth2[i] + normalDir * width + new Vector2(0, -10) - Main.screenPosition, colori, new Vector3(factor, 1, w2)));
						bars2.Add(new Vertex2D(mouth2[i] + normalDir * -width + new Vector2(0, -10) - Main.screenPosition, colori, new Vector3(factor, 0, w2)));
						if (i % 24 == 3 && i < 350)
						{
							float Len = (float)(NPC.localAI[0] - 373 - Math.Sqrt(i));
							var Tusk2 = new List<Vertex2D>
							{
								new Vertex2D(mouth2[i] + normalDir.RotatedBy(1.57) * width + new Vector2(0, -10) - Main.screenPosition, colori, new Vector3(0, 0, 0)),
								new Vertex2D(mouth2[i] + normalDir.RotatedBy(1.57) * -width + new Vector2(0, -10) - Main.screenPosition, colori, new Vector3(1, 0, 0)),
								new Vertex2D(mouth2[i] + new Vector2(0, -10) + normalDir * Math.Clamp(Len, 0, hangMaxL2[i]) - Main.screenPosition, colori, new Vector3(0.5f, 1, 0)),
							};
							Texture2D t2 = ModAsset.CrimsonTuskHang.Value;
							Main.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
							Main.graphics.GraphicsDevice.Textures[0] = t2; // GlodenBloodScaleMirror
							Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Tusk2.ToArray(), 0, Tusk2.Count / 3);
						}
						if (i % 8 == 3 && i >= 350)
						{
							float Len = (float)(NPC.localAI[0] - 373 - Math.Sqrt(i));
							var Tusk2 = new List<Vertex2D>
							{
								new Vertex2D(mouth2[i] + normalDir.RotatedBy(1.57) * width + new Vector2(0, -10) - Main.screenPosition, colori, new Vector3(0, 0, 0)),
								new Vertex2D(mouth2[i] + normalDir.RotatedBy(1.57) * -width + new Vector2(0, -10) - Main.screenPosition, colori, new Vector3(1, 0, 0)),
								new Vertex2D(mouth2[i] + new Vector2(0, -10) + normalDir * Math.Clamp(Len, 0, hangMaxL2[i]) - Main.screenPosition, colori, new Vector3(0.5f, 1, 0)),
							};
							Texture2D t2 = ModAsset.CrimsonTuskHang.Value;
							Main.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
							Main.graphics.GraphicsDevice.Textures[0] = t2; // GlodenBloodScaleMirror
							Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Tusk2.ToArray(), 0, Tusk2.Count / 3);
						}
					}
					if (i % 20 == 0)
					{
						foreach (Player player in Main.player)
						{
							if (player != null && player.active && !player.immune)
							{
								if ((player.Center - (mouth2[i] + new Vector2(0, -10))).Length() < 100 && player.Top.Y > mouth1[i].Y - 10)
								{
									player.Hurt(PlayerDeathReason.ByNPC(NPC.whoAmI), NPC.damage, 1, false, false, false, 30);
									player.immune = true;
									player.immuneTime = 30;
									if (player.longInvince)
									{
										player.immuneTime = 45;
									}
								}
							}
						}
					}
				}
				var triangleList1 = new List<Vertex2D>();
				var triangleList2 = new List<Vertex2D>();
				if (bars1.Count > 2)
				{
					triangleList1.Add(bars1[^2]);
					var vertex = new Vertex2D((bars1[^2].position + bars1[^1].position) * 0.5f + mouth1Vel * 15f, Color.White, new Vector3(0, 0.5f, 0));
					triangleList1.Add(bars1[^1]);
					triangleList1.Add(vertex);
					for (int i = bars1.Count - 4; i > -2; i -= 2)
					{
						triangleList1.Add(bars1[i]);
						triangleList1.Add(bars1[i + 2]);
						triangleList1.Add(bars1[i + 1]);

						triangleList1.Add(bars1[i + 1]);
						triangleList1.Add(bars1[i + 2]);
						triangleList1.Add(bars1[i + 3]);
					}
					Texture2D t1 = ModAsset.BloodRope.Value;
					Main.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
					Main.graphics.GraphicsDevice.Textures[0] = t1; // GlodenBloodScaleMirror
					Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList1.ToArray(), 0, triangleList1.Count / 3);
				}

				if (bars2.Count > 2)
				{
					triangleList2.Add(bars2[0]);
					var vertex = new Vertex2D((bars2[0].position + bars2[1].position) * 0.5f + mouth2Vel * 15f, Color.White, new Vector3(0, 0.5f, 0));
					triangleList2.Add(bars2[1]);
					triangleList2.Add(vertex);
					for (int i = 0; i < bars2.Count - 2; i += 2)
					{
						triangleList2.Add(bars2[i]);
						triangleList2.Add(bars2[i + 2]);
						triangleList2.Add(bars2[i + 1]);

						triangleList2.Add(bars2[i + 1]);
						triangleList2.Add(bars2[i + 2]);
						triangleList2.Add(bars2[i + 3]);
					}
					Texture2D t1 = ModAsset.BloodRope.Value;
					Main.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
					Main.graphics.GraphicsDevice.Textures[0] = t1; // GlodenBloodScaleMirror
					Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList2.ToArray(), 0, triangleList2.Count / 3);
				}
			}

			Main.spriteBatch.Draw(TuskS3, NPC.position - Main.screenPosition + new Vector2(15, 94) + subTuskPosition[2], new Rectangle?(NPC.frame), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS4, NPC.position - Main.screenPosition + new Vector2(15, 90) + subTuskPosition[3], new Rectangle?(NPC.frame), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(ModAsset.BloodTuskFleshBack.Value, NPC.position - Main.screenPosition + new Vector2(15, 90) + subTuskPosition[9], new Rectangle(0, 0, 220, (int)(312 - subTuskPosition[9].Y * 2f)), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);

			Color Blc = NPC.GetAlpha(Color.Black) * ((255 - NPC.alpha) / 255f);
			Main.spriteBatch.Draw(TuskBaseBlack, NPC.position - Main.screenPosition + new Vector2(15, 90) + subTuskPosition[8] + tuskHitMove, new Rectangle(0, 0, 220, (int)(312 - subTuskPosition[8].Y * 1.5f)), Blc, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);

			Color Alc = NPC.GetAlpha(new Color(255, 255, 255, 150)) * ((255 - NPC.alpha) / 255f);
			Main.spriteBatch.Draw(TuskBaseE1, NPC.position - Main.screenPosition + new Vector2(15, 90) + subTuskPosition[8] + eyeMove[0] + tuskHitMove * 0.9f, new Rectangle(0, 0, 220, (int)(312 - subTuskPosition[8].Y * 1.5f)), Alc, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskBaseE2, NPC.position - Main.screenPosition + new Vector2(15, 90) + subTuskPosition[8] + eyeMove[1] + tuskHitMove * 0.9f, new Rectangle(0, 0, 220, (int)(312 - subTuskPosition[8].Y * 1.5f)), Alc, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskBaseE3, NPC.position - Main.screenPosition + new Vector2(15, 90) + subTuskPosition[8] + eyeMove[2] + tuskHitMove * 0.9f, new Rectangle(0, 0, 220, (int)(312 - subTuskPosition[8].Y * 1.5f)), Alc, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskBaseE4, NPC.position - Main.screenPosition + new Vector2(15, 90) + subTuskPosition[8] + eyeMove[3] + tuskHitMove * 0.9f, new Rectangle(0, 0, 220, (int)(312 - subTuskPosition[8].Y * 1.5f)), Alc, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskBaseE5, NPC.position - Main.screenPosition + new Vector2(15, 90) + subTuskPosition[8] + eyeMove[4] + tuskHitMove * 0.9f, new Rectangle(0, 0, 220, (int)(312 - subTuskPosition[8].Y * 1.5f)), Alc, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskBaseE6, NPC.position - Main.screenPosition + new Vector2(15, 90) + subTuskPosition[8] + eyeMove[5] + tuskHitMove * 0.9f, new Rectangle(0, 0, 220, (int)(312 - subTuskPosition[8].Y * 1.5f)), Alc, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskBaseE7, NPC.position - Main.screenPosition + new Vector2(15, 90) + subTuskPosition[8] + eyeMove[6] + tuskHitMove * 0.9f, new Rectangle(0, 0, 220, (int)(312 - subTuskPosition[8].Y * 1.5f)), Alc, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);

			Main.spriteBatch.Draw(TuskBase, NPC.position - Main.screenPosition + new Vector2(15, 90) + subTuskPosition[8] + tuskHitMove, new Rectangle(0, 0, 220, (int)(312 - subTuskPosition[8].Y * 1.5f)), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS1, NPC.position - Main.screenPosition + new Vector2(15, 90) + subTuskPosition[0], new Rectangle?(NPC.frame), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS2, NPC.position - Main.screenPosition + new Vector2(15, 90) + subTuskPosition[1], new Rectangle?(NPC.frame), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS5, NPC.position - Main.screenPosition + new Vector2(15, 90) + subTuskPosition[4], new Rectangle?(NPC.frame), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS6, NPC.position - Main.screenPosition + new Vector2(15, 90) + subTuskPosition[5], new Rectangle?(NPC.frame), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS7, NPC.position - Main.screenPosition + new Vector2(15, 90) + subTuskPosition[6], new Rectangle?(NPC.frame), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS8, NPC.position - Main.screenPosition + new Vector2(15, 90) + subTuskPosition[7], new Rectangle?(NPC.frame), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
		}
		else
		{
			// 绘制一阶段獠牙
			Main.spriteBatch.Draw(TuskS3P1, NPC.position - Main.screenPosition + new Vector2(15, 94) + subTuskPosition[2], new Rectangle?(NPC.frame), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS4P1, NPC.position - Main.screenPosition + new Vector2(15, 90) + subTuskPosition[3], new Rectangle?(NPC.frame), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(ModAsset.BloodTuskFleshBack.Value, NPC.position - Main.screenPosition + new Vector2(15, 90) + subTuskPosition[9], new Rectangle(0, 0, 220, (int)(312 - subTuskPosition[9].Y * 2f)), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskBaseP1, NPC.position - Main.screenPosition + new Vector2(15, 90) + subTuskPosition[8] + tuskHitMove, new Rectangle(0, 0, 220, (int)(312 - subTuskPosition[8].Y * 1.5f)), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(ModAsset.BloodTuskS1P1.Value, NPC.position - Main.screenPosition + new Vector2(15, 90) + subTuskPosition[0], new Rectangle?(NPC.frame), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS2P1, NPC.position - Main.screenPosition + new Vector2(15, 90) + subTuskPosition[1], new Rectangle?(NPC.frame), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS5P1, NPC.position - Main.screenPosition + new Vector2(15, 90) + subTuskPosition[4], new Rectangle?(NPC.frame), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS6P1, NPC.position - Main.screenPosition + new Vector2(15, 90) + subTuskPosition[5], new Rectangle?(NPC.frame), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS7P1, NPC.position - Main.screenPosition + new Vector2(15, 90) + subTuskPosition[6], new Rectangle?(NPC.frame), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS8P1, NPC.position - Main.screenPosition + new Vector2(15, 90) + subTuskPosition[7], new Rectangle?(NPC.frame), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
		}
		Effect ef = ModAsset.TuskFade.Value;
		if (NPC.life < NPC.lifeMax / 2f && hasTranSkin > 0)
		{
			NPC.localAI[0] = 0;
			if (hasTranSkin > 2)
			{
				NPC.ai[2] = 254;
				NPC.dontTakeDamage = true;
			}
			else
			{
				NPC.ai[2] = 0;
				NPC.dontTakeDamage = false;
			}

			// 阶段切换shader
			if (!Main.gamePaused)
			{
				hasTranSkin--;
			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			ef.Parameters["minr"].SetValue(1 - hasTranSkin / 180f);
			ef.Parameters["uImage1"].SetValue(Commons.ModAsset.Noise_perlin.Value);

			ef.CurrentTechnique.Passes["Test"].Apply();
			Color cg;
			ef.Parameters["BackCol"].SetValue(new Vector4(color.R, color.G, color.B, 255 - NPC.alpha) / 255f);
			float alp = (255 - NPC.alpha) / 255f;
			cg = new Color(color.R / 255f * alp, color.G / 255f * alp, color.B / 255f * alp, alp);
			Main.spriteBatch.Draw(TuskS3P1, NPC.position - Main.screenPosition + new Vector2(15, 94) + subTuskPosition[2], new Rectangle?(NPC.frame), cg, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS4P1, NPC.position - Main.screenPosition + new Vector2(15, 90) + subTuskPosition[3], new Rectangle?(NPC.frame), cg, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(ModAsset.BloodTuskFleshBack.Value, NPC.position - Main.screenPosition + new Vector2(15, 90) + subTuskPosition[9], new Rectangle(0, 0, 220, (int)(312 - subTuskPosition[9].Y * 2f)), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskBaseP1, NPC.position - Main.screenPosition + new Vector2(15, 90) + subTuskPosition[8] + tuskHitMove, new Rectangle(0, 0, 220, (int)(312 - subTuskPosition[8].Y * 1.5f)), cg, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(ModAsset.BloodTuskS1P1.Value, NPC.position - Main.screenPosition + new Vector2(15, 90) + subTuskPosition[0], new Rectangle?(NPC.frame), cg, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS2P1, NPC.position - Main.screenPosition + new Vector2(15, 90) + subTuskPosition[1], new Rectangle?(NPC.frame), cg, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS5P1, NPC.position - Main.screenPosition + new Vector2(15, 90) + subTuskPosition[4], new Rectangle?(NPC.frame), cg, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS6P1, NPC.position - Main.screenPosition + new Vector2(15, 90) + subTuskPosition[5], new Rectangle?(NPC.frame), cg, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS7P1, NPC.position - Main.screenPosition + new Vector2(15, 90) + subTuskPosition[6], new Rectangle?(NPC.frame), cg, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS8P1, NPC.position - Main.screenPosition + new Vector2(15, 90) + subTuskPosition[7], new Rectangle?(NPC.frame), cg, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		}
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		if (basePos != Vector2.Zero)
		{
			// 拉丝底座
			Main.spriteBatch.Draw(ModAsset.BloodTuskFlesh1.Value, basePos - Main.screenPosition + new Vector2(15, 90) + subTuskPosition[9], new Rectangle(0, 0, 220, (int)(312 - subTuskPosition[9].Y * 2f)), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);

			if (basePos != NPC.position)
			{
				for (int z = 0; z < 4; z++)
				{
					hangItem[z] = new Vector2(0, 25 * (float)Math.Sin(Main.time / 70d + z) * (basePos.Y - NPC.position.Y) / 50f);
					var Vx2 = new List<Vertex2D>
					{
						new Vertex2D(NPC.position + new Vector2(-95, 162) - Main.screenPosition, color, new Vector3(0, 0, 0)),
						new Vertex2D(NPC.position + new Vector2(125, 162) - Main.screenPosition, color, new Vector3(1, 0, 0)),
						new Vertex2D(basePos + new Vector2(125, 172) + hangItem[z] - Main.screenPosition, color, new Vector3(1, 1, 0)),

						new Vertex2D(NPC.position + new Vector2(-95, 162) - Main.screenPosition, color, new Vector3(0, 0, 0)),
						new Vertex2D(basePos + new Vector2(125, 172) + hangItem[z] - Main.screenPosition, color, new Vector3(1, 1, 0)),
						new Vertex2D(basePos + new Vector2(-95, 172) + hangItem[z] - Main.screenPosition, color, new Vector3(0, 1, 0)),
					};
					Texture2D thang = ModContent.Request<Texture2D>("Everglow/" + ModAsset.BloodTuskDragLine_Path + (3 + z).ToString()).Value;
					Main.graphics.GraphicsDevice.Textures[0] = thang;
					Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx2.ToArray(), 0, Vx2.Count / 3);
				}

				var Vx = new List<Vertex2D>
				{
					new Vertex2D(NPC.position + new Vector2(-95, 162) - Main.screenPosition, color, new Vector3(0, 0, 0)),
					new Vertex2D(NPC.position + new Vector2(125, 162) - Main.screenPosition, color, new Vector3(1, 0, 0)),
					new Vertex2D(basePos + new Vector2(125, 172) - Main.screenPosition, color, new Vector3(1, 1, 0)),

					new Vertex2D(NPC.position + new Vector2(-95, 162) - Main.screenPosition, color, new Vector3(0, 0, 0)),
					new Vertex2D(basePos + new Vector2(125, 172) - Main.screenPosition, color, new Vector3(1, 1, 0)),
					new Vertex2D(basePos + new Vector2(-95, 172) - Main.screenPosition, color, new Vector3(0, 1, 0)),
				};

				Texture2D t = ModAsset.BloodTuskDragLine1.Value;
				Main.graphics.GraphicsDevice.Textures[0] = t;
				Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);

				t = ModAsset.BloodTuskDragLine2.Value;
				Main.graphics.GraphicsDevice.Textures[0] = t;
				Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
			}
		}
		else
		{
			Main.spriteBatch.Draw(ModAsset.BloodTuskFlesh1.Value, NPC.position - Main.screenPosition + new Vector2(15, 90) + subTuskPosition[9], new Rectangle(0, 0, 220, (int)(312 - subTuskPosition[9].Y * 2f)), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
		}
		Main.spriteBatch.Draw(ModAsset.BloodTuskFlesh.Value, NPC.position - Main.screenPosition + new Vector2(15, 90) + subTuskPosition[9], new Rectangle(0, 0, 220, (int)(312 - subTuskPosition[9].Y * 2f)), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		return false;
	}

	public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
	{
		if (tuskHitMove.Length() > 1)
		{
			sprK *= 0.9f;
		}

		sprK = 0.09f + sprK * 0.9f;
		if (NPC.Center - player.Center != Vector2.Zero)
		{
			tuskHitMove += Vector2.Normalize(NPC.Center - player.Center) * Math.Clamp(item.knockBack, 0, 20f);
		}
	}

	public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
	{
		if (tuskHitMove.Length() > 1)
		{
			sprK *= 0.9f;
		}

		sprK = 0.09f + sprK * 0.9f;
		if (projectile.velocity != Vector2.Zero)
		{
			tuskHitMove += Vector2.Normalize(projectile.velocity) * Math.Clamp(projectile.knockBack, 0, 20f);
		}
	}
}