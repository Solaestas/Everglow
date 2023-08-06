using Everglow.Myth.Bosses.Acytaea.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.Localization;

namespace Everglow.Myth.TheTusk.NPCs.Bosses.BloodTusk;

[AutoloadBossHead]
public class BloodTusk : ModNPC
{
	public override void SetStaticDefaults()
	{
		var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
		{
			CustomTexturePath = "Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTusk",
			Position = new Vector2(40f, 24f),
			PortraitPositionXOverride = 0f,
			PortraitPositionYOverride = 12f
		};
		NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
	}
	//private bool lockBlood = false;
	public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
	{
		string tex = "It was just a wolf tooth, dropped to the Crimson when its owner was defeated by a hero, gradually corrupted by the power of Cthulhu and granted mentality.";
		if (Language.ActiveCulture.Name == "zh-Hans")
			tex = "原本只是一颗狼牙,在它的主人被勇士讨伐时掉落至猩红之地,逐渐为克苏鲁的力量所沾染,有了自己的意识";
		// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
		bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
		{
			// Sets the spawning conditions of this NPC that is listed in the bestiary.
			BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
			BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson,

			// Sets the description of this NPC that is listed in the bestiary.
                
			new FlavorTextBestiaryInfoElement(tex)
		});
	}
	private Vector2[] V = new Vector2[10];
	private Vector2[] VMax = new Vector2[10];
	private int[] I = new int[10];
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
		Music = Common.MythContent.QuickMusic("TuskTension");
	}
	private bool OSquzze = false;
	private bool DSquzze = false;
	private bool Transparent = false;
	private int fir = 9999999;
	private int Dam = 0;
	public static int[] N = new int[4];
	public static Vector2 Cen;
	public static int secondStageHeadSlot = -1;
	private float Hm = 0;
	public override void OnKill()
	{
		NPC.SetEventFlagCleared(ref DownedBossSystem.downedTusk, -1);
		if (Main.netMode == NetmodeID.Server)
			NetMessage.SendData(MessageID.WorldData);
	}
	public override void Load()
	{
		//We want to give it a second boss head icon, so we register one
		string texture = BossHeadTexture + "_Void"; //Our texture is called "ClassName_Head_Boss_SecondStage"
		secondStageHeadSlot = Mod.AddBossHeadTexture(texture, -1); //-1 because we already have one registered via the [AutoloadBossHead] attribute, it would overwrite it otherwise
	}

	private int StartTime = 0;
	public override void BossHeadSlot(ref int index)
	{
		if (fir == 9999999)
			fir = index;
		int slot = secondStageHeadSlot;
		if (Transparent && slot != -1)
			//If the boss is in its second stage, display the other head icon instead
			index = slot;
		if (!Transparent)
			//If the boss is in its second stage, display the other head icon instead
			index = fir;
	}

	private int PhaseChange = 200;
	private Vector2 AimPosHeight = new Vector2(0, -250);
	private Vector2 OriginalPos = new Vector2(0, 0);
	private Vector2 OriginalPosII = new Vector2(0, 0);
	private Vector2 BasePos = Vector2.Zero;
	private Vector2 FirstPos = Vector2.Zero;


	//int coolClick = 0;//计时器
	//int n = 1;//输入数字
	//int[,] Output = new int[10, 10];//二维数组
	//int varit = 0;//辅助变量
	/*public string ReverseA(string text)
        {
            char[] cArray = text.ToCharArray();
            string reverse = String.Empty;
            for (int i = cArray.Length - 1; i > -1; i--)
            {
                reverse += cArray[i];
            }
            return reverse;
        }*/
	private int pulltime = 0;//拉动围墙向中夹的倒计时
	public override void AI()//这是一个自行刷新的函数
	{
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
					break;
			}
			if (DarkCenter == Vector2.Zero)
				DarkCenter = NPC.Bottom;
		}
		/*联机情况下错误排查*/
		if (NPC.Bottom.Y > DarkCenter.Y && Collision.SolidCollision(NPC.Bottom + new Vector2(0, -10), 1, 1))
			NPC.position.Y -= 5f;
		if (FirstPos == Vector2.Zero)
			FirstPos = NPC.position;
		if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
			NPC.TargetClosest();

		Player player = Main.player[NPC.target];
		StartTime++;
		VMax[0] = new Vector2(14, 18);
		VMax[1] = new Vector2(-8, 24);
		VMax[2] = new Vector2(0, 38);
		VMax[3] = new Vector2(6, 30);
		VMax[4] = new Vector2(-30, 30);
		VMax[5] = new Vector2(-10, 34);
		VMax[6] = new Vector2(12, 48);
		VMax[7] = new Vector2(16, 32);
		VMax[8] = new Vector2(0, 132);
		VMax[9] = new Vector2(0, 30);
		for (int n = 0; n < 10; n++)
		{
			if (fightT < 60)
				V[n] = V[n] * fightT / 60f + VMax[n] * (60 - fightT) / 60f;
			else
			{
				if (NPC.life >= NPC.lifeMax * 0.5)
				{
					if (NPC.localAI[0] >= 600)
					{
						if (I[n] == 0)
						{
							if (V[n].Length() > 0.5f && n < 8)
								V[n] = V[n] * 0.95f;
							else
							{
								if (Main.rand.NextBool(600) && n < 8)
									I[n] = 1;
							}
						}
						if (I[n] == 1)
						{
							if ((V[n] - VMax[n]).Length() > 0.5f && n < 8)
								V[n] = V[n] * 0.95f + VMax[n] * 0.05f;
							else
							{
								if (Main.rand.NextBool(240) && n < 8)
									I[n] = 0;
							}
						}
					}
					else
					{
						if (OSquzze)
							V[n] = V[n] * 0.85f + VMax[n] * 0.15f;
						if (DSquzze)
							V[n] = V[n] * 0.85f;
					}
				}
				else
				{
					if (NPC.localAI[0] >= 1150 || NPC.localAI[0] <= 850)
					{
						if (I[n] == 0)
						{
							if (V[n].Length() > 0.5f && n < 8)
								V[n] = V[n] * 0.95f;
							else
							{
								if (Main.rand.NextBool(600) && n < 8)
									I[n] = 1;
							}
						}
						if (I[n] == 1)
						{
							if ((V[n] - VMax[n]).Length() > 0.5f && n < 8)
								V[n] = V[n] * 0.95f + VMax[n] * 0.05f;
							else
							{
								if (Main.rand.NextBool(240) && n < 8)
									I[n] = 0;
							}
						}
					}
					else
					{
						if (OSquzze)
							V[n] = V[n] * 0.85f + VMax[n] * 0.15f;
						if (DSquzze)
							V[n] = V[n] * 0.85f;
					}
				}
			}
		}
		if (NPC.velocity.Length() <= 0.5f && StartTime > 4 && !startFight)
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
			/*if (!lockBlood)
                {
                    if (Misc.Projectiles.Weapon.Fragrans.Fragrans.DefTusk)
                    {
                        lockBlood = true;
                    }
                }*/
			startFight = true;
			NPC.dontTakeDamage = false;
		}
		for (int i = -3; i < 4; i++)
		{
			Tile tilev = Main.tile[(int)(NPC.Bottom.X / 16f) + i, (int)(NPC.Bottom.Y / 16f)];
			if (tilev.TileType == 19)
				NPC.position.Y += 16;
		}
		if (startFight)
		{
			if (ReallyStart)
			{
				NPC.damage = 80;
				if (Main.expertMode)
					NPC.damage = 120;
				if (Main.masterMode)
					NPC.damage = 160;
				if (fightT < 60)
				{
					fightT += 1;
					NPC.localAI[0] = 215;
				}
				else
				{
					fightT = 60;
					if (NPC.life >= NPC.lifeMax * 0.5f)
						NPC.localAI[0] += 1;
				}
			}
		}
		if (NPC.life < NPC.lifeMax * 0.5f)
		{
			if (NPC.localAI[0] < 89)
				NPC.localAI[0] += 1;
			if (NPC.localAI[0] >= 89)
			{
				if (pulltime <= 0)
					NPC.localAI[0] += 1;
				else
				{
					pulltime--;
				}
			}
			//核心AI转化
			if (NPC.localAI[0] < 850)
			{
				if (fightT >= 60)
				{
					if (NPC.localAI[0] > 6000)
						NPC.localAI[0] = 0;
				}
				if (PhaseChange > 0)
				{
					NPC.localAI[0] = 0;
					DSquzze = true;
					PhaseChange--;
				}
				else
				{
					DSquzze = false;
				}
				//Main.NewText(NPC.localAI[0]);
				if (NPC.localAI[0] == 30)
				{
					AimPosHeight = new Vector2(0, -250);
					NPC.noGravity = true;
					OriginalPos = NPC.position;
					BasePos = NPC.position;
					Mouth1 = new Vector2[400];
					Mouth2 = new Vector2[400];
					OldMouth1 = new Vector2[400];
					OldMouth2 = new Vector2[400];
					Mouth2 = new Vector2[400];
					Mouth1Vel = new Vector2(1, 0);
					Mouth2Vel = new Vector2(-1, 0);
				}
				if (NPC.localAI[0] < 33 && NPC.localAI[0] > 30)//调整基座高度
					BasePos = NPC.position;
				if (NPC.localAI[0] < 90 && NPC.localAI[0] > 30)//升起来
				{
					float k = (NPC.localAI[0] - 30f) / 60f;

					NPC.position = OriginalPos + AimPosHeight * (float)Math.Sqrt(k);
				}
				if (NPC.localAI[0] == 90)//锁定
				{
					OriginalPosII = NPC.position;
					for (int f = 0; f < 400; f++)
					{
						HangMaxL1[f] = Main.rand.Next(50, 170);
						HangMaxL2[f] = Main.rand.Next(50, 170);//控制獠牙长短
					}
				}

				if (NPC.localAI[0] < 750 && NPC.localAI[0] > 90)//伸长，CLAMP
				{
					NPC.knockBackResist = 0.5f;
					NPC.velocity.Y += (float)Math.Sin(Main.time / 70f) * 0.15f;
					NPC.position = NPC.position * 0.8f + OriginalPosII * 0.2f;
					NPC.velocity *= 0.95f;
					if (NPC.localAI[0] > 655)
					{
						for (int f = 0; f < 400; f++)
						{
							HangMaxL1[f] *= 0.9f;
							HangMaxL2[f] *= 0.9f;//控制獠牙长短
						}
					}

					double fx0 = (NPC.localAI[0] - 490) / 115f * Math.PI;
					if (NPC.localAI[0] > 720)
						fx0 = Math.PI;
					double fx1 = Math.Sin(fx0) * fx0 / 9d * Math.PI / 2d;
					if (NPC.localAI[0] > 605)
						fx1 = Math.Sin(fx0) * Math.PI / 2d;
					if (NPC.localAI[0] > 674)
						fx1 = Math.Sin(1.6 * Math.PI) * Math.PI / 2d;

					Mouth1[0] = NPC.Center + new Vector2(0, 75);
					int maxCountMouth1 = 0;//最大数量

					for (int f = Mouth1.Length - 1; f > 0; f--)//逐级检测
					{
						if (Mouth1[f - 1] != Vector2.Zero)//空值时,记录长度
						{
							maxCountMouth1 = f;
							break;
						}
					}
					if (maxCountMouth1 < 399)
					{
						for (int f = Mouth1.Length - 1; f > 0; f--)//逐级伸展
						{
							if (Mouth1[f - 1] != Vector2.Zero)//空值时,向前发展一步
							{
								Mouth1[f] = Mouth1[f - 1] + Mouth1Vel;
								break;
							}
						}
						for (int i = 0; i < maxCountMouth1; i++)//摇摆
						{
							float Shake = (float)((Math.Cos(i / (float)maxCountMouth1 * Math.PI) + 1) * Math.Sin(i / 50f + Main.time * 0.05));
							Mouth1[i] += Shake * new Vector2(0, -0.15f);
						}
					}
					else
					{
						if (OldMouth1[0] == Vector2.Zero && NPC.localAI[0] <= 491)
						{
							for (int i = 0; i < maxCountMouth1; i++)
							{
								OldMouth1[i] = Mouth1[i];
							}
						}
						for (int i = 0; i < maxCountMouth1; i++)
						{
							Mouth1[i] = NPC.Center + new Vector2(0, 75) + (OldMouth1[i] - (NPC.Center + new Vector2(0, 75))).RotatedBy(-fx1);
						}
					}

					if (maxCountMouth1 > 380 && maxCountMouth1 != 399)//上翘
						Mouth1Vel = Mouth1Vel.RotatedBy(-0.1);
					if (maxCountMouth1 > 300)
					{
						if (Mouth1Vel.Length() < 3)
							Mouth1Vel *= 1.02f;
					}
					if (Mouth1[4] != Vector2.Zero)//平衡
						Mouth1[2] = Mouth1[2] * 0.9f + (Mouth1[4] + Mouth1[0]) * 0.05f;
					if (Mouth1[2] != Vector2.Zero)
						Mouth1[1] = Mouth1[1] * 0.9f + (Mouth1[2] + Mouth1[0]) * 0.05f;



					Mouth2[0] = NPC.Center + new Vector2(0, 75);
					int maxCountMouth2 = 0;//最大数量

					for (int f = Mouth2.Length - 1; f > 0; f--)//逐级检测
					{
						if (Mouth2[f - 1] != Vector2.Zero)//空值时,记录长度
						{
							maxCountMouth2 = f;
							break;
						}
					}
					if (maxCountMouth2 < 399)
					{
						for (int f = Mouth2.Length - 1; f > 0; f--)//逐级伸展
						{
							if (Mouth2[f - 1] != Vector2.Zero)//空值时,向前发展一步
							{
								Mouth2[f] = Mouth2[f - 1] + Mouth2Vel;
								break;
							}
						}
						for (int i = 0; i < maxCountMouth2; i++)
						{
							float Shake = (float)((Math.Cos(i / (float)maxCountMouth2 * Math.PI) + 1) * Math.Sin(i / 50f + Main.time * 0.05));
							Mouth2[i] += Shake * new Vector2(0, -0.15f);//摇摆
						}
					}
					else
					{
						if (OldMouth2[0] == Vector2.Zero && NPC.localAI[0] <= 491)
						{
							for (int i = 0; i < maxCountMouth1; i++)
							{
								OldMouth2[i] = Mouth2[i];
							}
						}
						for (int i = 0; i < maxCountMouth1; i++)//旋转
						{
							Mouth2[i] = NPC.Center + new Vector2(0, 75) + (OldMouth2[i] - (NPC.Center + new Vector2(0, 75))).RotatedBy(fx1);
						}
					}
					if (maxCountMouth2 > 380 && maxCountMouth1 != 399)//上翘
						Mouth2Vel = Mouth2Vel.RotatedBy(0.1);

					if (maxCountMouth2 > 300)
					{
						if (Mouth2Vel.Length() < 3)
							Mouth2Vel *= 1.02f;
					}
					if (Mouth2[4] != Vector2.Zero)//平衡
						Mouth2[2] = Mouth2[2] * 0.9f + (Mouth2[4] + Mouth2[0]) * 0.05f;
					if (Mouth2[2] != Vector2.Zero)
						Mouth2[1] = Mouth2[1] * 0.9f + (Mouth2[2] + Mouth2[0]) * 0.05f;
				}

				if (NPC.localAI[0] == 750)//降落
				{
					NPC.knockBackResist = 0;
					NPC.position = OriginalPosII;
					OriginalPos = NPC.position;
					AimPosHeight = new Vector2(0, 250);
				}
				if (NPC.localAI[0] < 810 && NPC.localAI[0] > 750)
				{
					float k = (NPC.localAI[0] - 750f) / 60f;
					NPC.position = OriginalPos + AimPosHeight * k * k;
					NPC.velocity *= 0.5f;
				}
				if (NPC.localAI[0] == 810)
				{
					NPC.velocity *= 0f;
					NPC.noGravity = false;
					OriginalPos = NPC.position;
					AimPosHeight = new Vector2(0, 250);
					BasePos = NPC.position;
				}
			}
			if (NPC.localAI[0] >= 850 && NPC.localAI[0] < 1150)//收缩,归中
			{
				for (int f = 0; f < NPC.buffType.Length; f++)
				{
					NPC.buffImmune[f] = true;
				}
				if (NPC.localAI[0] % 850 == 60)
					OSquzze = true;
				if (NPC.localAI[0] % 850 == 80)
					OSquzze = false;
				if (NPC.localAI[0] % 850 >= 80 && NPC.localAI[0] % 850 < 100)
				{
					if (NPC.localAI[0] % 850 == 80)
					{
						Dam = NPC.damage;
						NPC.damage = 0;
						NPC.dontTakeDamage = true;
						Transparent = true;
					}

					if (NPC.alpha >= 255)
					{
						NPC.alpha = 255;
						if (!player.active || player.dead)
							NPC.active = false;
					}
					else
					{
						NPC.alpha += 13;
					}
				}
				if (NPC.localAI[0] % 850 == 105)
				{
					NPC.position += new Vector2(0, -400).RotatedBy(Main.rand.NextFloat(-1.4f, -0.8f) * Math.Sign(NPC.Center.X - player.Center.X));
					if (NPC.position.X > FirstPos.X)
						NPC.position.X = FirstPos.X;
					if (NPC.position.X < FirstPos.X)
						NPC.position.X = FirstPos.X;
					NPC.velocity.Y = 10f;
				}
				if (NPC.localAI[0] % 850 == 250)
					DSquzze = true;
				if (NPC.localAI[0] % 850 == 270)
					DSquzze = false;
				if (NPC.localAI[0] % 850 >= 200 && NPC.localAI[0] % 850 < 220)
				{
					NPC.dontTakeDamage = false;
					NPC.damage = Dam;
					Transparent = false;
					if (NPC.alpha <= 0)
					{
						NPC.alpha = 0;
						NPC.damage = Dam;
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
				if (NPC.localAI[0] == 1453)
					NPC.NewNPC(null, (int)(NPC.Bottom.X + 600), (int)(NPC.Bottom.Y - 16), ModContent.NPCType<TuskWaveHuge>(), 0, -1, 1);
				if (NPC.localAI[0] == 1497)
					NPC.NewNPC(null, (int)(NPC.Bottom.X - 400), (int)(NPC.Bottom.Y - 16), ModContent.NPCType<TuskWaveHuge>(), 0, 0.6f, 0.8f);
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
			if (NPC.localAI[0] >= 3200 && NPC.localAI[0] < 3800)//扇面
			{
				int Freq = 60;
				if (Main.expertMode)
					Freq = 50;
				if (Main.masterMode)
					Freq = 40;
				int times = 8;
				if (Main.expertMode)
					times = 10;
				if (Main.masterMode)
					times = 12;
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
			if (NPC.localAI[0] >= 3800 && NPC.localAI[0] < 4400)//扫射
			{
				int Freq = 10;
				if (Main.expertMode)
					Freq = 7;
				if (Main.masterMode)
					Freq = 4;
				if (NPC.localAI[0] % Freq == 0)
				{
					var v0 = new Vector2(0, -16f);
					Vector2 v2 = v0.RotatedBy((NPC.localAI[0] % 41 - 20) * 0.04);
					v2.Y -= 3f;
					Projectile.NewProjectile(null, new Vector2(-3, 80) + NPC.Center, v2, ModContent.ProjectileType<Projectiles.CrimsonTuskProjGra>(), NPC.damage / 9, 1);
				}
			}
			if (NPC.localAI[0] >= 4400 && NPC.localAI[0] <= 5400)//层层落下
			{
				int Freq = 180;
				if (Main.expertMode)
					Freq = 150;
				if (Main.masterMode)
					Freq = 99;
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
				NPC.localAI[0] = 0;
		}//Phase2
		else
		{
			BasePos = NPC.position;
			//核心AI转化
			if (fightT >= 60)
			{
				if (NPC.localAI[0] > 6000)
					NPC.localAI[0] = 0;
			}
			if (NPC.localAI[0] < 600)
			{
				for (int f = 0; f < NPC.buffType.Length; f++)
				{
					NPC.buffImmune[f] = true;
				}
				if (NPC.localAI[0] % 300 == 60)
					OSquzze = true;
				if (NPC.localAI[0] % 300 == 80)
					OSquzze = false;
				if (NPC.localAI[0] % 300 >= 80 && NPC.localAI[0] % 300 < 100)
				{
					if (NPC.localAI[0] % 300 == 80)
					{
						Dam = NPC.damage;
						NPC.damage = 0;
						NPC.dontTakeDamage = true;
						Transparent = true;
					}

					if (NPC.alpha >= 255)
					{
						NPC.alpha = 255;
						if (!player.active || player.dead)
							NPC.active = false;
					}
					else
					{
						NPC.alpha += 13;
					}
				}
				if (NPC.localAI[0] % 300 == 105)
				{
					NPC.position += new Vector2(0, -400).RotatedBy(Main.rand.NextFloat(-1.4f, -0.8f) * Math.Sign(NPC.Center.X - player.Center.X));
					if (NPC.position.X > FirstPos.X + 600)
						NPC.position.X = FirstPos.X + 600;
					if (NPC.position.X < FirstPos.X - 600)
						NPC.position.X = FirstPos.X - 600;
					NPC.velocity.Y = 10f;
				}
				if (NPC.localAI[0] % 300 == 250)
					DSquzze = true;
				if (NPC.localAI[0] % 300 == 270)
					DSquzze = false;
				if (NPC.localAI[0] % 300 >= 200 && NPC.localAI[0] % 300 < 220)
				{
					NPC.dontTakeDamage = false;
					NPC.damage = Dam;
					Transparent = false;
					if (NPC.alpha <= 0)
					{
						NPC.alpha = 0;
						NPC.damage = Dam;
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
						Dist = 15;
					if (Main.expertMode && !Main.masterMode)
						Dist = 20;
					if (Main.masterMode)
						Dist = 24;
					for (int i = 1; i < Dist; i++)
					{
						int dX = Main.rand.Next(-5, 5);
						if (!Main.rand.NextBool(3))
							NPC.NewNPC(null, (int)NPC.Center.X + (int)(96 * i * 15f / Dist) + X + dX, (int)player.Center.Y - 20, ModContent.NPCType<LittleTusk>());
						else
						{
							NPC.NewNPC(null, (int)NPC.Center.X + (int)(96 * i * 15f / Dist) + X + dX, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
						}
						dX = Main.rand.Next(-5, 5);

						if (!Main.rand.NextBool(3))
							NPC.NewNPC(null, (int)NPC.Center.X - (int)(96 * i * 15f / Dist) + X + dX, (int)player.Center.Y - 20, ModContent.NPCType<LittleTusk>());
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
					Dist = 10;
				if (Main.expertMode && !Main.masterMode)
					Dist = 8;
				if (Main.masterMode)
					Dist = 6;
				if (NPC.localAI[0] % (Dist * 2) == Dist)
				{
					int X = Main.rand.Next(-15, 15);
					if (!Main.rand.NextBool(3))
						NPC.NewNPC(null, (int)NPC.Center.X + (int)(NPC.localAI[0] % 200) * 5 + X, (int)player.Center.Y - 20, ModContent.NPCType<LittleTusk>());
					else
					{
						NPC.NewNPC(null, (int)NPC.Center.X + (int)(NPC.localAI[0] % 200) * 5 + X, (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
					}
					X = Main.rand.Next(-15, 15);
					if (!Main.rand.NextBool(3))
						NPC.NewNPC(null, (int)NPC.Center.X - (int)(NPC.localAI[0] % 200) * 5 + X, (int)player.Center.Y - 20, ModContent.NPCType<LittleTusk>());
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
					V[8] = V[8] * 0.85f + new Vector2(0, 180) * 0.15f;
				if (NPC.localAI[0] < 1930 && NPC.localAI[0] > 1830)
					Hm += 1 / 100f;
				if (NPC.localAI[0] > 2300)
					Hm -= 1 / 100f;
				if (NPC.localAI[0] > 1850 && NPC.localAI[0] < 2350)
				{
					for (int i = 0; i < 10; i++)
					{
						Dust.NewDust(NPC.Center + new Vector2(-13, 60), 20, 20, DustID.Blood, 0, Main.rand.NextFloat(-15f, -9f) * Hm, 0, default, Main.rand.NextFloat(0.3f, 4f));
					}
					int Freq = 18;
					if (Main.expertMode && !Main.masterMode)
						Freq = 12;
					if (Main.masterMode)
						Freq = 10;
					if (NPC.localAI[0] % Freq == 1)
					{
						int r = /*NPC.NewNPC(null, (int)(NPC.Center.X - 3), (int)(NPC.Center.Y + 80), ModContent.NPCType<CrimsonTusk>())*/Projectile.NewProjectile(null, new Vector2(-3, 80) + NPC.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.CrimsonTuskProjGra>(), NPC.damage / 9, 1);
						Main.projectile[r].velocity = new Vector2(0, Main.rand.NextFloat(-26f, -12f)).RotatedBy(Main.rand.NextFloat(-0.5f, 0.5f));
					}
				}
				if (NPC.localAI[0] > 2370)
					V[8] = V[8] * 0.85f;
			}
			if (NPC.localAI[0] > 2400 && NPC.localAI[0] <= 2600)
			{
				int Freq = 15;
				if (Main.expertMode && !Main.masterMode)
					Freq = 10;
				if (Main.masterMode)
					Freq = 5;
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
					Freq = 10;
				if (Main.masterMode)
					Freq = 5;
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
					Freq = 10;
				if (Main.masterMode)
					Freq = 5;
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
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 100), new Vector2(0, -15).RotatedBy(-Math.Sin((NPC.localAI[0] - 3000) / 40f)), ModContent.ProjectileType<Projectiles.TuskSpice>(), NPC.damage / 9, 3f, Main.myPlayer, Math.Abs(((NPC.localAI[0] - 3000) / 4f % 70 - 35) * 18) + 240);
					if (NPC.localAI[0] % 24 == 12)
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 100), new Vector2(0, -15).RotatedBy(Math.Sin((NPC.localAI[0] - 3000) / 40f)), ModContent.ProjectileType<Projectiles.TuskSpice>(), NPC.damage / 9, 3f, Main.myPlayer, -Math.Abs(((NPC.localAI[0] - 3000) / 4f % 70 - 35) * 18) - 240);
				}
				if (Main.expertMode && !Main.masterMode)
				{
					if (NPC.localAI[0] % 12 == 0)
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 100), new Vector2(0, -15).RotatedBy(-Math.Sin((NPC.localAI[0] - 3000) / 40f)), ModContent.ProjectileType<Projectiles.TuskSpice>(), NPC.damage / 9, 3f, Main.myPlayer, Math.Abs(((NPC.localAI[0] - 3000) / 4f % 70 - 35) * 18) + 240);
					if (NPC.localAI[0] % 12 == 6)
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 100), new Vector2(0, -15).RotatedBy(Math.Sin((NPC.localAI[0] - 3000) / 40f)), ModContent.ProjectileType<Projectiles.TuskSpice>(), NPC.damage / 9, 3f, Main.myPlayer, -Math.Abs(((NPC.localAI[0] - 3000) / 4f % 70 - 35) * 18) - 240);
				}
				if (Main.masterMode)
				{
					if (NPC.localAI[0] % 6 == 0)
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 100), new Vector2(0, -15).RotatedBy(-Math.Sin((NPC.localAI[0] - 3000) / 40f)), ModContent.ProjectileType<Projectiles.TuskSpice>(), NPC.damage / 9, 3f, Main.myPlayer, Math.Abs(((NPC.localAI[0] - 3000) / 4f % 70 - 35) * 18) + 240);
					if (NPC.localAI[0] % 6 == 3)
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 100), new Vector2(0, -15).RotatedBy(Math.Sin((NPC.localAI[0] - 3000) / 40f)), ModContent.ProjectileType<Projectiles.TuskSpice>(), NPC.damage / 9, 3f, Main.myPlayer, -Math.Abs(((NPC.localAI[0] - 3000) / 4f % 70 - 35) * 18) - 240);
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
				/*if(NPC.localAI[0] == 4189)
                    {
                        lockBlood = true;
                    }*/
				if (NPC.localAI[0] == 3605)
				{
					Dx1 = (int)(player.Center.X - 3);
					NPC.NewNPC(null, Dx1, (int)(player.Center.Y - 10), ModContent.NPCType<BloodyMouth1>(), 0, 1, 0);
					NPC.NewNPC(null, Dx1, (int)(player.Center.Y - 10), ModContent.NPCType<BloodyMouth2>(), 0, -1, 0);
				}
				if (NPC.localAI[0] % 10 == 0 && NPC.localAI[0] >= 3605 && NPC.localAI[0] <= 3720)
					NPC.NewNPC(null, Dx1 + Main.rand.Next(260, 800), (int)player.Center.Y - 20, ModContent.NPCType<LittleTusk>());
				if (NPC.localAI[0] % 10 == 2 && NPC.localAI[0] >= 3605 && NPC.localAI[0] <= 3720)
					NPC.NewNPC(null, Dx1 - Main.rand.Next(260, 800), (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
				if (NPC.localAI[0] % 10 == 5 && NPC.localAI[0] >= 3605 && NPC.localAI[0] <= 3720)
					NPC.NewNPC(null, Dx1 - Main.rand.Next(260, 800), (int)player.Center.Y - 20, ModContent.NPCType<LittleTusk>());
				if (NPC.localAI[0] % 10 == 8 && NPC.localAI[0] >= 3605 && NPC.localAI[0] <= 3720)
					NPC.NewNPC(null, Dx1 + Main.rand.Next(260, 800), (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk>());
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
					Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center + new Vector2(-300, -270) + new Vector2(15, -1) * 68f, new Vector2(-45, 3), ModContent.ProjectileType<Projectiles.TuskCrack>(), NPC.damage, 3f, player.whoAmI, 0, 0f);
				}
			}
			if (NPC.localAI[0] > 4800 && NPC.localAI[0] <= 5400)
			{
				int MaxTusk = 6;
				if (Main.expertMode && !Main.masterMode)
					MaxTusk = 10;
				if (Main.masterMode)
					MaxTusk = 15;
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
					Freq = 9;
				if (Main.masterMode)
					Freq = 6;
				if (NPC.localAI[0] % Freq == 0)
				{
					NPC.NewNPC(null, (int)(NPC.Center.X - 1300 + (NPC.localAI[0] - 5400) % 200 * 13f), (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk2>());
					NPC.NewNPC(null, (int)(NPC.Center.X + 1300 - (NPC.localAI[0] - 5400) % 200 * 13f), (int)player.Center.Y - 20, ModContent.NPCType<LargeTusk2>());
				}
			}
		}


		if (NPC.life < NPC.lifeMax * 0.999f)
		{
			iMax = 2;
			if (Main.expertMode)
				iMax = 3;
			if (Main.masterMode)
				iMax = 4;
			Cen = NPC.Center + new Vector2(-3, 90);
			NPC.localAI[1] += 1;
			if (summon)
			{
				if (NPC.localAI[0] > 600)
				{
					for (int i = 0; i < iMax; i++)
					{
						N[i] = NPC.NewNPC(null, (int)(NPC.Center.X + 12), (int)(NPC.Center.Y + 90), ModContent.NPCType<CrimsonTuskControlable>(), 0, -1, i, (i - 1.5f) * 0.7f);
					}
					summon = false;
				}
			}
			if (NPC.life >= NPC.lifeMax * 0.5)
			{
				if (NPC.localAI[0] > 600)
				{
					for (int i = 0; i < iMax; i++)
					{
						if (NPC.localAI[1] % 200 > i * 50 && NPC.localAI[1] % 200 < (i + 1) * 50)
						{
							Vector2 v = player.Center - Main.npc[N[i]].Center;
							if (i % 2 == 0)
								v = new Vector2(-v.X, v.Y);
							v = v / v.Length() * 7f;
							Main.npc[N[i]].velocity = Main.npc[N[i]].velocity * 0.95f + v * 0.05f;
							int k = i + 1;
							if (k >= 4)
								k = 0;
							v = player.Center - Main.npc[N[k]].Center;
							if (i % 2 == 0)
								v = new Vector2(-v.X, v.Y);
							v = v / v.Length() * 7f;
							Main.npc[N[k]].velocity = Main.npc[N[k]].velocity * 0.975f + v * 0.025f;
						}
						if (Main.npc[N[i]].Center.Y > NPC.Center.Y - 240)
							Main.npc[N[i]].velocity.Y -= 0.25f;
						Back[i] = false;
					}
					if (NPC.localAI[1] > 600)
						NPC.localAI[1] = 0;
				}
				else
				{
					for (int i = 0; i < iMax; i++)
					{
						Vector2 v = Cen - Main.npc[N[i]].Center;
						if (v.Length() > 10 && !Back[i])
						{
							v = v / v.Length() * 10f;
							Main.npc[N[i]].velocity = Main.npc[N[i]].velocity * 0.95f + v * 0.05f;
						}
						else
						{
							if (!Back[i])
								Back[i] = true;
							Main.npc[N[i]].position = Cen - new Vector2(15, 20);
							Main.npc[N[i]].velocity = Main.npc[N[i]].velocity / Main.npc[N[i]].velocity.Length() * 0.5f;
						}
					}
				}
			}
			else
			{
				if (NPC.localAI[0] <= 850 || NPC.localAI[0] >= 1150)
				{
					for (int i = 0; i < iMax; i++)
					{
						if (NPC.localAI[1] % 200 > i * 50 && NPC.localAI[1] % 200 < (i + 1) * 50)
						{
							Vector2 v = player.Center - Main.npc[N[i]].Center;
							if (i % 2 == 0)
								v = new Vector2(-v.X, v.Y);
							v = v / v.Length() * 7f;
							Main.npc[N[i]].velocity = Main.npc[N[i]].velocity * 0.95f + v * 0.05f;
							int k = i + 1;
							if (k >= 4)
								k = 0;
							v = player.Center - Main.npc[N[k]].Center;
							if (i % 2 == 0)
								v = new Vector2(-v.X, v.Y);
							v = v / v.Length() * 7f;
							Main.npc[N[k]].velocity = Main.npc[N[k]].velocity * 0.975f + v * 0.025f;
						}
						if (Main.npc[N[i]].Center.Y > NPC.Center.Y - 240)
							Main.npc[N[i]].velocity.Y -= 0.25f;
						Back[i] = false;
					}
					if (NPC.localAI[1] > 600)
						NPC.localAI[1] = 0;
				}
				else
				{
					for (int i = 0; i < iMax; i++)
					{
						Vector2 v = Cen - Main.npc[N[i]].Center;
						if (v.Length() > 10 && !Back[i])
						{
							v = v / v.Length() * 10f;
							Main.npc[N[i]].velocity = Main.npc[N[i]].velocity * 0.95f + v * 0.05f;
						}
						else
						{
							if (!Back[i])
								Back[i] = true;
							Main.npc[N[i]].position = Cen - new Vector2(15, 20);
							Main.npc[N[i]].velocity = Main.npc[N[i]].velocity / Main.npc[N[i]].velocity.Length() * 0.5f;
						}
					}
				}
			}
		}
		if (!player.active || player.dead)
		{
			for (int i = 0; i < iMax; i++)
			{
				Main.npc[N[i]].ai[3] = 30;
			}
			NPC.alpha += 5;
			if (NPC.alpha > 250)
				NPC.active = false;
		}
		if (HasbeenKilled)
		{

		}
		if (ReallyStart)
		{
			NPC.localAI[2]++;
			if (NPC.localAI[2] < 600)
			{
				//测试部分
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
		/*if (locktime > 0)
            {
                NPC.dontTakeDamage = true;
                locktime--;
            }*/
	}

	private int Dx1 = 0;
	public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
	{
		bool flag = NPC.life <= 0;
		if (!flag)
		{
			float num = NPC.life / (float)NPC.lifeMax;
			bool flag2 = num > 1f;
			if (flag2)
				num = 1f;
			int num2 = (int)(36f * num);
			float num3 = position.X - 18f * scale;
			float num4 = position.Y;
			bool flag3 = Main.player[Main.myPlayer].gravDir == -1f;
			if (flag3)
			{
				num4 -= Main.screenPosition.Y;
				num4 = Main.screenPosition.Y + Main.screenHeight - num4;
			}
			float num5 = 0f;
			float num6 = 255f;
			num -= 0.1f;
			bool flag4 = (double)num > 0.5;
			float num7;
			float num8;
			if (flag4)
			{
				num7 = 255f;
				num8 = 255f * (1f - num) * 2f;
			}
			else
			{
				num7 = 255f * num * 2f;
				num8 = 255f;
			}
			float num9 = 0.95f;
			num8 *= num9;
			num7 *= num9;
			num6 *= num9;
			bool flag5 = num8 < 0f;
			if (flag5)
				num8 = 0f;
			bool flag6 = num8 > 255f;
			if (flag6)
				num8 = 255f;
			bool flag7 = num7 < 0f;
			if (flag7)
				num7 = 0f;
			bool flag8 = num7 > 255f;
			if (flag8)
				num7 = 255f;
			bool flag9 = num6 < 0f;
			if (flag9)
				num6 = 0f;
			bool flag10 = num6 > 255f;
			if (flag10)
				num6 = 255f;
			var color = new Color((byte)num8, (byte)num7, (byte)num5, (byte)num6);
			bool flag11 = num2 < 3;
			if (flag11)
				num2 = 3;
			bool flag12 = num2 < 38;
			if (flag12)
			{
				bool flag13 = num2 < 40;
				if (flag13)
					Main.spriteBatch.Draw(TextureAssets.Hb2.Value, new Vector2(num3 - Main.screenPosition.X + num2 * scale, num4 - Main.screenPosition.Y), new Rectangle?(new Rectangle(2, 0, 2, TextureAssets.Hb2.Height())), color, 0f, new Vector2(0f, 0f), scale, SpriteEffects.None, 0f);
				bool flag14 = num2 < 38;
				if (flag14)
					Main.spriteBatch.Draw(TextureAssets.Hb2.Value, new Vector2(num3 - Main.screenPosition.X + (num2 + 2) * scale, num4 - Main.screenPosition.Y), new Rectangle?(new Rectangle(num2 + 2, 0, 36 - num2 - 2, TextureAssets.Hb2.Height())), color, 0f, new Vector2(0f, 0f), scale, SpriteEffects.None, 0f);
				bool flag15 = num2 > 2;
				if (flag15)
					Main.spriteBatch.Draw(TextureAssets.Hb1.Value, new Vector2(num3 - Main.screenPosition.X, num4 - Main.screenPosition.Y), new Rectangle?(new Rectangle(0, 0, num2 - 2, TextureAssets.Hb1.Height())), color, 0f, new Vector2(0f, 0f), scale, SpriteEffects.None, 0f);
				bool flag16 = num2 > 18;//分血条
				if (flag16)
					Main.spriteBatch.Draw(TextureAssets.Hb1.Value, new Vector2(num3 - Main.screenPosition.X + 17, num4 - Main.screenPosition.Y), new Rectangle?(new Rectangle(0, 0, 4, TextureAssets.Hb1.Height())), color, 0f, new Vector2(0f, 0f), scale, SpriteEffects.None, 0f);
				else
				{
					Main.spriteBatch.Draw(TextureAssets.Hb2.Value, new Vector2(num3 - Main.screenPosition.X + 17, num4 - Main.screenPosition.Y), new Rectangle?(new Rectangle(0, 0, 4, TextureAssets.Hb1.Height())), color, 0f, new Vector2(0f, 0f), scale, SpriteEffects.None, 0f);
				}
				Main.spriteBatch.Draw(TextureAssets.Hb1.Value, new Vector2(num3 - Main.screenPosition.X + (num2 - 2) * scale, num4 - Main.screenPosition.Y), new Rectangle?(new Rectangle(32, 0, 2, TextureAssets.Hb1.Height())), color, 0f, new Vector2(0f, 0f), scale, SpriteEffects.None, 0f);
			}
			else
			{
				bool flag16 = num2 < 40;
				if (flag16)
					Main.spriteBatch.Draw(TextureAssets.Hb2.Value, new Vector2(num3 - Main.screenPosition.X + num2 * scale, num4 - Main.screenPosition.Y), new Rectangle?(new Rectangle(num2, 0, 36 - num2, TextureAssets.Hb2.Height())), color, 0f, new Vector2(0f, 0f), scale, SpriteEffects.None, 0f);
				bool flag17 = num2 > 18;//分血条
				if (flag17)
					Main.spriteBatch.Draw(TextureAssets.Hb1.Value, new Vector2(num3 - Main.screenPosition.X + 17, num4 - Main.screenPosition.Y), new Rectangle?(new Rectangle(0, 0, 4, TextureAssets.Hb1.Height())), color, 0f, new Vector2(0f, 0f), scale, SpriteEffects.None, 0f);
				else
				{
					Main.spriteBatch.Draw(TextureAssets.Hb2.Value, new Vector2(num3 - Main.screenPosition.X + 17, num4 - Main.screenPosition.Y), new Rectangle?(new Rectangle(0, 0, 4, TextureAssets.Hb1.Height())), color, 0f, new Vector2(0f, 0f), scale, SpriteEffects.None, 0f);
				}
				Main.spriteBatch.Draw(TextureAssets.Hb1.Value, new Vector2(num3 - Main.screenPosition.X, num4 - Main.screenPosition.Y), new Rectangle?(new Rectangle(0, 0, num2, TextureAssets.Hb1.Height())), color, 0f, new Vector2(0f, 0f), scale, SpriteEffects.None, 0f);
			}
		}
		return false;
	}
	public override bool PreAI()
	{
		NPC.TargetClosest(false);

		if (HasbeenKilled)
		{
			Killing--;
			if (Killing == 150)
			{
				for (int h = 0; h < 15; h++)
				{
					//Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(-3, 50), Vector2.Zero, ModContent.ProjectileType<Projectiles.TuskKillEffect>(), 0, 0, Main.myPlayer, 0);
					NPC.NewNPC(null, (int)NPC.Center.X - 3, (int)NPC.Center.Y + 50, ModContent.NPCType<Projectiles.TuskKillEffect>());
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
				float vFX = Main.rand.Next(-2000, 2000) / 5000f;

				var vF = new Vector2(vFX, -(float)Math.Cos(vFX * Math.PI) * 6f);
				Gore.NewGore(null, NPC.position, vF, ModContent.Find<ModGore>("Everglow/Myth/TheTusk/Gores/BloodTuskBroken1").Type, 1f);
				vFX = Main.rand.Next(-2000, 2000) / 5000f;
				vF = new Vector2(vFX, -(float)Math.Cos(vFX * Math.PI) * 6f);
				Gore.NewGore(null, NPC.position, vF, ModContent.Find<ModGore>("Everglow/Myth/TheTusk/Gores/BloodTuskBroken2").Type, 1f);
				vFX = Main.rand.Next(-2000, 2000) / 5000f;
				vF = new Vector2(vFX, -(float)Math.Cos(vFX * Math.PI) * 6f);
				Gore.NewGore(null, NPC.position, vF, ModContent.Find<ModGore>("Everglow/Myth/TheTusk/Gores/BloodTuskBroken3").Type, 1f);
				vFX = Main.rand.Next(-2000, 2000) / 5000f;
				vF = new Vector2(vFX, -(float)Math.Cos(vFX * Math.PI) * 12f);
				Gore.NewGore(null, NPC.position, vF, ModContent.Find<ModGore>("Everglow/Myth/TheTusk/Gores/BloodTuskBroken4").Type, 1f);
				vFX = Main.rand.Next(-2000, 2000) / 5000f;
				vF = new Vector2(vFX, -(float)Math.Cos(vFX * Math.PI) * 12f);
				Gore.NewGore(null, NPC.position, vF, ModContent.Find<ModGore>("Everglow/Myth/TheTusk/Gores/BloodTuskBroken5").Type, 1f);
				vFX = Main.rand.Next(-2000, 2000) / 5000f;
				vF = new Vector2(vFX, -(float)Math.Cos(vFX * Math.PI) * 12f);
				Gore.NewGore(null, NPC.position, vF, ModContent.Find<ModGore>("Everglow/Myth/TheTusk/Gores/BloodTuskBroken6").Type, 1f);
				if (!Main.expertMode && !Main.masterMode)
				{
					int itemType = ModContent.ItemType<Items.Weapons.ToothKnife>();
					h = Main.rand.Next(6);
					if (h == 1)
						itemType = ModContent.ItemType<Items.Weapons.ToothStaff>();
					if (h == 2)
						itemType = ModContent.ItemType<Items.Accessories.TuskLace>();
					if (h == 3)
						itemType = ModContent.ItemType<Items.Weapons.ToothMagicBall>();
					if (h == 4)
						itemType = ModContent.ItemType<Items.Weapons.BloodyBoneYoyo>();
					if (h == 5)
						itemType = ModContent.ItemType<Items.Weapons.SpineGun>();
					Item.NewItem(null, NPC.Hitbox, itemType);
				}
				else
				{
					Item.NewItem(null, NPC.Hitbox, ModContent.ItemType<Items.BossDrop.TuskTreasureBag>());
					if (Main.masterMode)
						Item.NewItem(null, NPC.Hitbox, ModContent.ItemType<Items.BossDrop.TuskRelic>());
				}
				if (Main.rand.NextBool(10))
					Item.NewItem(null, NPC.Hitbox, ModContent.ItemType<Items.BossDrop.BloodyTuskTrophy>());

				OSquzze = false;
				DSquzze = false;
				Transparent = false;
				fir = 9999999;
				Dam = 0;
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
	private bool[] Back = new bool[4];
	private bool summon = true;
	private int iMax = 0;
	private bool HasbeenKilled = false;
	public static int Killing = 0;
	//int locktime = 0;
	public override void HitEffect(NPC.HitInfo hit)
	{
		/*if (NPC.life < NPC.lifeMax / 2)
            {
                if(NPC.localAI[0] > 600)
                {
                    if (!lockBlood)
                    {
                        lockBlood = true;
                        locktime = 1600;
                        NPC.dontTakeDamage = true;
                    }
                }
            }*/
		if (!ReallyStart)
		{
			ReallyStart = true;
			if (!Main.dedServ)
				Music = Common.MythContent.QuickMusic("TuskFighting");
			Main.NewText("The Tusk has awoken!", 175, 75, 255);
			NPC.NewNPC(null, (int)NPC.Center.X - 800, (int)NPC.Center.Y - 200, ModContent.NPCType<TuskWallLeft>());
			NPC.NewNPC(null, (int)NPC.Center.X + 800, (int)NPC.Center.Y - 200, ModContent.NPCType<TuskWallRight>());
		}
		NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<TuskCooling>());
		if (NPC.life <= 0)
		{
			if (!HasbeenKilled)
			{
				NPC.life = 1;
				NPC.dontTakeDamage = true;
				HasbeenKilled = true;
				NPC.active = true;
				Killing = 180;
				for (int i = 0; i < iMax; i++)
				{
					Main.npc[N[i]].ai[3] = 30;
				}
			}
		}
	}
	public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
	{
		target.AddBuff(BuffID.Bleeding, 120);
	}
	private bool startFight = false;
	private bool ReallyStart = false;
	private int fightT = 0;
	private int HasTranSkin = 240;
	private int[] Eye = new int[7];
	private int[] EyeMotivate = new int[7];
	private float[] EyeRot = new float[7];
	private Vector2[] EyeMove = new Vector2[7];
	private Vector2 tuskHitMove = Vector2.Zero;
	private float SprK = 0.9f;
	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		if (!startFight)
			return false;
		Color color = Lighting.GetColor((int)(NPC.Center.X / 16d), (int)(NPC.Center.Y / 16d));
		color = NPC.GetAlpha(color) * ((255 - NPC.alpha) / 255f);
		color.R = (byte)Math.Clamp(color.R + tuskHitMove.Length() * tuskHitMove.Length() * 10f, 0f, 255f);
		color.G = (byte)Math.Clamp(color.G - tuskHitMove.Length() * tuskHitMove.Length() * 5f, 0f, 255f);
		color.B = (byte)Math.Clamp(color.B - tuskHitMove.Length() * tuskHitMove.Length() * 5f, 0f, 255f);
		color.A = (byte)Math.Clamp(color.A - tuskHitMove.Length() * tuskHitMove.Length() * 3f, 0f, 255f);
		Texture2D TuskBaseP1 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskTeethPhase1").Value;
		Texture2D TuskS1P1 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskS1P1").Value;
		Texture2D TuskS2P1 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskS2P1").Value;
		Texture2D TuskS3P1 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskS3bP1").Value;
		Texture2D TuskS4P1 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskS4bP1").Value;
		Texture2D TuskS5P1 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskS5P1").Value;
		Texture2D TuskS6P1 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskS6P1").Value;
		Texture2D TuskS7P1 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskS7P1").Value;
		Texture2D TuskS8P1 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskS8P1").Value;

		Texture2D TuskBase = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskTeeth").Value;
		Texture2D TuskBaseBlack = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskTeethBlack").Value;
		Texture2D TuskBaseE1 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskEye1").Value;
		Texture2D TuskBaseE2 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskEye2").Value;
		Texture2D TuskBaseE3 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskEye3").Value;
		Texture2D TuskBaseE4 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskEye4").Value;
		Texture2D TuskBaseE5 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskEye5").Value;
		Texture2D TuskBaseE6 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskEye6").Value;
		Texture2D TuskBaseE7 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskEye7").Value;
		Texture2D TuskS1 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskS1").Value;
		Texture2D TuskS2 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskS2").Value;
		Texture2D TuskS3 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskS3b").Value;
		Texture2D TuskS4 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskS4b").Value;
		Texture2D TuskS5 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskS5").Value;
		Texture2D TuskS6 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskS6").Value;
		Texture2D TuskS7 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskS7").Value;
		Texture2D TuskS8 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskS8").Value;

		if (!Main.gamePaused)
			tuskHitMove *= SprK;
		if (NPC.life < NPC.lifeMax / 2f)
		{
			if (!Main.gamePaused)
			{
				for (int i = 0; i < 7; i++)
				{
					if (Main.rand.NextBool(400) && Eye[i] == 0 && EyeMove[i] == Vector2.Zero)
						Eye[i] = 60;
					if (Eye[i] > 0)
						Eye[i]--;
					int Eyevalue = 30 - Math.Abs(Eye[i] - 30);
					if (Main.rand.NextBool(400) && EyeMove[i] == Vector2.Zero && Eye[i] == 0)
					{
						EyeMotivate[i] = 180;
						EyeRot[i] = Main.rand.NextFloat(0, 6.283f);
					}
					float Ran = (float)Math.Sqrt(Math.Clamp(90 - Math.Abs(EyeMotivate[i] - 90), 0f, 25f) / 25f);
					float Rot = 90 - Math.Abs(EyeMotivate[i] - 90) / 30f;
					EyeMove[i] = new Vector2(0, Ran * 1.5f).RotatedBy(Rot * Rot + EyeRot[i]);
					if (EyeMotivate[i] > 0)
					{
						EyeMotivate[i]--;
						if (EyeMotivate[i] == 0)
							EyeMove[i] = Vector2.Zero;
					}
					if (i == 0)
					{
						if (Eyevalue > 6 && Eyevalue <= 12)
							TuskBaseE1 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskEye1C").Value;
						if (Eyevalue > 12 && Eyevalue <= 18)
							TuskBaseE1 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskEye1L").Value;
						if (Eyevalue > 18 && Eyevalue <= 24)
							TuskBaseE1 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskEye1O").Value;
						if (Eyevalue > 24)
							TuskBaseE1 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTusk_Head_Boss_Void").Value;
					}
					if (i == 1)
					{
						if (Eyevalue > 16 && Eyevalue <= 24)
							TuskBaseE2 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskEye2C").Value;
						if (Eyevalue > 24)
							TuskBaseE2 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTusk_Head_Boss_Void").Value;
					}
					if (i == 2)
					{
						if (Eyevalue > 16 && Eyevalue <= 24)
							TuskBaseE3 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskEye3C").Value;
						if (Eyevalue > 24)
							TuskBaseE3 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTusk_Head_Boss_Void").Value;
					}
					if (i == 3)
					{
						if (Eyevalue > 18)
							TuskBaseE4 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTusk_Head_Boss_Void").Value;
					}
					if (i == 4)
					{
						if (Eyevalue > 18)
							TuskBaseE5 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTusk_Head_Boss_Void").Value;
					}
					if (i == 5)
					{
						if (Eyevalue > 18)
							TuskBaseE6 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTusk_Head_Boss_Void").Value;
					}
					if (i == 6)
					{
						if (Eyevalue > 18)
							TuskBaseE7 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTusk_Head_Boss_Void").Value;
					}
				}
			}
			//绘制二阶段獠牙
			if (NPC.localAI[0] < 60)
			{
				for (int nm = 0; nm < 10; nm++)
				{
					V[nm] = V[nm] * 0.85f;
				}
				if (NPC.alpha > 0)
					NPC.alpha -= 13;
				else
				{
					NPC.alpha = 0;
				}
			}
			if (NPC.localAI[0] < 750 && NPC.localAI[0] > 90)
			{
				//向下开口
				var bars1 = new List<VertexBase.CustomVertexInfo>();
				var bars2 = new List<VertexBase.CustomVertexInfo>();

				for (int i = 1; i < 400; ++i)//右侧
				{
					if (Mouth1[i] != Vector2.Zero)
					{
						float width = 9;
						if (i > 380)
							width = 9 + 6 * (float)(Math.Sin((i - 380) / 20d * Math.PI) + 1);
						if (NPC.localAI[0] >= 700)
							width *= Math.Clamp((750 - NPC.localAI[0]) / 50f, 0f, 1f);
						var normalDir = Mouth1[i - 1] - Mouth1[i];
						normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
						Color colori = Lighting.GetColor((int)(Mouth1[i].X / 16d), (int)(Mouth1[i].Y / 16d));
						var factor = Math.Abs(i / 30f % 2 - 1);

						var w2 = MathHelper.Lerp(1f, 0.0f, 0);
						bars1.Add(new VertexBase.CustomVertexInfo(Mouth1[i] + normalDir * width + new Vector2(0, -10) - Main.screenPosition, colori, new Vector3(factor, 1, w2)));
						bars1.Add(new VertexBase.CustomVertexInfo(Mouth1[i] + normalDir * -width + new Vector2(0, -10) - Main.screenPosition, colori, new Vector3(factor, 0, w2)));

						if (i % 24 == 3 && i < 350)
						{
							Main.spriteBatch.End();
							Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
							float Len = (float)(NPC.localAI[0] - 373 - Math.Sqrt(i));
							var Tusk1 = new List<VertexBase.CustomVertexInfo>
							{
								new VertexBase.CustomVertexInfo(Mouth1[i] + normalDir.RotatedBy(1.57) * width + new Vector2(0, -10) - Main.screenPosition, colori, new Vector3(0, 0, 0)),
								new VertexBase.CustomVertexInfo(Mouth1[i] + normalDir.RotatedBy(1.57) * -width + new Vector2(0, -10) - Main.screenPosition, colori, new Vector3(1, 0, 0)),
								new VertexBase.CustomVertexInfo(Mouth1[i] + new Vector2(0, -10) - normalDir * Math.Clamp(Len, 0, HangMaxL1[i]) - Main.screenPosition, colori, new Vector3(0.5f, 1, 0))
							};
							Texture2D t1 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/CrimsonTuskHang").Value;
							Main.graphics.GraphicsDevice.Textures[0] = t1;//GlodenBloodScaleMirror
							Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Tusk1.ToArray(), 0, Tusk1.Count / 3);
							if (!Main.gamePaused)
							{
								for (int j = 0; j < Main.player.Length; j++)
								{
									if (!Main.player[j].dead)
									{
										if ((Main.player[j].Center - (Mouth1[i] + new Vector2(0, -10) + normalDir * Math.Clamp(Len, 0, HangMaxL1[i]))).Length() < 30)
											Projectile.NewProjectile(null, Main.player[j].Center, Vector2.Zero, ModContent.ProjectileType<playerHit>(), NPC.damage / 8, 0, j, 0, 0);
									}
								}
							}
							Main.spriteBatch.End();
							Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
						}
						if (i % 8 == 3 && i >= 350)
						{
							Main.spriteBatch.End();
							Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
							float Len = (float)(NPC.localAI[0] - 373 - Math.Sqrt(i));
							var Tusk1 = new List<VertexBase.CustomVertexInfo>
							{
								new VertexBase.CustomVertexInfo(Mouth1[i] + normalDir.RotatedBy(1.57) * width + new Vector2(0, -10) - Main.screenPosition, colori, new Vector3(0, 0, 0)),
								new VertexBase.CustomVertexInfo(Mouth1[i] + normalDir.RotatedBy(1.57) * -width + new Vector2(0, -10) - Main.screenPosition, colori, new Vector3(1, 0, 0)),
								new VertexBase.CustomVertexInfo(Mouth1[i] + new Vector2(0, -10) - normalDir * Math.Clamp(Len, 0, HangMaxL1[i]) - Main.screenPosition, colori, new Vector3(0.5f, 1, 0))
							};
							Texture2D t1 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/CrimsonTuskHang").Value;
							Main.graphics.GraphicsDevice.Textures[0] = t1;//GlodenBloodScaleMirror
							Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Tusk1.ToArray(), 0, Tusk1.Count / 3);
							if (!Main.gamePaused)
							{
								for (int j = 0; j < Main.player.Length; j++)
								{
									if (!Main.player[j].dead)
									{
										if ((Main.player[j].Center - (Mouth1[i] + new Vector2(0, -10) + normalDir * Math.Clamp(Len, 0, HangMaxL1[i]))).Length() < 30)
											Projectile.NewProjectile(null, Main.player[j].Center, Vector2.Zero, ModContent.ProjectileType<playerHit>(), NPC.damage / 8, 0, j, 0, 0);
									}
								}
							}
							Main.spriteBatch.End();
							Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
						}
					}
				}

				for (int i = 399; i > 0; --i)//左侧
				{
					if (Mouth2[i] != Vector2.Zero)
					{
						float width = 9;
						if (i > 380)
							width = 9 + 6 * (float)(Math.Sin((i - 380) / 20d * Math.PI) + 1);
						if (NPC.localAI[0] >= 700)
							width *= Math.Clamp((750 - NPC.localAI[0]) / 50f, 0f, 1f);
						var normalDir = Mouth2[i - 1] - Mouth2[i];
						normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
						Color colori = Lighting.GetColor((int)(Mouth2[i].X / 16d), (int)(Mouth2[i].Y / 16d));
						var factor = Math.Abs(i / 30f % 2 - 1);

						var w2 = MathHelper.Lerp(1f, 0.0f, 0);
						bars2.Add(new VertexBase.CustomVertexInfo(Mouth2[i] + normalDir * width + new Vector2(0, -10) - Main.screenPosition, colori, new Vector3(factor, 1, w2)));
						bars2.Add(new VertexBase.CustomVertexInfo(Mouth2[i] + normalDir * -width + new Vector2(0, -10) - Main.screenPosition, colori, new Vector3(factor, 0, w2)));
						if (i % 24 == 3 && i < 350)
						{
							Main.spriteBatch.End();
							Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
							float Len = (float)(NPC.localAI[0] - 373 - Math.Sqrt(i));
							var Tusk2 = new List<VertexBase.CustomVertexInfo>
							{
								new VertexBase.CustomVertexInfo(Mouth2[i] + normalDir.RotatedBy(1.57) * width + new Vector2(0, -10) - Main.screenPosition, colori, new Vector3(0, 0, 0)),
								new VertexBase.CustomVertexInfo(Mouth2[i] + normalDir.RotatedBy(1.57) * -width + new Vector2(0, -10) - Main.screenPosition, colori, new Vector3(1, 0, 0)),
								new VertexBase.CustomVertexInfo(Mouth2[i] + new Vector2(0, -10) + normalDir * Math.Clamp(Len, 0, HangMaxL2[i]) - Main.screenPosition, colori, new Vector3(0.5f, 1, 0))
							};
							Texture2D t2 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/CrimsonTuskHang").Value;
							Main.graphics.GraphicsDevice.Textures[0] = t2;//GlodenBloodScaleMirror
							Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Tusk2.ToArray(), 0, Tusk2.Count / 3);
							if (!Main.gamePaused)
							{
								for (int j = 0; j < Main.player.Length; j++)
								{
									if (!Main.player[j].dead)
									{
										if ((Main.player[j].Center - (Mouth2[i] + new Vector2(0, -10) + normalDir * Math.Clamp(Len, 0, HangMaxL2[i]))).Length() < 30)
											Projectile.NewProjectile(null, Main.player[j].Center, Vector2.Zero, ModContent.ProjectileType<playerHit>(), NPC.damage / 8, 0, j, 0, 0);
									}
								}
							}
							Main.spriteBatch.End();
							Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
						}
						if (i % 8 == 3 && i >= 350)
						{
							Main.spriteBatch.End();
							Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
							float Len = (float)(NPC.localAI[0] - 373 - Math.Sqrt(i));
							var Tusk2 = new List<VertexBase.CustomVertexInfo>
							{
								new VertexBase.CustomVertexInfo(Mouth2[i] + normalDir.RotatedBy(1.57) * width + new Vector2(0, -10) - Main.screenPosition, colori, new Vector3(0, 0, 0)),
								new VertexBase.CustomVertexInfo(Mouth2[i] + normalDir.RotatedBy(1.57) * -width + new Vector2(0, -10) - Main.screenPosition, colori, new Vector3(1, 0, 0)),
								new VertexBase.CustomVertexInfo(Mouth2[i] + new Vector2(0, -10) + normalDir * Math.Clamp(Len, 0, HangMaxL2[i]) - Main.screenPosition, colori, new Vector3(0.5f, 1, 0))
							};
							Texture2D t2 = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/CrimsonTuskHang").Value;
							Main.graphics.GraphicsDevice.Textures[0] = t2;//GlodenBloodScaleMirror
							Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Tusk2.ToArray(), 0, Tusk2.Count / 3);
							if (!Main.gamePaused)
							{
								for (int j = 0; j < Main.player.Length; j++)
								{
									if (!Main.player[j].dead)
									{
										if ((Main.player[j].Center - (Mouth2[i] + new Vector2(0, -10) + normalDir * Math.Clamp(Len, 0, HangMaxL2[i]))).Length() < 30)
											Projectile.NewProjectile(null, Main.player[j].Center, Vector2.Zero, ModContent.ProjectileType<playerHit>(), NPC.damage / 8, 0, j, 0, 0);
									}
								}
							}
							Main.spriteBatch.End();
							Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
						}
					}
				}
				var triangleList1 = new List<VertexBase.CustomVertexInfo>();
				var triangleList2 = new List<VertexBase.CustomVertexInfo>();
				if (bars1.Count > 2)
				{
					Main.spriteBatch.End();
					Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
					triangleList1.Add(bars1[^2]);
					var vertex = new VertexBase.CustomVertexInfo((bars1[^2].Position + bars1[^1].Position) * 0.5f + Mouth1Vel * 15f, Color.White, new Vector3(0, 0.5f, 0));
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
					Texture2D t1 = ModContent.Request<Texture2D>("Everglow/Myth/UIImages/Tusk/BloodRope").Value;
					Main.graphics.GraphicsDevice.Textures[0] = t1;//GlodenBloodScaleMirror
					Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList1.ToArray(), 0, triangleList1.Count / 3);

					Main.spriteBatch.End();
					Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
				}

				if (bars2.Count > 2)
				{
					Main.spriteBatch.End();
					Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
					triangleList2.Add(bars2[0]);
					var vertex = new VertexBase.CustomVertexInfo((bars2[0].Position + bars2[1].Position) * 0.5f + Mouth2Vel * 15f, Color.White, new Vector3(0, 0.5f, 0));
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
					Texture2D t1 = ModContent.Request<Texture2D>("Everglow/Myth/UIImages/Tusk/BloodRope").Value;
					Main.graphics.GraphicsDevice.Textures[0] = t1;//GlodenBloodScaleMirror
					Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList2.ToArray(), 0, triangleList2.Count / 3);

					Main.spriteBatch.End();
					Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
				}
			}

			Main.spriteBatch.Draw(TuskS3, NPC.position - Main.screenPosition + new Vector2(15, 94) + V[2], new Rectangle?(NPC.frame), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS4, NPC.position - Main.screenPosition + new Vector2(15, 90) + V[3], new Rectangle?(NPC.frame), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskFleshBack").Value, NPC.position - Main.screenPosition + new Vector2(15, 90) + V[9], new Rectangle(0, 0, 220, (int)(312 - V[9].Y * 2f)), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);

			Color Blc = NPC.GetAlpha(Color.Black) * ((255 - NPC.alpha) / 255f);
			Main.spriteBatch.Draw(TuskBaseBlack, NPC.position - Main.screenPosition + new Vector2(15, 90) + V[8] + tuskHitMove, new Rectangle(0, 0, 220, (int)(312 - V[8].Y * 1.5f)), Blc, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);

			Color Alc = NPC.GetAlpha(new Color(255, 255, 255, 150)) * ((255 - NPC.alpha) / 255f);
			Main.spriteBatch.Draw(TuskBaseE1, NPC.position - Main.screenPosition + new Vector2(15, 90) + V[8] + EyeMove[0] + tuskHitMove * 0.9f, new Rectangle(0, 0, 220, (int)(312 - V[8].Y * 1.5f)), Alc, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskBaseE2, NPC.position - Main.screenPosition + new Vector2(15, 90) + V[8] + EyeMove[1] + tuskHitMove * 0.9f, new Rectangle(0, 0, 220, (int)(312 - V[8].Y * 1.5f)), Alc, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskBaseE3, NPC.position - Main.screenPosition + new Vector2(15, 90) + V[8] + EyeMove[2] + tuskHitMove * 0.9f, new Rectangle(0, 0, 220, (int)(312 - V[8].Y * 1.5f)), Alc, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskBaseE4, NPC.position - Main.screenPosition + new Vector2(15, 90) + V[8] + EyeMove[3] + tuskHitMove * 0.9f, new Rectangle(0, 0, 220, (int)(312 - V[8].Y * 1.5f)), Alc, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskBaseE5, NPC.position - Main.screenPosition + new Vector2(15, 90) + V[8] + EyeMove[4] + tuskHitMove * 0.9f, new Rectangle(0, 0, 220, (int)(312 - V[8].Y * 1.5f)), Alc, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskBaseE6, NPC.position - Main.screenPosition + new Vector2(15, 90) + V[8] + EyeMove[5] + tuskHitMove * 0.9f, new Rectangle(0, 0, 220, (int)(312 - V[8].Y * 1.5f)), Alc, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskBaseE7, NPC.position - Main.screenPosition + new Vector2(15, 90) + V[8] + EyeMove[6] + tuskHitMove * 0.9f, new Rectangle(0, 0, 220, (int)(312 - V[8].Y * 1.5f)), Alc, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);

			Main.spriteBatch.Draw(TuskBase, NPC.position - Main.screenPosition + new Vector2(15, 90) + V[8] + tuskHitMove, new Rectangle(0, 0, 220, (int)(312 - V[8].Y * 1.5f)), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS1, NPC.position - Main.screenPosition + new Vector2(15, 90) + V[0], new Rectangle?(NPC.frame), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS2, NPC.position - Main.screenPosition + new Vector2(15, 90) + V[1], new Rectangle?(NPC.frame), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS5, NPC.position - Main.screenPosition + new Vector2(15, 90) + V[4], new Rectangle?(NPC.frame), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS6, NPC.position - Main.screenPosition + new Vector2(15, 90) + V[5], new Rectangle?(NPC.frame), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS7, NPC.position - Main.screenPosition + new Vector2(15, 90) + V[6], new Rectangle?(NPC.frame), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS8, NPC.position - Main.screenPosition + new Vector2(15, 90) + V[7], new Rectangle?(NPC.frame), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
		}
		else
		{

			//绘制一阶段獠牙
			Main.spriteBatch.Draw(TuskS3P1, NPC.position - Main.screenPosition + new Vector2(15, 94) + V[2], new Rectangle?(NPC.frame), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS4P1, NPC.position - Main.screenPosition + new Vector2(15, 90) + V[3], new Rectangle?(NPC.frame), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskFleshBack").Value, NPC.position - Main.screenPosition + new Vector2(15, 90) + V[9], new Rectangle(0, 0, 220, (int)(312 - V[9].Y * 2f)), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskBaseP1, NPC.position - Main.screenPosition + new Vector2(15, 90) + V[8] + tuskHitMove, new Rectangle(0, 0, 220, (int)(312 - V[8].Y * 1.5f)), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS1P1, NPC.position - Main.screenPosition + new Vector2(15, 90) + V[0], new Rectangle?(NPC.frame), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS2P1, NPC.position - Main.screenPosition + new Vector2(15, 90) + V[1], new Rectangle?(NPC.frame), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS5P1, NPC.position - Main.screenPosition + new Vector2(15, 90) + V[4], new Rectangle?(NPC.frame), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS6P1, NPC.position - Main.screenPosition + new Vector2(15, 90) + V[5], new Rectangle?(NPC.frame), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS7P1, NPC.position - Main.screenPosition + new Vector2(15, 90) + V[6], new Rectangle?(NPC.frame), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS8P1, NPC.position - Main.screenPosition + new Vector2(15, 90) + V[7], new Rectangle?(NPC.frame), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
		}
		Effect ef = ModContent.Request<Effect>("Everglow/Myth/Effects/TuskFade").Value;
		if (NPC.life < NPC.lifeMax / 2f && HasTranSkin > 0)
		{
			NPC.localAI[0] = 0;
			if (HasTranSkin > 2)
			{
				NPC.ai[2] = 254;
				NPC.dontTakeDamage = true;
			}
			else
			{
				NPC.ai[2] = 0;
				NPC.dontTakeDamage = false;
			}

			//阶段切换shader
			if (!Main.gamePaused)
				HasTranSkin--;
			if (HasTranSkin == 239)
				SoundEngine.PlaySound(new SoundStyle("Everglow/Myth/Sounds/TuskCrack")/*, NPC.Bottom*/); //Camera moves to boss when going in phase 2. ~Setnour6
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			ef.Parameters["minr"].SetValue(1 - HasTranSkin / 180f);
			ef.Parameters["uImage1"].SetValue(ModContent.Request<Texture2D>("Everglow/Myth/UIImages/Tusk/Perlin").Value);

			ef.CurrentTechnique.Passes["Test"].Apply();
			Color cg = color;
			ef.Parameters["BackCol"].SetValue(new Vector4(color.R, color.G, color.B, 255 - NPC.alpha) / 255f);
			float alp = (255 - NPC.alpha) / 255f;
			cg = new Color(color.R / 255f * alp, color.G / 255f * alp, color.B / 255f * alp, alp);
			Main.spriteBatch.Draw(TuskS3P1, NPC.position - Main.screenPosition + new Vector2(15, 94) + V[2], new Rectangle?(NPC.frame), cg, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS4P1, NPC.position - Main.screenPosition + new Vector2(15, 90) + V[3], new Rectangle?(NPC.frame), cg, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskFleshBack").Value, NPC.position - Main.screenPosition + new Vector2(15, 90) + V[9], new Rectangle(0, 0, 220, (int)(312 - V[9].Y * 2f)), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskBaseP1, NPC.position - Main.screenPosition + new Vector2(15, 90) + V[8] + tuskHitMove, new Rectangle(0, 0, 220, (int)(312 - V[8].Y * 1.5f)), cg, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS1P1, NPC.position - Main.screenPosition + new Vector2(15, 90) + V[0], new Rectangle?(NPC.frame), cg, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS2P1, NPC.position - Main.screenPosition + new Vector2(15, 90) + V[1], new Rectangle?(NPC.frame), cg, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS5P1, NPC.position - Main.screenPosition + new Vector2(15, 90) + V[4], new Rectangle?(NPC.frame), cg, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS6P1, NPC.position - Main.screenPosition + new Vector2(15, 90) + V[5], new Rectangle?(NPC.frame), cg, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS7P1, NPC.position - Main.screenPosition + new Vector2(15, 90) + V[6], new Rectangle?(NPC.frame), cg, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(TuskS8P1, NPC.position - Main.screenPosition + new Vector2(15, 90) + V[7], new Rectangle?(NPC.frame), cg, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		}

		//Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskFlesh2").Value, NPC.position - Main.screenPosition + new Vector2(15, 88) + V[9] + new Vector2(0, -1), new Rectangle(0, 0, 220, (int)(312 - V[9].Y * 2f)), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
		if (BasePos != Vector2.Zero)
		{
			//拉丝底座
			Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskFlesh1").Value, BasePos - Main.screenPosition + new Vector2(15, 90) + V[9], new Rectangle(0, 0, 220, (int)(312 - V[9].Y * 2f)), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);

			if (BasePos != NPC.position)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

				for (int z = 0; z < 4; z++)
				{
					hangItem[z] = new Vector2(0, 25 * (float)Math.Sin(Main.time / 70d + z) * (BasePos.Y - NPC.position.Y) / 50f);
					var Vx2 = new List<VertexBase.CustomVertexInfo>
					{
						new VertexBase.CustomVertexInfo(NPC.position + new Vector2(-95, 162) - Main.screenPosition, color, new Vector3(0, 0, 0)),
						new VertexBase.CustomVertexInfo(NPC.position + new Vector2(125, 162) - Main.screenPosition, color, new Vector3(1, 0, 0)),
						new VertexBase.CustomVertexInfo(BasePos + new Vector2(125, 172) + hangItem[z] - Main.screenPosition, color, new Vector3(1, 1, 0)),

						new VertexBase.CustomVertexInfo(NPC.position + new Vector2(-95, 162) - Main.screenPosition, color, new Vector3(0, 0, 0)),
						new VertexBase.CustomVertexInfo(BasePos + new Vector2(125, 172) + hangItem[z] - Main.screenPosition, color, new Vector3(1, 1, 0)),
						new VertexBase.CustomVertexInfo(BasePos + new Vector2(-95, 172) + hangItem[z] - Main.screenPosition, color, new Vector3(0, 1, 0))
					};
					Texture2D thang = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskDragLine" + (3 + z).ToString()).Value;
					Main.graphics.GraphicsDevice.Textures[0] = thang;
					Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx2.ToArray(), 0, Vx2.Count / 3);
				}

				var Vx = new List<VertexBase.CustomVertexInfo>
				{
					new VertexBase.CustomVertexInfo(NPC.position + new Vector2(-95, 162) - Main.screenPosition, color, new Vector3(0, 0, 0)),
					new VertexBase.CustomVertexInfo(NPC.position + new Vector2(125, 162) - Main.screenPosition, color, new Vector3(1, 0, 0)),
					new VertexBase.CustomVertexInfo(BasePos + new Vector2(125, 172) - Main.screenPosition, color, new Vector3(1, 1, 0)),

					new VertexBase.CustomVertexInfo(NPC.position + new Vector2(-95, 162) - Main.screenPosition, color, new Vector3(0, 0, 0)),
					new VertexBase.CustomVertexInfo(BasePos + new Vector2(125, 172) - Main.screenPosition, color, new Vector3(1, 1, 0)),
					new VertexBase.CustomVertexInfo(BasePos + new Vector2(-95, 172) - Main.screenPosition, color, new Vector3(0, 1, 0))
				};


				Texture2D t = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskDragLine1").Value;
				Main.graphics.GraphicsDevice.Textures[0] = t;
				Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);

				t = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskDragLine2").Value;
				Main.graphics.GraphicsDevice.Textures[0] = t;
				Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			}
		}
		else
		{
			Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskFlesh1").Value, NPC.position - Main.screenPosition + new Vector2(15, 90) + V[9], new Rectangle(0, 0, 220, (int)(312 - V[9].Y * 2f)), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);
		}
		Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/BloodTuskFlesh").Value, NPC.position - Main.screenPosition + new Vector2(15, 90) + V[9], new Rectangle(0, 0, 220, (int)(312 - V[9].Y * 2f)), color, NPC.rotation, new Vector2(110, 156), 1f, SpriteEffects.None, 0f);

		return false;
	}
	public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		/*List<VertexBase.CustomVertexInfo> Vx = new List<VertexBase.CustomVertexInfo>();
            Color color = Color.White;
            for (int x = 0; x < 20; x++)
            {
                if (x < 19)
                {
                    Vx.Add(new VertexBase.CustomVertexInfo(DarkCenter - Main.screenPosition, color, new Vector3(1f, 0.5f, 0)));
                    Vx.Add(new VertexBase.CustomVertexInfo(FogSpace[x] - Main.screenPosition, color, new Vector3(0, 0, 0)));
                    Vx.Add(new VertexBase.CustomVertexInfo(FogSpace[x + 1] - Main.screenPosition, color, new Vector3(0, 1, 0)));
                }
                else
                {
                    Vx.Add(new VertexBase.CustomVertexInfo(DarkCenter - Main.screenPosition, color, new Vector3(1f, 0.5f, 0)));
                    Vx.Add(new VertexBase.CustomVertexInfo(FogSpace[x] - Main.screenPosition, color, new Vector3(0, 0, 0)));
                    Vx.Add(new VertexBase.CustomVertexInfo(FogSpace[0] - Main.screenPosition, color, new Vector3(0, 1, 0)));
                }
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Texture2D t = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/Black").Value;
            Main.graphics.GraphicsDevice.Textures[0] = t;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            base.PostDraw(spriteBatch, screenPos, drawColor);*/
	}

	private Vector2[] hangItem = new Vector2[8];
	private Vector2[] Mouth1 = new Vector2[400];
	private Vector2[] Mouth2 = new Vector2[400];
	private Vector2[] OldMouth1 = new Vector2[400];
	private Vector2[] OldMouth2 = new Vector2[400];
	private Vector2 Mouth1Vel = new Vector2(1, 0);
	private Vector2 Mouth2Vel = new Vector2(-1, 0);
	private float[] HangMaxL1 = new float[400];
	private float[] HangMaxL2 = new float[400];
	public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
	{
		if (tuskHitMove.Length() > 1)
			SprK *= 0.9f;
		SprK = 0.09f + SprK * 0.9f;
		if (NPC.Center - player.Center != Vector2.Zero)
			tuskHitMove += Vector2.Normalize(NPC.Center - player.Center) * Math.Clamp(item.knockBack, 0, 20f);
	}
	public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
	{
		if (tuskHitMove.Length() > 1)
			SprK *= 0.9f;
		SprK = 0.09f + SprK * 0.9f;
		if (projectile.velocity != Vector2.Zero)
			tuskHitMove += Vector2.Normalize(projectile.velocity) * Math.Clamp(projectile.knockBack, 0, 20f);
	}
	private int h = 0;
	public override void ModifyNPCLoot(NPCLoot npcLoot)
	{
		/*大师*/
		//npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsMasterMode(), ModContent.ItemType<Items.Weapons.Legendary.ToothSpear>(), 40/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsMasterMode(), ModContent.ItemType<Items.Weapons.Legendary.ToothBow>(), 8/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		/*Uncomment when needed*///npcLoot.Add(ItemDropRule.ByCondition(new FiveRandomM(), ModContent.ItemType<Items.BossDrop.TuskRelic>(), 1/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
								 //npcLoot.Add(ItemDropRule.ByCondition(new FiveRandomM(), ModContent.ItemType<Items.BossDrop.TuskTreasureBag>(), 1/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));

		/*专家*/
		//npcLoot.Add(ItemDropRule.ByCondition(new OnlyExper(), ModContent.ItemType<Items.Weapons.Legendary.ToothSpear>(), 125/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//npcLoot.Add(ItemDropRule.ByCondition(new OnlyExper(), ModContent.ItemType<Items.Weapons.Legendary.ToothBow>(), 25/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		/*Uncomment when needed*///npcLoot.Add(ItemDropRule.ByCondition(DropBasedOnExpertMode, ModContent.ItemType<Items.BossDrop.TuskTreasureBag>(), 1/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		/*普通*/
		//npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType< Items.Weapons.Legendary.ToothSpear> (), 500/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		//npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<Items.Weapons.Legendary.ToothBow>(), 100/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		/*Uncomment when needed*///npcLoot.Add(ItemDropRule.ByCondition(new FiveRandomN(), ModContent.ItemType<Items.Weapons.SpineGun>(), 6/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		/*Uncomment when needed*///npcLoot.Add(ItemDropRule.ByCondition(new FiveRandomN(), ModContent.ItemType<Items.Weapons.BloodyBoneYoyo>(), 6/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		/*Uncomment when needed*/// npcLoot.Add(ItemDropRule.ByCondition(new FiveRandomN(), ModContent.ItemType<Items.Weapons.ToothMagicBall>(), 6/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		/*Uncomment when needed*/// npcLoot.Add(ItemDropRule.ByCondition(new FiveRandomN(), ModContent.ItemType<Items.Accessories.TuskLace>(), 6/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		/*Uncomment when needed*///npcLoot.Add(ItemDropRule.ByCondition(new FiveRandomN(), ModContent.ItemType<Items.Weapons.ToothStaff>(), 6/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
		/*Uncomment when needed*///npcLoot.Add(ItemDropRule.ByCondition(new FiveRandomN(), ModContent.ItemType<Items.Weapons.ToothKnife>(), 6/*概率分母*/, 1/*最小*/, 1/*最大*/, 1/*概率分子*/));
	}
	public static Vector2 DarkCenter;
	public static Vector2[] FogSpace = new Vector2[20];
	public static bool CheckUpdate = false;
	public static void DrawDarkFog(SpriteBatch sb)
	{
		var color2 = new Color(255, 255, 255, 0);
		Color color = Color.White;
		Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/White").Value, Vector2.Zero, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White, 0, new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f), 5f, SpriteEffects.None, 0f);

		//if (MythMod.TuskFogIndex < 1)
		{
			Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/DarkFog").Value, DarkCenter - Main.screenPosition, null, color, 0, new Vector2(500, 500), 4f, SpriteEffects.None, 0f);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			var Vx = new List<VertexBase.CustomVertexInfo>();
			var Vx2 = new List<VertexBase.CustomVertexInfo>();
			if (!CheckUpdate)
			{
				for (int f = 0; f < 200; f++)
				{
					if (Main.npc[f].type == ModContent.NPCType<TuskWallLeft>() && Main.npc[f].active)
					{
						FogSpace[2] = Main.npc[f].Center + new Vector2(-66, 432);
						FogSpace[3] = Main.npc[f].Center + new Vector2(-127, 260);
						FogSpace[4] = Main.npc[f].Center + new Vector2(-169, 67);
						FogSpace[5] = Main.npc[f].Center + new Vector2(-198, -131);
						FogSpace[6] = Main.npc[f].Center + new Vector2(-203, -313);
						FogSpace[7] = Main.npc[f].Center + new Vector2(-100, -563);

					}
					if (Main.npc[f].type == ModContent.NPCType<TuskWallRight>() && Main.npc[f].active)
					{
						FogSpace[17] = Main.npc[f].Center + new Vector2(66, 432);
						FogSpace[16] = Main.npc[f].Center + new Vector2(127, 260);
						FogSpace[15] = Main.npc[f].Center + new Vector2(169, 67);
						FogSpace[14] = Main.npc[f].Center + new Vector2(198, -131);
						FogSpace[13] = Main.npc[f].Center + new Vector2(203, -313);
						FogSpace[12] = Main.npc[f].Center + new Vector2(100, -563);
					}

				}
				CheckUpdate = true;
			}

			FogSpace[8] = DarkCenter + new Vector2((FogSpace[7].X - DarkCenter.X) * 2 / 3f, FogSpace[7].Y - 10 - DarkCenter.Y);
			FogSpace[9] = DarkCenter + new Vector2((FogSpace[7].X - DarkCenter.X) * 1 / 3f, FogSpace[7].Y - 12 - DarkCenter.Y);
			FogSpace[1] = DarkCenter + new Vector2((FogSpace[2].X - DarkCenter.X) * 2 / 3f, 160);
			FogSpace[0] = DarkCenter + new Vector2((FogSpace[2].X - DarkCenter.X) * 1 / 3f, 160);


			FogSpace[11] = DarkCenter + new Vector2((FogSpace[12].X - DarkCenter.X) * 2 / 3f, FogSpace[12].Y - 10 - DarkCenter.Y);
			FogSpace[10] = DarkCenter + new Vector2((FogSpace[12].X - DarkCenter.X) * 1 / 3f, FogSpace[12].Y - 12 - DarkCenter.Y);
			FogSpace[18] = DarkCenter + new Vector2((FogSpace[17].X - DarkCenter.X) * 2 / 3f, 160);
			FogSpace[19] = DarkCenter + new Vector2((FogSpace[17].X - DarkCenter.X) * 1 / 3f, 160);
			for (int x = 0; x < 20; x++)
			{
				if (x < 19)
				{
					Vx.Add(new VertexBase.CustomVertexInfo(DarkCenter - Main.screenPosition, color, new Vector3(1f, 0.5f, 0)));
					Vx.Add(new VertexBase.CustomVertexInfo(FogSpace[x] - Main.screenPosition, color, new Vector3(0, 0, 0)));
					Vx.Add(new VertexBase.CustomVertexInfo(FogSpace[x + 1] - Main.screenPosition, color, new Vector3(0, 1, 0)));
				}
				else
				{
					Vx.Add(new VertexBase.CustomVertexInfo(DarkCenter - Main.screenPosition, color, new Vector3(1f, 0.5f, 0)));
					Vx.Add(new VertexBase.CustomVertexInfo(FogSpace[x] - Main.screenPosition, color, new Vector3(0, 0, 0)));
					Vx.Add(new VertexBase.CustomVertexInfo(FogSpace[0] - Main.screenPosition, color, new Vector3(0, 1, 0)));
				}
				if (x < 19)
				{
					Vx2.Add(new VertexBase.CustomVertexInfo(DarkCenter - Main.screenPosition, color2, new Vector3(1f, 0.5f, 0)));
					Vx2.Add(new VertexBase.CustomVertexInfo(FogSpace[x] - Main.screenPosition, color2, new Vector3(0, 0, 0)));
					Vx2.Add(new VertexBase.CustomVertexInfo(FogSpace[x + 1] - Main.screenPosition, color2, new Vector3(0, 1, 0)));
				}
				else
				{
					Vx2.Add(new VertexBase.CustomVertexInfo(DarkCenter - Main.screenPosition, color2, new Vector3(1f, 0.5f, 0)));
					Vx2.Add(new VertexBase.CustomVertexInfo(FogSpace[x] - Main.screenPosition, color2, new Vector3(0, 0, 0)));
					Vx2.Add(new VertexBase.CustomVertexInfo(FogSpace[0] - Main.screenPosition, color2, new Vector3(0, 1, 0)));
				}
			}

			Texture2D t = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/WhiteBlack2").Value;
			Main.graphics.GraphicsDevice.Textures[0] = t;
			Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/PerlinFog").Value;
			Effect PurpleFog = ModContent.Request<Effect>("Everglow/Myth/Effects/PurpleFog").Value;
			PurpleFog.Parameters["m"].SetValue((float)Main.time * -0.001f);
			PurpleFog.CurrentTechnique.Passes[0].Apply();
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			t = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/WhiteBlack").Value;
			Main.graphics.GraphicsDevice.Textures[0] = t;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx2.ToArray(), 0, Vx2.Count / 3);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		}
		Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/Sight").Value, Main.LocalPlayer.Center - Main.screenPosition, null, color2, 0, new Vector2(500, 500), 0.75f, SpriteEffects.None, 0f);

		/*Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Effect PurpleFog2 = ModContent.Request<Effect>("Everglow/Myth/Effects/DarkFog").Value;
            if(Killing > 0)
            {
                Minr = 1 - Killing / 180f;
            }
            PurpleFog2.Parameters["minr"].SetValue(Minr);
            PurpleFog2.CurrentTechnique.Passes[0].Apply();
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/NPCs/Bosses/BloodTusk/PurpleFog").Value, Vector2.Zero, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), color2, 0, new Vector2(1024, 1024), 3f, SpriteEffects.None, 0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);*/
	}
	public static float Minr = 0;
}
