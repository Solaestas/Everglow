using Everglow.Myth;
using Everglow.Myth.Common;
using Everglow.Myth.LanternMoon.Projectiles.DashCore;
using Terraria.Audio;
using Terraria.Localization;

namespace Everglow.Myth.LanternMoon.NPCs.FlamingDashCore
{
	[AutoloadBossHead]
	public class FlamingDashCore : ModNPC
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Spiritual Fiery Core");
			DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "花火幻魂心");
		}
		public override void SetDefaults()
		{
			NPC.damage = 180;
			NPC.lifeMax = 1560000;
			NPC.width = 40;
			NPC.height = 40;
			NPC.defense = 0;
			NPC.value = 200000;
			NPC.aiStyle = -1;
			NPC.boss = true;
			NPC.knockBackResist = 0f;
			NPC.dontTakeDamage = false;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.HitSound = SoundID.NPCHit3;
			NPCID.Sets.TrailingMode[NPC.type] = 0;
			NPCID.Sets.TrailCacheLength[NPC.type] = 60;
		}
		public static bool CheckNewBoss = true;
		public static int PauseCool = 120;
		public static int Shine = 0;
		public static Color ColorShine = new Color(0, 0, 0, 0);
		Vector2 Pos960;
		int Freq = 36;
		public override void AI()
		{
			float Str = 1;
			NPC.TargetClosest(false);
			Player player = Main.player[NPC.target];
			NPC.localAI[0] += 1;
			if (NPC.localAI[0] <= 15)
				Sca = NPC.localAI[0] / 15f;
			else
			{
				Sca = 1;
			}
			NPC.color.R = (byte)(NPC.color.R * 0.94f + Aimcolor.R * 0.06f);
			NPC.color.G = (byte)(NPC.color.G * 0.94f + Aimcolor.G * 0.06f);
			NPC.color.B = (byte)(NPC.color.B * 0.94f + Aimcolor.B * 0.06f);
			NPC.color.A = (byte)(NPC.color.A * 0.94f + Aimcolor.A * 0.06f);
			NPCOldColor[0] = NPC.color;
			for (int f = NPCOldColor.Length - 1; f > 0; f--)
			{
				NPCOldColor[f] = NPCOldColor[f - 1];
			}
			Vector2 Vadd = NPC.velocity - NPC.oldVelocity;
			float theta = (Vadd.X * NPC.velocity.X + Vadd.Y * NPC.velocity.Y) / (Vadd.Length() * NPC.velocity.Length()) + 1;
			NPCOldWidth[0] = 20 * theta;
			for (int f = NPCOldWidth.Length - 1; f > 0; f--)
			{
				NPCOldWidth[f] = NPCOldWidth[f - 1];
			}
			if (Shine > 0)
				Shine -= 1;
			else
			{
				Shine = 0;
			}
			if (NPC.localAI[0] <= 150)
			{
				Aimcolor = Color.Red;
				if (NPC.localAI[0] == 100)
					NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<RedCore>());
			}//教程
			if (NPC.localAI[0] > 180 && NPC.localAI[0] <= 360)
			{
				Aimcolor = new Color(0, 255, 17);
				if (NPC.localAI[0] == 250)
					NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<GreenCore>());
			}//教程
			if (NPC.localAI[0] > 360 && NPC.localAI[0] <= 540)
				Aimcolor = new Color(129, 4, 224);
			if (NPC.localAI[0] > 540 && NPC.localAI[0] <= 690)
				Aimcolor = new Color(255, 204, 0);
			if (NPC.localAI[0] <= 690)
			{
				Vector2 v = player.Center + new Vector2((float)Math.Sin(NPC.localAI[0] / 40f) * 500f, (float)Math.Sin((NPC.localAI[0] + 200) / 40f) * 50f - 150) - NPC.Center;
				if (NPC.velocity.Length() < 9f)
					NPC.velocity += Vector2.Normalize(v) * 0.35f;
				NPC.velocity *= 0.96f;
			}//教程

			if (NPC.localAI[0] > 690 && NPC.localAI[0] <= 920)
			{
				if (NPC.localAI[0] <= 880)
				{
					if (Math.Abs(60 - NPC.localAI[0] % 60) < 15)
						Aimcolor = new Color(0, 255, 17);
					else
					{
						Aimcolor = Color.Red;
					}
				}
				else
				{
					Aimcolor = new Color(255, 157, 0);
				}
				if (NPC.localAI[0] % 60 == 42)
					AimPos = new Vector2(0, -400).RotatedByRandom(6.283);
				//颜色
				Vector2 v0 = player.Center + AimPos - NPC.Center;
				Vector2 v1 = player.Center + AimPos - NPC.Center + Vector2.Normalize(v0) * 60;
				var v2 = Vector2.Normalize(v1);
				if (NPC.velocity.Length() < 129f)
					NPC.velocity += v2;
				NPC.velocity *= 0.95f;
				//动作
				if (NPC.localAI[0] == 700)
				{
					Shine = 3;
					ColorShine = NPC.color;
					//MythContentPlayer mplayer = Terraria.Main.player[Terraria.Main.myPlayer].GetModPlayer<MythContentPlayer>();
					//mplayer.Shake = 3;
					ScreenShaker mplayer = Main.player[Main.myPlayer].GetModPlayer<ScreenShaker>();
					mplayer.FlyCamPosition = new Vector2(0, 56).RotatedByRandom(6.283);

					if ((player.Center - NPC.Center).Length() > 100)
						Str = 100 / (player.Center - NPC.Center).Length();
					mplayer.DirFlyCamPosStrength = Str; //Using Direct FlyCamPosition because FlyCamPosition itself being used causes errors (see ScreenShaker ModPlayer) ~Setnour6
					SoundEngine.PlaySound(SoundID.Item36, NPC.Center);//特效
					Vector2 vn = new Vector2(0, -20).RotatedBy(NPC.localAI[0] / 90d);
					for (int h = 0; h < 6; h++)
					{
						Vector2 vm = vn.RotatedBy(h / 3d * Math.PI);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vm, ModContent.ProjectileType<RedFlame0>(), Dam, 0f, player.whoAmI, 0, 0);
					}
					for (int h = 0; h < 6; h++)
					{
						Vector2 vm = vn.RotatedBy(h / 3d * Math.PI + 0.15);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vm * 0.7f, ModContent.ProjectileType<GreenFlame0Little>(), Dam, 0f, player.whoAmI, 0, 0);
						vm = vn.RotatedBy(h / 3d * Math.PI - 0.15);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vm * 0.7f, ModContent.ProjectileType<GreenFlame0Little>(), Dam, 0f, player.whoAmI, 0, 0);
						vm = vn.RotatedBy(h / 3d * Math.PI + 0.25);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vm * 0.6f, ModContent.ProjectileType<GreenFlame0Little>(), Dam, 0f, player.whoAmI, 0, 0);
						vm = vn.RotatedBy(h / 3d * Math.PI - 0.25);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vm * 0.6f, ModContent.ProjectileType<GreenFlame0Little>(), Dam, 0f, player.whoAmI, 0, 0);
					}
				}
				if (NPC.localAI[0] == 760)
				{
					Shine = 3;
					ColorShine = new Color(255, 60, 0);
					//MythContentPlayer mplayer = Terraria.Main.player[Terraria.Main.myPlayer].GetModPlayer<MythContentPlayer>();
					//mplayer.Shake = 3;
					ScreenShaker mplayer = Main.player[Main.myPlayer].GetModPlayer<ScreenShaker>();
					mplayer.FlyCamPosition = new Vector2(0, 56).RotatedByRandom(6.283);
					if ((player.Center - NPC.Center).Length() > 100)
						Str = 100 / (player.Center - NPC.Center).Length();
					mplayer.DirFlyCamPosStrength = Str;
					SoundEngine.PlaySound(SoundID.Item36, NPC.Center);//特效
					Vector2 vn = new Vector2(0, -20).RotatedBy(NPC.localAI[0] / 90d);
					for (int h = 0; h < 6; h++)
					{
						Vector2 vm = vn.RotatedBy(h / 3d * Math.PI);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vm, ModContent.ProjectileType<RedFlame0Split>(), Dam, 0f, player.whoAmI, 0, 0);
					}
					for (int h = 0; h < 6; h++)
					{
						Vector2 vm = vn.RotatedBy(h / 3d * Math.PI + 0.15);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vm * 0.7f, ModContent.ProjectileType<RedFlame0Little>(), Dam, 0f, player.whoAmI, 0, 0);
						vm = vn.RotatedBy(h / 3d * Math.PI - 0.15);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vm * 0.7f, ModContent.ProjectileType<GreenFlame0Little>(), Dam, 0f, player.whoAmI, 0, 0);
						vm = vn.RotatedBy(h / 3d * Math.PI + 0.25);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vm * 0.6f, ModContent.ProjectileType<RedFlame0Little>(), Dam, 0f, player.whoAmI, 0, 0);
						vm = vn.RotatedBy(h / 3d * Math.PI - 0.25);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vm * 0.6f, ModContent.ProjectileType<GreenFlame0Little>(), Dam, 0f, player.whoAmI, 0, 0);
					}
				}
				if (NPC.localAI[0] == 820)
				{
					Shine = 3;
					ColorShine = new Color(255, 60, 0);
					//MythContentPlayer mplayer = Terraria.Main.player[Terraria.Main.myPlayer].GetModPlayer<MythContentPlayer>();
					//mplayer.Shake = 3;
					ScreenShaker mplayer = Main.player[Main.myPlayer].GetModPlayer<ScreenShaker>();
					mplayer.FlyCamPosition = new Vector2(0, 56).RotatedByRandom(6.283);
					if ((player.Center - NPC.Center).Length() > 100)
						Str = 100 / (player.Center - NPC.Center).Length();
					mplayer.DirFlyCamPosStrength = Str;
					SoundEngine.PlaySound(SoundID.Item36, NPC.Center);//特效
					Vector2 vn = new Vector2(0, -14).RotatedBy(NPC.localAI[0] / 90d);
					for (int h = 0; h < 3; h++)
					{
						Vector2 vm = vn.RotatedBy(h / 1.5d * Math.PI);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vm, ModContent.ProjectileType<RedFlame1Split>(), Dam, 0f, player.whoAmI, 0, 0);
					}
					for (int h = 0; h < 3; h++)
					{
						Vector2 vm = vn.RotatedBy((h + 0.5) / 1.5d * Math.PI);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vm, ModContent.ProjectileType<GreenFlame1Split>(), Dam, 0f, player.whoAmI, 0, 0);
					}
				}
				if (NPC.localAI[0] == 880)
				{
					Shine = 3;
					ColorShine = new Color(255, 60, 0);
					//MythContentPlayer mplayer = Terraria.Main.player[Terraria.Main.myPlayer].GetModPlayer<MythContentPlayer>();
					//mplayer.Shake = 3;
					ScreenShaker mplayer = Main.player[Main.myPlayer].GetModPlayer<ScreenShaker>();
					mplayer.FlyCamPosition = new Vector2(0, 56).RotatedByRandom(6.283);
					if ((player.Center - NPC.Center).Length() > 100)
						Str = 100 / (player.Center - NPC.Center).Length();
					mplayer.DirFlyCamPosStrength = Str;
					SoundEngine.PlaySound(SoundID.Item36, NPC.Center);//特效
					Vector2 vn = new Vector2(0, -17).RotatedBy(NPC.localAI[0] / 90d);
					for (int h = 0; h < 12; h++)
					{
						Vector2 vm = vn.RotatedBy(h / 12d * Math.PI);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vm, ModContent.ProjectileType<GreenFlame0Little>(), Dam, 0f, player.whoAmI, 0, 0);
					}
					for (int h = 12; h < 24; h++)
					{
						Vector2 vm = vn.RotatedBy(h / 12d * Math.PI);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vm, ModContent.ProjectileType<RedFlame0Little>(), Dam, 0f, player.whoAmI, 0, 0);
					}
				}
				if (NPC.localAI[0] == 894)
				{
					Shine = 3;
					ColorShine = NPC.color;
					//MythContentPlayer mplayer = Terraria.Main.player[Terraria.Main.myPlayer].GetModPlayer<MythContentPlayer>();
					//mplayer.Shake = 3;
					ScreenShaker mplayer = Main.player[Main.myPlayer].GetModPlayer<ScreenShaker>();
					mplayer.FlyCamPosition = new Vector2(0, 56).RotatedByRandom(6.283);
					if ((player.Center - NPC.Center).Length() > 100)
						Str = 100 / (player.Center - NPC.Center).Length();
					mplayer.DirFlyCamPosStrength = Str;//特效
					SoundEngine.PlaySound(SoundID.Item36, NPC.Center);
					for (int h = 0; h < 36; h++)
					{
						Vector2 vm = new Vector2(0, Main.rand.NextFloat(Main.rand.NextFloat(0f, 14f), 24f)).RotatedByRandom(6.283);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vm, ModContent.ProjectileType<YellowFlame0>(), Dam, 0f, player.whoAmI, 0, 0);
						vm = new Vector2(0, Main.rand.NextFloat(Main.rand.NextFloat(0f, 14f), 24f)).RotatedByRandom(6.283);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vm, ModContent.ProjectileType<BrownFlame0>(), Dam, 0f, player.whoAmI, 0, 0);
					}
				}
				//弹幕
			}//红绿花火
			if (NPC.localAI[0] > 920 && NPC.localAI[0] <= 1880)
			{
				/*if (NPC.localAI[0] <= 880)
                {
                    if (Math.Abs(60 - NPC.localAI[0] % 60) < 15)
                    {
                        Aimcolor = new Color(0, 255, 17);
                    }
                    else
                    {
                        Aimcolor = Color.Red;
                    }
                }
                else
                {
                    Aimcolor = new Color(255, 157, 0);
                }*/
				if (NPC.localAI[0] <= 960)
					AimPos = new Vector2(0, -400);
				else
				{
					double kd = (NPC.localAI[0] - 960) / 120d;
					AimPos = new Vector2(0, (float)(-270 + Math.Sin(kd * 2) * 50)).RotatedBy(kd * kd);
				}
				//颜色
				Vector2 v0 = player.Center + AimPos - NPC.Center;
				Vector2 v1 = player.Center + AimPos - NPC.Center - Vector2.Normalize(v0) * 60;
				Vector2 v2 = v1 / 40f;
				if (NPC.velocity.Length() < 729f)
					NPC.velocity += v2;
				NPC.velocity *= 0.95f;
				//动作
				if (NPC.localAI[0] >= 930)
				{
					/*Shine = 3;
                    ColorShine = Color.White;
                    MythContentPlayer mplayer = Terraria.Main.player[Terraria.Main.myPlayer].GetModPlayer<MythContentPlayer>();
                    mplayer.Shake = 3;
                    float Str = 1;
                    if ((player.Center - NPC.Center).Length() > 100)
                    {
                        Str = 100 / (player.Center - NPC.Center).Length();
                    }
                    mplayer.ShakeStrength = Str;
                    SoundEngine.PlaySound(SoundID.Item36, NPC.Center);//特效*/
					double kd = (NPC.localAI[0] - 960) / 60d;
					Vector2 vn = new Vector2(0, (float)(-kd * 260)).RotatedBy(kd * kd + Math.Sin(NPC.localAI[0] / 20d) * 5);
					if (NPC.localAI[0] == 960)
						Pos960 = NPC.Center;
					if (NPC.localAI[0] % 8 == 2 && NPC.localAI[0] > 960 && NPC.localAI[0] < 1400)
					{
						for (int h = 0; h < 6; h++)
						{
							Vector2 vm = vn.RotatedBy(h / 3d * Math.PI);
							Projectile.NewProjectile(NPC.GetSource_FromAI(), Pos960 + vm, Vector2.Zero, ModContent.ProjectileType<BlueFlame1Boom>(), Dam, 0f, player.whoAmI, (float)(h / 3d * Math.PI + NPC.localAI[0] / 35d), 0);
						}
					}
					if (NPC.localAI[0] == 934)
					{
						for (int h = 0; h < 4; h++)
						{
							Vector2 vm = vn.RotatedBy(h / 2d * Math.PI);
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vm, ModContent.ProjectileType<RedFlame2Split>(), Dam, 0f, player.whoAmI, 0, 0);
						}
					}
					if (NPC.localAI[0] == 934)
						Freq = 24;
					if (NPC.localAI[0] >= 1200 && NPC.localAI[0] % 7 == 0)
					{
						if (Freq > 2)
							Freq--;
					}
					if (NPC.localAI[0] >= 1200 && NPC.localAI[0] % Freq == 0)
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity / Freq, ModContent.ProjectileType<YellowToRedFlame0Little>(), Dam, 0f, player.whoAmI, 0, 0);
					if (NPC.localAI[0] == 1200 && player.ownedProjectileCounts[ModContent.ProjectileType<GoldDashLine>()] < 2)
					{
						Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center + new Vector2(0, 350), new Vector2(8, 0), ModContent.ProjectileType<GoldDashLine>(), Dam, 0f, player.whoAmI, 0, 0);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center + new Vector2(0, -350), new Vector2(-8, 0), ModContent.ProjectileType<GoldDashLine>(), Dam, 0f, player.whoAmI, 0, 0);
					}
				}
				if (EverglowConfig.DebugMode)
					Main.NewText(Main.MouseWorld.ToString(), 255, 0, 0);
			}//红绿花火
			if (NPC.localAI[0] > 1820 && NPC.localAI[0] <= 2560)//蓝黄风车
			{
				if (Math.Abs(60 - NPC.localAI[0] % 60) < 15)
					Aimcolor = new Color(0, 131, 255);
				else
				{
					Aimcolor = new Color(255, 204, 0);
				}
				//颜色
				if (NPC.localAI[0] < 1500)
				{
					if (NPC.localAI[0] % 100 == 2)
						AimPos = new Vector2(0, -400);
					Vector2 v0 = player.Center + AimPos - NPC.Center;
					Vector2 v1 = player.Center + AimPos - NPC.Center + Vector2.Normalize(v0) * 60;
					var v2 = Vector2.Normalize(v1);
					if (NPC.velocity.Length() < 129f)
						NPC.velocity += v2;
					NPC.velocity *= 0.95f;
				}
				else
				{
					NPC.velocity *= 0.92f;
				}
				//动作
				if (NPC.localAI[0] == 1940)
				{
					Vector2 vn = new Vector2(0, -6).RotatedBy(NPC.localAI[0] / 90d);
					for (int h = 0; h < 6; h++)
					{
						Vector2 vm = vn.RotatedBy(h / 3d * Math.PI);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center + new Vector2(-100, 0), vm, ModContent.ProjectileType<YellowFlame0Little>(), Dam, 0f, player.whoAmI, 0, 0);
					}
					for (int h = 0; h < 6; h++)
					{
						Vector2 vm = vn.RotatedBy(h / 3d * Math.PI);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center + new Vector2(100, 0), vm, ModContent.ProjectileType<BlueFlame0Little>(), Dam, 0f, player.whoAmI, 0, 0);
					}
					NPC.NewNPC(null, (int)(player.Center.X + 100), (int)player.Center.Y, ModContent.NPCType<BlueCore>());
					NPC.NewNPC(null, (int)(player.Center.X - 100), (int)player.Center.Y, ModContent.NPCType<YellowCore>());
				}
				if (NPC.localAI[0] > 1990)
				{
					if (NPC.localAI[0] % 5 == 0)
					{
						double FireRot = (NPC.localAI[0] - 1990) / 2d;
						double TFireRot = Math.Pow(1.01d, FireRot);
						Vector2 vn = new Vector2(0, -12).RotatedBy(TFireRot);
						Vector2 vn2 = new Vector2(0, -16).RotatedBy(-TFireRot);
						for (int h = 0; h < 5; h++)
						{
							Vector2 vm = vn.RotatedBy(h / 2.5d * Math.PI);
							int f = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vm, ModContent.ProjectileType<YellowFlame0Little>(), Dam, 0f, player.whoAmI, 15, 0);
							Main.projectile[f].timeLeft = 100;
						}
						for (int h = 0; h < 5; h++)
						{
							Vector2 vm = vn2.RotatedBy(h / 2.5d * Math.PI);
							int f = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vm, ModContent.ProjectileType<BlueFlame0Little>(), Dam, 0f, player.whoAmI, 15, 0);
							Main.projectile[f].timeLeft = 100;
						}
					}
					if (Main.masterMode)
					{
						if (NPC.localAI[0] % 20 == 0)
						{
							double FireRot = (NPC.localAI[0] - 1990) / 2d;
							double TFireRot = Math.Pow(1.01d, FireRot);
							Vector2 vn = new Vector2(0, -16).RotatedBy(TFireRot);
							Vector2 vn2 = new Vector2(0, -16).RotatedBy(-TFireRot);
							for (int h = 0; h < 5; h++)
							{
								Vector2 vm = vn.RotatedBy(h / 2.5d * Math.PI);
								Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vm, ModContent.ProjectileType<YellowFlame0>(), Dam, 0f, player.whoAmI, 15, 0);
							}
							for (int h = 0; h < 5; h++)
							{
								Vector2 vm = vn2.RotatedBy(h / 2.5d * Math.PI);
								Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vm, ModContent.ProjectileType<BlueFlame0>(), Dam, 0f, player.whoAmI, 15, 0);
							}
						}
					}
				}
				//弹幕
			}//蓝黄花火
			if (NPC.localAI[0] > 2560 && NPC.localAI[0] <= 2860)
			{
				if (Math.Abs(60 - NPC.localAI[0] % 60) < 15)
				{
					if (NPC.localAI[0] >= 2550 && NPC.localAI[0] <= 2610)
						Aimcolor = new Color(224, 3, 0);
					if (NPC.localAI[0] >= 2610 && NPC.localAI[0] <= 2670)
						Aimcolor = new Color(129, 4, 224);
					if (NPC.localAI[0] >= 2670 && NPC.localAI[0] <= 2730)
						Aimcolor = new Color(196, 125, 129);
					if (NPC.localAI[0] >= 2730 && NPC.localAI[0] <= 2790)
						Aimcolor = new Color(255, 204, 0);
					if (NPC.localAI[0] >= 2790 && NPC.localAI[0] <= 2850)
						Aimcolor = new Color(225, 186, 96);
				}
				else
				{
					Aimcolor = Color.Red;
				}
				if (NPC.localAI[0] % 60 == 2)
					AimPos = new Vector2(0, -500).RotatedByRandom(6.283);
				//颜色
				Vector2 v0 = player.Center + AimPos - NPC.Center;
				Vector2 v1 = player.Center + AimPos - NPC.Center + Vector2.Normalize(v0) * 60;
				var v2 = Vector2.Normalize(v1);
				if (NPC.velocity.Length() < 129f)
					NPC.velocity += v2 * 2;
				NPC.velocity *= 0.95f;
				//动作
				if (NPC.localAI[0] % 60 == 0)
				{
					Shine = 3;
					ColorShine = NPC.color;
					//MythContentPlayer mplayer = Terraria.Main.player[Terraria.Main.myPlayer].GetModPlayer<MythContentPlayer>();
					//mplayer.Shake = 3;
					ScreenShaker mplayer = Main.player[Main.myPlayer].GetModPlayer<ScreenShaker>();
					mplayer.FlyCamPosition = new Vector2(0, 56).RotatedByRandom(6.283);
					if ((player.Center - NPC.Center).Length() > 100)
						Str = 100 / (player.Center - NPC.Center).Length();
					mplayer.DirFlyCamPosStrength = Str;
					SoundEngine.PlaySound(SoundID.Item36, NPC.Center);//特效
					if (NPC.localAI[0] >= 2550 && NPC.localAI[0] <= 2610)
					{
						for (int h = 0; h < 72; h++)
						{
							Vector2 vm = new Vector2(0, Main.rand.NextFloat(Main.rand.NextFloat(0f, 14f), 24f)).RotatedByRandom(6.283);
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vm, ModContent.ProjectileType<BrownFlame0>(), Dam, 0f, player.whoAmI, 0, 0);
						}
						for (int h = 0; h < 24; h++)
						{
							Vector2 vm = new Vector2(0, Main.rand.NextFloat(Main.rand.NextFloat(0f, 7f), 12f)).RotatedByRandom(6.283);
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vm, ModContent.ProjectileType<RedFlame0Little>(), Dam, 0f, player.whoAmI, 0, 0);
						}
					}
					if (NPC.localAI[0] >= 2610 && NPC.localAI[0] <= 2670)
					{
						for (int h = 0; h < 72; h++)
						{
							Vector2 vm = new Vector2(0, Main.rand.NextFloat(Main.rand.NextFloat(0f, 14f), 24f)).RotatedByRandom(6.283);
							int d = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vm, ModContent.ProjectileType<PurpleGreenFlame0>(), Dam, 0f, player.whoAmI, 0, 0);
							Main.projectile[d].timeLeft = Main.rand.Next(170, 190);
						}
					}
					if (NPC.localAI[0] >= 2670 && NPC.localAI[0] <= 2730)
					{
						float SqY = Main.rand.NextFloat(0.3f, 1f);
						float RandomA = Main.rand.NextFloat(0, 6.283f);
						for (int h = 0; h < 30; h++)
						{
							float Fx = (float)(Math.Sin(h / 30d * Math.PI) * (1 - SqY) + 0.5f);
							Vector2 vm = new Vector2(0, 30).RotatedBy(h / 15d * Math.PI);
							Vector2 vn = (vm * SqY).RotatedBy(RandomA);
							int f = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vn, ModContent.ProjectileType<GreenFlame0>(), Dam, 0f, player.whoAmI, 0, 0);
							Main.projectile[f].scale = Fx;
						}
						for (int h = 0; h < 24; h++)
						{
							Vector2 vm = new Vector2(0, Main.rand.NextFloat(Main.rand.NextFloat(0f, 7f), 12f)).RotatedByRandom(6.283);
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vm, ModContent.ProjectileType<PinkFlame0>(), Dam, 0f, player.whoAmI, 0, 0);
						}
					}
					if (NPC.localAI[0] >= 2730 && NPC.localAI[0] <= 2790)
					{
						for (int h = 0; h < 72; h++)
						{
							Vector2 vm = new Vector2(0, Main.rand.NextFloat(Main.rand.NextFloat(0f, 14f), 24f)).RotatedByRandom(6.283);
							int d = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vm, ModContent.ProjectileType<YellowFlame0Shine>(), Dam, 0f, player.whoAmI, 0, 0);
							Main.projectile[d].timeLeft = Main.rand.Next(170, 190);
						}
					}
					if (NPC.localAI[0] >= 2790 && NPC.localAI[0] <= 2850)
					{
						for (int h = 0; h < 8; h++)
						{
							Vector2 vm = new Vector2(0, Main.rand.NextFloat(Main.rand.NextFloat(0f, 14f), 24f)).RotatedByRandom(6.283);
							int d = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vm, ModContent.ProjectileType<GreenFlame0Split>(), Dam, 0f, player.whoAmI, 0, 0);
							Main.projectile[d].timeLeft = Main.rand.Next(170, 190);
						}
						for (int h = 0; h < 8; h++)
						{
							Vector2 vm = new Vector2(0, Main.rand.NextFloat(Main.rand.NextFloat(0f, 14f), 24f)).RotatedByRandom(6.283);
							int d = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vm, ModContent.ProjectileType<RedFlame0Split>(), Dam, 0f, player.whoAmI, 0, 0);
							Main.projectile[d].timeLeft = Main.rand.Next(170, 190);
						}
					}
				}
				//弹幕
			}//五连炸
			if (NPC.localAI[0] > 2860 && NPC.localAI[0] <= 3700)//蓝漩涡
			{
				Aimcolor = new Color(0, 0, 255);
				//颜色
				if (NPC.localAI[0] < 2900)
				{
					if (NPC.localAI[0] % 100 == 2)
						AimPos = new Vector2(-200, -300);
					Vector2 v0 = player.Center + AimPos - NPC.Center;
					Vector2 v1 = player.Center + AimPos - NPC.Center + Vector2.Normalize(v0) * 60;
					var v2 = Vector2.Normalize(v1);
					if (NPC.velocity.Length() < 129f)
						NPC.velocity += v2;
					NPC.velocity *= 0.95f;
				}
				else
				{
					NPC.velocity *= 0.92f;
				}
				//动作
				if (NPC.localAI[0] >= 2940 && NPC.localAI[0] < 3040)
				{
					Vector2 vm = new Vector2(0, Main.rand.NextFloat(Main.rand.NextFloat(150, 1000f), 1001f)).RotatedByRandom(6.283);
					float VLength = vm.Length() - 149.99f;
					int d = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + vm, vm.RotatedBy(Math.PI * 1.5) / 10000000f * VLength, ModContent.ProjectileType<BlueStarTrail>(), Dam, 0f, player.whoAmI, 0, 0);
					Main.projectile[d].timeLeft = Main.rand.Next(536, 618);
					for (int f = 0; f < Main.projectile.Length; f++)
					{
						if (Main.projectile[f].type == ModContent.ProjectileType<BlueStarTrail>())
						{
							Main.projectile[f].velocity *= 0.97f;
							Vector2 v0 = NPC.Center - Main.projectile[f].Center;
							float VL0 = 150f / (v0.Length() + 150f);
							Main.projectile[f].position += Vector2.Normalize(v0).RotatedBy(VL0 * Math.PI) * (VL0 + 1) * SwirlSpeed;
						}
					}
					SwirlSpeed += 0.01f;
				}
				if (NPC.localAI[0] >= 3040 && NPC.localAI[0] < 3200)
				{
					Vector2 vm = new Vector2(0, Main.rand.NextFloat(Main.rand.NextFloat(150, 1000f), 1001f)).RotatedByRandom(6.283);
					float VLength = vm.Length() - 149.99f;
					int d = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + vm, vm.RotatedBy(Math.PI * 1.5) / 10000000f * VLength, ModContent.ProjectileType<BlueStarTrail>(), Dam, 0f, player.whoAmI, 0, 0);
					Main.projectile[d].timeLeft = Main.rand.Next(536, 618);
					for (int f = 0; f < Main.projectile.Length; f++)
					{
						if (Main.projectile[f].type == ModContent.ProjectileType<BlueStarTrail>())
						{
							Main.projectile[f].velocity *= 0.97f;
							Vector2 v0 = NPC.Center - Main.projectile[f].Center;
							if (v0.Length() < 200)
							{
								if (Main.projectile[f].timeLeft > 90)
									Main.projectile[f].timeLeft = 80;
							}
							float VL0 = 150f / (v0.Length() + 150f);
							Main.projectile[f].position += Vector2.Normalize(v0).RotatedBy(VL0 * Math.PI) * (VL0 + 1) * SwirlSpeed;
						}
					}
					if (SwirlSpeed < 8)
						SwirlSpeed += 0.15f;
					else
					{
						SwirlSpeed = 8f;
					}
				}
				if (NPC.localAI[0] == 3200)
				{
					for (int f = 0; f < Main.projectile.Length; f++)
					{
						if (Main.projectile[f].type == ModContent.ProjectileType<BlueStarTrail>())
							Main.projectile[f].timeLeft = Main.rand.Next(46, 62);
					}
				}
				if (NPC.localAI[0] >= 3200)
				{
					for (int f = 0; f < Main.projectile.Length; f++)
					{
						if (Main.projectile[f].type == ModContent.ProjectileType<BlueStarTrail>())
							Main.projectile[f].timeLeft = Main.rand.Next(46, 62);
					}
					for (int f = 0; f < Main.projectile.Length; f++)
					{
						if (Main.projectile[f].type == ModContent.ProjectileType<BlueStarTrail>())
						{
							Main.projectile[f].velocity *= 0.97f;
							Vector2 v0 = NPC.Center - Main.projectile[f].Center;
							float VL0 = 150f / (v0.Length() + 150f);
							Main.projectile[f].position += Vector2.Normalize(v0).RotatedBy(VL0 * Math.PI) * (VL0 + 1) * SwirlSpeed;
						}
					}
					SwirlSpeed *= 0.96f;
				}
				//弹幕
			}//蓝黄花火
			if (NPC.localAI[0] > 3800)//清零
				NPC.localAI[0] = 600;

		}
		float SwirlSpeed = 0;
		int Dam = 110;
		Vector2 AimPos = Vector2.Zero;
		Color Aimcolor = new Color(0, 0, 0, 0);
		Color[] NPCOldColor = new Color[70];
		float[] NPCOldWidth = new float[70];
		float oldH = 0;

		private Effect ef2;
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			var bars = new List<DashCoreVertexInfo>();
			ef2 = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/TrailRainbow").Value;
			// 把所有的点都生成出来，按照顺序

			for (int i = 1; i < NPC.oldPos.Length; ++i)
			{
				float width = NPCOldWidth[i];
				if (Tokill > 0)
				{
					if (Tokill < 60)
						width = NPCOldWidth[i] * Tokill / 60f;
				}
				if (NPC.oldPos[i] == Vector2.Zero)
					break;
				//spriteBatch.Draw(Main.magicPixel, NPC.oldPos[i] - Main.screenPosition,
				//    new Rectangle(0, 0, 1, 1), Color.White, 0f, new Vector2(0.5f, 0.5f), 5f, SpriteEffects.None, 0f);


				var normalDir = NPC.oldPos[i - 1] - NPC.oldPos[i];
				normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

				var factor = i / (float)NPC.oldPos.Length;
				var color = Color.Lerp(Color.White, Color.Red, factor);
				var w = MathHelper.Lerp(1f, 0.05f, factor);

				//HSV[i - 1] = RGBtoHSV(NPCOldColor[i]);
				float min, max, tmp, H, S, V;
				float R = NPCOldColor[i].R * 1.0f / 255f, G = NPCOldColor[i].G * 1.0f / 255f, B = NPCOldColor[i].B * 1.0f / 255f;
				tmp = Math.Min(R, G);
				min = Math.Min(tmp, B);
				tmp = Math.Max(R, G);
				max = Math.Max(tmp, B);
				H = 0;
				if (max == min)
					H = 0;
				else if (max == R && G > B)
				{
					H = 60 * (G - B) * 1f / (max - min) + 0;
				}
				else if (max == R && G < B)
				{
					H = 60 * (G - B) * 1f / (max - min) + 360;
				}
				else if (max == G)
				{
					H = 60 * (B - R) * 1f / (max - min) + 120;
				}
				else if (max == B)
				{
					H = 60 * (R - G) * 1f / (max - min) + 240;
				}
				if (Math.Abs(H - oldH) > 200)
					bars.Add(new DashCoreVertexInfo(NPC.oldPos[i] + normalDir * width + new Vector2(20, 20), color, new Vector4((float)Math.Sqrt(factor), 1, w, oldH / 360f)));
				else
				{
					bars.Add(new DashCoreVertexInfo(NPC.oldPos[i] + normalDir * width + new Vector2(20, 20), color, new Vector4((float)Math.Sqrt(factor), 1, w, (float)H / 360f)));
				}
				bars.Add(new DashCoreVertexInfo(NPC.oldPos[i] + normalDir * -width + new Vector2(20, 20), color, new Vector4((float)Math.Sqrt(factor), 0, w, (float)H / 360f)));
				oldH = H;
			}

			var triangleList = new List<DashCoreVertexInfo>();

			if (bars.Count > 2)
			{

				// 按照顺序连接三角形
				triangleList.Add(bars[0]);
				var vertex = new DashCoreVertexInfo((bars[0].Position + bars[1].Position) * 0.5f + Vector2.Normalize(NPC.velocity) * 30, Color.White, new Vector4(0, 0.5f, 1, oldH / 360f));
				triangleList.Add(bars[1]);
				triangleList.Add(vertex);
				for (int i = 0; i < bars.Count - 2; i += 2)
				{
					triangleList.Add(bars[i]);
					triangleList.Add(bars[i + 2]);
					triangleList.Add(bars[i + 1]);

					triangleList.Add(bars[i + 1]);
					triangleList.Add(bars[i + 2]);
					triangleList.Add(bars[i + 3]);
				}
				RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
				// 干掉注释掉就可以只显示三角形栅格
				//RasterizerState rasterizerState = new RasterizerState();
				//rasterizerState.CullMode = CullMode.None;
				//rasterizerState.FillMode = FillMode.WireFrame;
				//Main.graphics.GraphicsDevice.RasterizerState = rasterizerState;

				var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
				var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;


				// 把变换和所需信息丢给shader
				ef2.Parameters["uTransform"].SetValue(model * projection);
				ef2.Parameters["uTime"].SetValue(-(float)Main.time * 0.012f);
				Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/heatmapRainbow").Value;
				Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/FogTraceGamma2").Value;
				Main.graphics.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/FogTraceBeta3").Value;
				Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
				Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
				Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;
				//Main.graphics.GraphicsDevice.Textures[0] = Main.magicPixel;
				//Main.graphics.GraphicsDevice.Textures[1] = Main.magicPixel;
				//Main.graphics.GraphicsDevice.Textures[2] = Main.magicPixel;

				ef2.CurrentTechnique.Passes[0].Apply();


				Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);

				Main.graphics.GraphicsDevice.RasterizerState = originalState;
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			}
			return false;
		}
		int TrueL = 1;
		float x = 0;
		float Sca = 0;
		int Tokill = -10;
		float ka = 0;
		bool CheckAutoPause = false;
		bool HasCheckAutoPause = false;

		float VagueStre = 0;

		private Color RGBtoHSV(Color color)
		{
			float min, max, tmp, H, S, V;
			float R = color.R * 1.0f / 255f, G = color.G * 1.0f / 255f, B = color.B * 1.0f / 255f;
			tmp = Math.Min(R, G);
			min = Math.Min(tmp, B);
			tmp = Math.Max(R, G);
			max = Math.Max(tmp, B);
			H = 0;
			if (max == min)
				H = 0;
			else if (max == R && G > B)
			{
				H = 60 * (G - B) * 1f / (max - min) + 0;
			}
			else if (max == R && G < B)
			{
				H = 60 * (G - B) * 1f / (max - min) + 360;
			}
			else if (max == G)
			{
				H = 60 * (B - R) * 1f / (max - min) + 120;
			}
			else if (max == B)
			{
				H = 60 * (R - G) * 1f / (max - min) + 240;
			}
			return new Color(H, 0, 0, 0);
		}
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (CheckNewBoss)
			{
				if (Main.netMode != NetmodeID.SinglePlayer)
				{
					CheckNewBoss = false;
					return;
				}
				if (!HasCheckAutoPause)
				{
					CheckAutoPause = Main.autoPause;
					HasCheckAutoPause = true;
				}
				Main.autoPause = true;
				Main.InGameUI.IsVisible = true;
				Main.gamePaused = true;
				if (VagueStre < 0.08f)
					VagueStre += 0.001f;
				else
				{
					VagueStre = 0.08f;
				}
				if (PauseCool > 0)
					PauseCool--;
				else
				{
					if (Main.mouseLeft)
					{
						CheckNewBoss = false;
						Main.gamePaused = false;
						Main.autoPause = CheckAutoPause;
					}
				}
			}
			if (!CheckNewBoss)
			{
				if (!Main.dedServ)
					Music = MythContent.QuickMusic("DashCore");
				if (VagueStre > 0f)
					VagueStre -= 0.001f;
				else
				{
					VagueStre = 0;
				}
			}
			x += 0.01f;
			float K = (float)(Math.Sin(x + Math.Sin(x) * 6) * (0.95 + Math.Sin(x + 0.24 + Math.Sin(x))) + 3) / 30f;
			float M = (float)(Math.Sin(x + Math.Tan(x) * 6) * (0.95 + Math.Cos(x + 0.24 + Math.Sin(x))) + 3) / 30f;
			spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightEffect").Value, NPC.Center - Main.screenPosition, null, new Color(NPC.color.R, NPC.color.G, NPC.color.B, 0) * 0.4f, 0, new Vector2(128f, 128f), K * 2.4f * Sca, SpriteEffects.None, 0f);
			spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightEffect").Value, NPC.Center - Main.screenPosition, null, new Color(NPC.color.R, NPC.color.G, NPC.color.B, 0) * 0.4f, (float)(Math.PI * 0.5), new Vector2(128f, 128f), K * 2.4f * Sca, SpriteEffects.None, 0f);
			spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightEffect").Value, NPC.Center - Main.screenPosition, null, new Color(NPC.color.R, NPC.color.G, NPC.color.B, 0) * 0.4f, (float)(Math.PI * 0.75), new Vector2(128f, 128f), M * 2.4f * Sca, SpriteEffects.None, 0f);
			spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightEffect").Value, NPC.Center - Main.screenPosition, null, new Color(NPC.color.R, NPC.color.G, NPC.color.B, 0) * 0.4f, (float)(Math.PI * 0.25), new Vector2(128f, 128f), M * 2.4f * Sca, SpriteEffects.None, 0f);
			spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightEffect").Value, NPC.Center - Main.screenPosition, null, new Color(NPC.color.R, NPC.color.G, NPC.color.B, 0) * 0.4f, x * 6f, new Vector2(128f, 128f), (M + K) * 2.4f * Sca, SpriteEffects.None, 0f);
			spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/LightEffect").Value, NPC.Center - Main.screenPosition, null, new Color(NPC.color.R, NPC.color.G, NPC.color.B, 0) * 0.4f, -x * 6f, new Vector2(128f, 128f), (float)Math.Sqrt(M * M + K * K) * 2.4f * Sca, SpriteEffects.None, 0f);
			spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			var bars = new List<Vertex2D>();
			float width = 12;
			if (Tokill > 0)
			{
				if (Tokill < 60)
					width = Tokill / 5f;
			}
			if (Tokill > 0)
			{
				if (Tokill < 100)
				{
					if (ka > 0.005)
						ka *= 0.96f;
					else
					{
						ka = 0;
					}
				}
			}
			else
			{
				if (ka < 1)
					ka += 0.01f;
				else
				{
					ka = 1;
				}
			}
			TrueL = 0;
			for (int i = 1; i < NPC.oldPos.Length; ++i)
			{
				if (NPC.oldPos[i] == Vector2.Zero)
					break;
				TrueL++;
			}
			for (int i = 1; i < NPC.oldPos.Length; ++i)
			{
				if (NPC.oldPos[i] == Vector2.Zero)
					break;
				var normalDir = NPC.oldPos[i - 1] - NPC.oldPos[i];
				normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
				//width *= (float)((Math.Log((i + 6)) / 6) / (double)(i + 6) * 24d - 12.7 * (i / 2000f));
				var factor = i / (float)TrueL;
				var w = MathHelper.Lerp(1f, 0.05f, factor);
				var NewFac = (float)Math.Sqrt(i + 1) / TrueL * 24 - NPC.localAI[0] / 30f;
				var NewFac2 = (float)Math.Sqrt(i) / TrueL * 24 - NPC.localAI[0] / 30f;
				Lighting.AddLight(NPC.oldPos[i], (255 - NPC.alpha) * 1.2f / 50f * ka * (1 - factor), (255 - NPC.alpha) * 0.7f / 50f * ka * (1 - factor), 0);
				//bars.Add(new VertexBase.CustomVertexInfo(NPC.oldPos[i] + normalDir * width + new Vector2(20, 20) - Main.screenPosition, new Color(254, 254, 254, 0), new Vector3(NewFac % 1f + 0.5f, 1, w)));
				//bars.Add(new VertexBase.CustomVertexInfo(NPC.oldPos[i] + normalDir * -width + new Vector2(20, 20) - Main.screenPosition, new Color(254, 254, 254, 0), new Vector3(NewFac % 1f + 0.5f, 0, w)));
				bars.Add(new Vertex2D(NPC.oldPos[i] + normalDir * width + new Vector2(20, 20) - Main.screenPosition, new Color(NPCOldColor[i].R, NPCOldColor[i].G, NPCOldColor[i].B, 0), new Vector3(factor, 1, w)));
				bars.Add(new Vertex2D(NPC.oldPos[i] + normalDir * -width + new Vector2(20, 20) - Main.screenPosition, new Color(NPCOldColor[i].R, NPCOldColor[i].G, NPCOldColor[i].B, 0), new Vector3(factor, 0, w)));
			}
			var Vx = new List<Vertex2D>();
			if (bars.Count > 2)
			{
				Vx.Add(bars[0]);
				var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f + Vector2.Normalize(NPC.velocity) * 30, new Color(NPC.color.R, NPC.color.G, NPC.color.B, 0), new Vector3(0, 0.5f, 1));
				Vx.Add(bars[1]);
				Vx.Add(vertex);
				for (int i = 0; i < bars.Count - 2; i += 2)
				{
					Vx.Add(bars[i]);
					Vx.Add(bars[i + 2]);
					Vx.Add(bars[i + 1]);

					Vx.Add(bars[i + 1]);
					Vx.Add(bars[i + 2]);
					Vx.Add(bars[i + 3]);
				}
			}
			Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/CoreFlame").Value;
			t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/Bosses/Acytaea/Projectiles/Metero").Value;
			Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
		}
		public struct DashCoreVertexInfo : IVertexType
		{
			private static VertexDeclaration _vertexDeclaration = new VertexDeclaration(new VertexElement[3]
			{
				new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
				new VertexElement(8, VertexElementFormat.Color, VertexElementUsage.Color, 0),
				new VertexElement(12, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 0)
			});
			public Vector2 Position;
			public Color Color;
			public Vector4 TexCoord;

			public DashCoreVertexInfo(Vector2 position, Color color, Vector4 texCoord)
			{
				Position = position;
				Color = color;
				TexCoord = texCoord;
			}

			public VertexDeclaration VertexDeclaration
			{
				get
				{
					return _vertexDeclaration;
				}
			}
		}
	}

}
