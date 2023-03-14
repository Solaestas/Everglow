using Everglow.Myth.Common;
using Everglow.Myth.LanternMoon.NPCs;
using Everglow.Myth.LanternMoon.Projectiles.LanternKing;
using Terraria.DataStructures;
using Terraria.Localization;

namespace Everglow.Myth.LanternMoon.NPCs.LanternGhostKing
{
	[AutoloadBossHead]
	public class LanternGhostKing : ModNPC
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Lantern Ghost King");
			DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "灯笼鬼王");
			Main.npcFrameCount[NPC.type] = 3;
		}
		public override void SetDefaults()
		{
			NPC.damage = 100;
			if (Main.expertMode)
				NPC.lifeMax = 20000;
			else
			{
				NPC.lifeMax = 30000;
			}
			NPC.width = 250;
			NPC.height = 150;
			NPC.defense = 50;
			NPC.value = 20000;
			NPC.aiStyle = -1;
			NPC.boss = true;
			NPC.knockBackResist = 0f;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.HitSound = SoundID.NPCHit3;
			Music = MythContent.QuickMusic("DashCore");
		}
		internal bool NearDie = false;
		internal Vector2 RingCenterTrend;
		internal Vector2 RingCenter;
		internal Vector2 Lantern3RingCenter;
		internal float RingRadius = 0;
		internal float RingRadiusTrend = 1200;
		internal bool canDespawn;
		public override bool CheckActive()
		{
			return canDespawn;
		}

		public override void OnSpawn(IEntitySource source)
		{
			NPC.localAI[0] = -2;
		}
		public override void AI()
		{
			NPC.TargetClosest(false);
			Player player = Main.player[NPC.target];
			if (NPC.localAI[0] > 14400)
				NPC.ai[0] = (14680 - NPC.localAI[0]) / 240f;
			else
			{
				NPC.ai[0] = 1;
			}
			if (Main.dayTime)
				canDespawn = true;
			else
			{
				canDespawn = false;
			}
			if (NPC.localAI[0] == -2)
				RingCenter = NPC.Center;
			if (NPC.localAI[0] % 15 == 0)
				NPC.frameCounter += 1;
			NPC.localAI[0] += 1;
			Lighting.AddLight(NPC.Center, (255 - NPC.alpha) * 0.75f / 255f * NPC.scale, (255 - NPC.alpha) * 0.24f * NPC.scale / 255f, (255 - NPC.alpha) * 0f / 255f * NPC.scale);
			if (!NearDie)
				NPC.dontTakeDamage = false;
			if (player.dead)
			{
				if (NPC.life < NPC.lifeMax)
					NPC.life += 10;
				else
				{
					NPC.life = NPC.lifeMax;
				}
			}

			if (NPC.localAI[0] <= 0)
			{
				NPC.rotation = NPC.velocity.X / 120f;
				Vector2 v = player.Center + new Vector2((float)Math.Sin(NPC.localAI[0] / 40f) * 500f, (float)Math.Sin((NPC.localAI[0] + 200) / 40f) * 50f - 350) - NPC.Center;
				if (NPC.velocity.Length() < 9f)
					NPC.velocity += v / v.Length() * 0.35f;
				NPC.velocity *= 0.96f;
				RingCenterTrend = NPC.Center;
				RingRadiusTrend = 1200;
			}
			//金线
			if (NPC.localAI[0] < 700 && NPC.localAI[0] > 0)
			{
				NPC.rotation = NPC.velocity.X / 120f;
				Vector2 v = player.Center + new Vector2((float)Math.Sin(NPC.localAI[0] / 40f) * 500f, (float)Math.Sin((NPC.localAI[0] + 200) / 40f) * 50f - 350) - NPC.Center;
				if (NPC.velocity.Length() < 9f)
					NPC.velocity += v / v.Length() * 0.35f;
				NPC.velocity *= 0.96f;
				if (NPC.localAI[0] % 30 == 1)
					ShootGoldLines();
				RingCenterTrend = NPC.Center;
				RingRadiusTrend = 1200;
			}
			if (NPC.localAI[0] >= 700 && NPC.localAI[0] < 1500)
			{
				if (NPC.localAI[0] % 250 == 0)
					Lantern3RingCenter = new Vector2(0, -300).RotatedBy(Main.rand.NextFloat(-0.25f, 0.25f) * Math.PI);
				if (NPC.localAI[0] % 250 < 20)
				{
					NPC.velocity *= 0.95f;
					NPC.rotation *= 0.95f;
				}
				if (NPC.localAI[0] % 250 < 30 && NPC.localAI[0] % 250 < 20)
				{
					NPC.velocity *= 0;
					NPC.rotation *= 0.95f;
				}
				if (NPC.localAI[0] % 250 == 30)
				{
					for (int i = 0; i < 12; i++)
					{
						Vector2 v1 = new Vector2(0, 100).RotatedBy(i / 6d * Math.PI);
						Vector2 v2 = v1 + NPC.Center;
						Projectile.NewProjectile(NPC.GetSource_FromAI(), v2.X, v2.Y, 0, 0, ModContent.ProjectileType<DarkLantern>(), 50, 0f, player.whoAmI, 120 - NPC.localAI[0] % 250, (float)(i / 6d * Math.PI));
					}
				}
				if (NPC.localAI[0] % 250 == 60)
				{
					for (int i = 0; i < 24; i++)
					{
						Vector2 v1 = new Vector2(0, 150).RotatedBy(i / 12d * Math.PI);
						Vector2 v2 = v1 + NPC.Center;
						Projectile.NewProjectile(NPC.GetSource_FromAI(), v2.X, v2.Y, 0, 0, ModContent.ProjectileType<DarkLantern>(), 50, 0f, player.whoAmI, 120 - NPC.localAI[0] % 250, (float)(i / 12d * Math.PI));
					}
				}
				if (NPC.localAI[0] % 250 == 90)
				{
					for (int i = 0; i < 36; i++)
					{
						Vector2 v1 = new Vector2(0, 200).RotatedBy(i / 18d * Math.PI);
						Vector2 v2 = v1 + NPC.Center;
						Projectile.NewProjectile(NPC.GetSource_FromAI(), v2.X, v2.Y, 0, 0, ModContent.ProjectileType<DarkLantern>(), 50, 0f, player.whoAmI, 120 - NPC.localAI[0] % 250, (float)(i / 18d * Math.PI));
					}
				}
				if (NPC.localAI[0] % 250 > 120)
				{
					Vector2 v = player.Center + Lantern3RingCenter + player.velocity * 30f;
					NPC.velocity += (v - NPC.Center) / (v - NPC.Center).Length() * 0.25f;
					if (NPC.velocity.Length() > 20f)
						NPC.velocity *= 0.96f;
				}
				RingCenterTrend = NPC.Center;
				RingRadiusTrend = 1200;
			}
			if (NPC.localAI[0] >= 1500 && NPC.localAI[0] < 1700)
			{
				NPC.rotation *= 0.95f;
				NPC.velocity *= 0.95f;
				if (NPC.localAI[0] == 1600)
				{
					for (int j = 0; j < 150; j++)
					{
						Vector2 v2 = new Vector2(0, Main.rand.Next(Main.rand.Next(0, 1200), 1200)).RotatedByRandom(Math.PI * 2);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), v2.X + NPC.Center.X, v2.Y + NPC.Center.Y, 0, 0, ModContent.ProjectileType<DarkLanternBomb>(), 50, 0f, player.whoAmI, v2.Length() / 4f, 0);
					}
				}
			}
			if (NPC.localAI[0] >= 1700 && NPC.localAI[0] < 2000)
			{
				NPC.rotation *= 0.95f;
				NPC.velocity *= 0.95f;
				if (NPC.localAI[0] == 1700)
				{
					for (int t = 0; t < Main.projectile.Length; t++)
					{
						if (Main.projectile[t].type == ModContent.ProjectileType<DarkLantern>() && Main.projectile[t].active && Main.projectile[t].timeLeft > 180)
							Main.projectile[t].timeLeft = 180;
					}
				}
				if (NPC.localAI[0] % 9 == 0)
				{
					float dx = (NPC.localAI[0] - 1700) / 300f;
					for (int j = 0; j < 6; j++)
					{
						Vector2 v2 = new Vector2(0, 1 + dx).RotatedBy(Math.PI * j / 3d + dx * dx * 4);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), v2.X + NPC.Center.X, v2.Y + NPC.Center.Y, v2.X, v2.Y, ModContent.ProjectileType<floatLantern2>(), 50, 0f, player.whoAmI, 0, 0);
					}
					for (int j = 0; j < 3; j++)
					{
						Vector2 v2 = new Vector2(0, 1 + dx).RotatedBy(Math.PI * j / 1.5 + dx * dx * 4);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), v2.X + NPC.Center.X, v2.Y + NPC.Center.Y, v2.X, v2.Y, ModContent.ProjectileType<floatLantern3>(), 50, 0f, player.whoAmI, 0, 0);
					}
					for (int j = 0; j < 3; j++)
					{
						Vector2 v2 = new Vector2(0, 1 + dx).RotatedBy(Math.PI * (j + 1.5) / 1.5 + dx * dx * 4);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), v2.X + NPC.Center.X, v2.Y + NPC.Center.Y, v2.X, v2.Y, ModContent.ProjectileType<floatLantern4>(), 50, 0f, player.whoAmI, 0, 0);
					}
				}
			}
			if (NPC.localAI[0] >= 2000 && NPC.localAI[0] < 2500)
			{
				NPC.rotation *= 0.95f;
				Vector2 v = player.Center + new Vector2(0, -350) - NPC.Center;
				if (NPC.velocity.Length() < 9f)
				{
					NPC.velocity += v / v.Length() * 0.35f;
					NPC.velocity.X += player.velocity.X * 0.07f;
				}
				NPC.velocity *= 0.96f;
				if (NPC.localAI[0] % 60 == 0)
					NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y + 100, ModContent.NPCType<FloatLantern>(), 0, 0, 0, 0, 0, 255);
				RingCenterTrend = NPC.Center;
				RingRadiusTrend = 1200;
			}
			if (NPC.localAI[0] >= 2500 && NPC.localAI[0] < 3000)
			{
				NPC.rotation *= 0.95f;
				Vector2 vz = player.Center + new Vector2(0, -350) - NPC.Center;
				if (NPC.velocity.Length() < 9f)
				{
					NPC.velocity += vz / vz.Length() * 0.35f;
					NPC.velocity.X += player.velocity.X * 0.07f;
				}
				NPC.velocity *= 0.96f;
				if (NPC.localAI[0] % 6 == 0)
				{
					Vector2 v = new Vector2(0, -1.8f).RotatedBy(Main.rand.NextFloat(-1f, 1f));
					Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center.X + Main.rand.Next(-600, 600), player.Center.Y - 500, v.X + NPC.velocity.X, v.Y + NPC.velocity.Y, ModContent.ProjectileType<floatLantern>(), 50, 0f, player.whoAmI, 0, 0);
				}
				if (NPC.localAI[0] % 120 == 0)
				{
					for (int i = 0; i < 5; i++)
					{
						Vector2 v0 = new Vector2(0, -Main.rand.Next(120, 570)).RotatedBy(Main.rand.NextFloat(-1.4f, 1.4f));
						Vector2 v = v0 / 1000000f;
						Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center.X + v0.X, player.Center.Y + v0.Y, -v.X, -v.Y, ModContent.ProjectileType<ExplodeLantern>(), 50, 0f, player.whoAmI, 0, 0);
					}
				}
				RingCenterTrend = NPC.Center;
				RingRadiusTrend = 1200;
			}
			if (NPC.localAI[0] >= 13000 && NPC.localAI[0] < 13700)
			{
				NPC.rotation *= 0.95f;
				NPC.velocity *= 0.95f;
				if (NPC.localAI[0] % 9 == 0)
				{
					for (int j = 0; j < 6; j++)
					{
						var v2 = new Vector2(0, 1);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), RingCenterTrend.X + (j - 2.5f) * 360 + (float)(Math.Sin(NPC.localAI[0] / 50f + j) * 150f), RingCenterTrend.Y - 1400, v2.X, v2.Y, ModContent.ProjectileType<floatLantern2>(), 16, 0f, player.whoAmI, 0, 0);
					}
				}
				if (NPC.localAI[0] == 13000)
				{
					for (int j = 0; j < 30; j++)
					{
						Projectile.NewProjectile(NPC.GetSource_FromAI(), RingCenter.X, RingCenter.Y, 0, 0, ModContent.ProjectileType<DarkLantern3>(), 16, 0f, player.whoAmI, j, 360);
					}
					for (int j = 0; j < 30; j++)
					{
						Projectile.NewProjectile(NPC.GetSource_FromAI(), RingCenter.X, RingCenter.Y, 0, 0, ModContent.ProjectileType<DarkLantern3>(), 16, 0f, player.whoAmI, j + 1, 720);
					}
					for (int j = 0; j < 30; j++)
					{
						Projectile.NewProjectile(NPC.GetSource_FromAI(), RingCenter.X, RingCenter.Y, 0, 0, ModContent.ProjectileType<DarkLantern3>(), 16, 0f, player.whoAmI, j + 2, 1080);
					}
				}
				if (NPC.localAI[0] % 300 == 0)
				{
					for (int j = 0; j < 10; j++)
					{
						Vector2 v = new Vector2(0, 120).RotatedBy(j / 5d * Math.PI);
						Vector2 v2 = v.RotatedBy(Math.PI * 1.5) / v.Length();
						Projectile.NewProjectile(NPC.GetSource_FromAI(), RingCenter.X + v.X, RingCenter.Y + v.Y, v2.X, v2.Y, ModContent.ProjectileType<floatLantern2>(), 16, 0f, player.whoAmI, 0, 0);
					}
					for (int j = 0; j < 10; j++)
					{
						Vector2 v = new Vector2(0, 300).RotatedBy(j / 5d * Math.PI);
						Vector2 v2 = v.RotatedBy(Math.PI * 1.5) / v.Length();
						Projectile.NewProjectile(NPC.GetSource_FromAI(), RingCenter.X + v.X, RingCenter.Y + v.Y, v2.X, v2.Y, ModContent.ProjectileType<floatLantern2>(), 16, 0f, player.whoAmI, 0, 0);
					}
				}
				RingCenterTrend = NPC.Center + new Vector2(0, -500);
				RingRadiusTrend = 600;
			}
			if (NPC.localAI[0] >= 13700 && NPC.localAI[0] < 14400)
			{
				NPC.rotation *= 0.95f;
				NPC.velocity *= 0.95f;
				if (NPC.localAI[0] == 13900)
				{
					for (int l = 0; l < 5; l++)
					{
						Vector2 v0 = new Vector2(0, -7).RotatedBy(Main.rand.NextFloat(0f, 6.28f));
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, v0, ModContent.ProjectileType<Redlight>(), 0, 0f, player.whoAmI, 0, 0);
					}
				}
				if (NPC.localAI[0] == 13700)
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, 48, 0, ModContent.ProjectileType<GoldLanternLine8>(), 0, 0f, player.whoAmI, 0, 0);
				RingCenterTrend = NPC.Center + new Vector2(0, -300);
				RingRadiusTrend = 1000;
			}
			if (NPC.localAI[0] == 2998)
				NPC.localAI[0] = 1;
			if (NPC.localAI[0] == 2999)
				NPC.localAI[0] = 1;
			if (NPC.localAI[0] >= 14200)
				NPC.StrikeNPC(10005, 0, 1);
			if (Main.dayTime)
				NPC.velocity.Y += 1;
			RingRadius = RingRadius * 0.99f + RingRadiusTrend * 0.01f;
			RingCenter = RingCenter * 0.99f + RingCenterTrend * 0.01f;
		}
		private void ShootGoldLines()
		{
			Player player = Main.player[NPC.target];
			Vector2 v2 = new Vector2(0, Main.rand.NextFloat(0, 0.25f)).RotatedByRandom(Math.PI * 2f);
			Vector2 v4 = new Vector2(0, Main.rand.NextFloat(0, 7f)).RotatedByRandom(Math.PI * 2f);
			if (NPC.localAI[0] % 60 == 1)
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + v4.X, NPC.Center.Y + 110f + v4.Y, NPC.velocity.X / 3f + v2.X, NPC.velocity.Y * 0.25f + v2.Y, ModContent.ProjectileType<GoldLanternLine>(), 75, 0f, player.whoAmI, 0f, 0f);
			for (int h = 0; h < 15; h++)
			{
				Vector2 vn = new Vector2(0, -20).RotatedBy(Main.rand.NextFloat(-2f, 2f) + NPC.rotation);
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + vn.X * 5, NPC.Center.Y + vn.Y * 5, NPC.velocity.X + vn.X, NPC.velocity.Y + vn.Y, ModContent.ProjectileType<GoldLanternLine4>(), 25, 0f, player.whoAmI, 0, 0);
			}
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
			{
				if (!NearDie)
				{
					NearDie = true;
					NPC.life = 1;
					NPC.active = true;
					NPC.dontTakeDamage = true;
					NPC.localAI[0] = 12999;
					return;
				}
				else
				{
					Vector2 goreVelocity = new Vector2(Main.rand.NextFloat(0.4f), 0).RotatedByRandom(6.283);
					int gr0 = Gore.NewGore(null, NPC.position + new Vector2(60 + Main.rand.NextFloat(-60, 60f), 40 + Main.rand.NextFloat(-60, 60f)), goreVelocity, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore0").Type, 1f);
					Main.gore[gr0].timeLeft = 900;
					for (int f = 0; f < 13; f++)
					{
						goreVelocity = new Vector2(Main.rand.NextFloat(-0.4f, 1.4f), 0).RotatedByRandom(6.283) * 6f;
						int gra0 = Gore.NewGore(null, NPC.position, goreVelocity, ModContent.Find<ModGore>("Everglow/FloatLanternGore3").Type, 1f);
						Main.gore[gra0].timeLeft = Main.rand.Next(600, 900);
						goreVelocity = new Vector2(Main.rand.NextFloat(-0.4f, 1.4f), 0).RotatedByRandom(6.283) * 6f;
						int gra1 = Gore.NewGore(null, NPC.position, goreVelocity, ModContent.Find<ModGore>("Everglow/FloatLanternGore4").Type, 1f);
						Main.gore[gra1].timeLeft = Main.rand.Next(600, 900);
						goreVelocity = new Vector2(Main.rand.NextFloat(-0.4f, 1.4f), 0).RotatedByRandom(6.283) * 6f;
						int gra2 = Gore.NewGore(null, NPC.position, goreVelocity, ModContent.Find<ModGore>("Everglow/FloatLanternGore5").Type, 1f);
						Main.gore[gra2].timeLeft = Main.rand.Next(600, 900);
						goreVelocity = new Vector2(Main.rand.NextFloat(-0.4f, 1.4f), 0).RotatedByRandom(6.283) * 6f;
						int gra3 = Gore.NewGore(null, NPC.position, goreVelocity, ModContent.Find<ModGore>("Everglow/FloatLanternGore6").Type, 1f);
						Main.gore[gra3].timeLeft = Main.rand.Next(600, 900);
					}

					goreVelocity = new Vector2(Main.rand.NextFloat(0.4f), 0).RotatedByRandom(6.283);
					int gr1 = Gore.NewGore(null, NPC.position + new Vector2(60 + Main.rand.NextFloat(-60, 60f), 40 + Main.rand.NextFloat(-60, 60f)), goreVelocity, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore1").Type, 1f);
					Main.gore[gr1].timeLeft = 900;

					goreVelocity = new Vector2(Main.rand.NextFloat(0.4f), 0).RotatedByRandom(6.283);
					int gr2 = Gore.NewGore(null, NPC.position + new Vector2(60 + Main.rand.NextFloat(-60, 60f), 40 + Main.rand.NextFloat(-60, 60f)), goreVelocity, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore2").Type, 1f);
					Main.gore[gr2].timeLeft = 900;

					goreVelocity = new Vector2(Main.rand.NextFloat(0.4f), 0).RotatedByRandom(6.283);
					int gr3 = Gore.NewGore(null, NPC.position + new Vector2(60 + Main.rand.NextFloat(-60, 60f), 40 + Main.rand.NextFloat(-60, 60f)), goreVelocity, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore3").Type, 1f);
					Main.gore[gr3].timeLeft = 900;

					goreVelocity = new Vector2(Main.rand.NextFloat(0.4f), 0).RotatedByRandom(6.283);
					int gr4 = Gore.NewGore(null, NPC.position + new Vector2(60 + Main.rand.NextFloat(-60, 60f), 40 + Main.rand.NextFloat(-60, 60f)), goreVelocity, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore4").Type, 1f);
					Main.gore[gr4].timeLeft = 900;

					goreVelocity = new Vector2(Main.rand.NextFloat(0.4f), 0).RotatedByRandom(6.283);
					int gr5 = Gore.NewGore(null, NPC.position + new Vector2(60 + Main.rand.NextFloat(-60, 60f), 40 + Main.rand.NextFloat(-60, 60f)), goreVelocity, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore5").Type, 1f);
					Main.gore[gr5].timeLeft = 900;

					goreVelocity = new Vector2(Main.rand.NextFloat(0.4f), 0).RotatedByRandom(6.283);
					int gr6 = Gore.NewGore(null, NPC.position + new Vector2(60 + Main.rand.NextFloat(-60, 60f), 40 + Main.rand.NextFloat(-60, 60f)), goreVelocity, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore6").Type, 1f);
					Main.gore[gr6].timeLeft = 900;

					goreVelocity = new Vector2(Main.rand.NextFloat(0.4f), 0).RotatedByRandom(6.283);
					int gr7 = Gore.NewGore(null, NPC.position + new Vector2(60 + Main.rand.NextFloat(-60, 60f), 40 + Main.rand.NextFloat(-60, 60f)), goreVelocity, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore5").Type, 1f);
					Main.gore[gr7].timeLeft = 900;

					goreVelocity = new Vector2(Main.rand.NextFloat(0.4f), 0).RotatedByRandom(6.283);
					int gr8 = Gore.NewGore(null, NPC.position + new Vector2(60 + Main.rand.NextFloat(-60, 60f), 40 + Main.rand.NextFloat(-60, 60f)), goreVelocity, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore6").Type, 1f);
					Main.gore[gr8].timeLeft = 900;

					goreVelocity = new Vector2(Main.rand.NextFloat(0.4f), 0).RotatedByRandom(6.283);
					int gr9 = Gore.NewGore(null, NPC.position + new Vector2(60 + Main.rand.NextFloat(-60, 60f), 40 + Main.rand.NextFloat(-60, 60f)), goreVelocity, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore5").Type, 1f);
					Main.gore[gr9].timeLeft = 900;

					goreVelocity = new Vector2(Main.rand.NextFloat(0.4f), 0).RotatedByRandom(6.283);
					int gr10 = Gore.NewGore(null, NPC.position + new Vector2(60 + Main.rand.NextFloat(-60, 60f), 40 + Main.rand.NextFloat(-60, 60f)), goreVelocity, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore6").Type, 1f);
					Main.gore[gr10].timeLeft = 900;

					goreVelocity = new Vector2(Main.rand.NextFloat(0.4f), 0).RotatedByRandom(6.283);
					int gr11 = Gore.NewGore(null, NPC.position + new Vector2(60 + Main.rand.NextFloat(-60, 60f), 40 + Main.rand.NextFloat(-60, 60f)), goreVelocity, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore5").Type, 1f);
					Main.gore[gr11].timeLeft = 900;

					goreVelocity = new Vector2(Main.rand.NextFloat(0.4f), 0).RotatedByRandom(6.283);
					int gr12 = Gore.NewGore(null, NPC.position + new Vector2(60 + Main.rand.NextFloat(-60, 60f), 40 + Main.rand.NextFloat(-60, 60f)), goreVelocity, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore6").Type, 1f);
					Main.gore[gr12].timeLeft = 900;

					goreVelocity = new Vector2(Main.rand.NextFloat(0.4f), 0).RotatedByRandom(6.283);
					int gr13 = Gore.NewGore(null, NPC.position + new Vector2(60 + Main.rand.NextFloat(-60, 60f), 40 + Main.rand.NextFloat(-60, 60f)), goreVelocity, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore7").Type, 1f);
					Main.gore[gr13].timeLeft = 900;

					goreVelocity = new Vector2(Main.rand.NextFloat(0.4f), 0).RotatedByRandom(6.283);
					int gr14 = Gore.NewGore(null, NPC.position + new Vector2(60 + Main.rand.NextFloat(-60, 60f), 40 + Main.rand.NextFloat(-60, 60f)), goreVelocity, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore8").Type, 1f);
					Main.gore[gr14].timeLeft = 900;

					goreVelocity = new Vector2(Main.rand.NextFloat(0.4f), 0).RotatedByRandom(6.283);
					int gr15 = Gore.NewGore(null, NPC.position + new Vector2(60 + Main.rand.NextFloat(-60, 60f), 40 + Main.rand.NextFloat(-60, 60f)), goreVelocity, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore9").Type, 1f);
					Main.gore[gr15].timeLeft = 900;

					goreVelocity = new Vector2(Main.rand.NextFloat(0.4f), 0).RotatedByRandom(6.283);
					int gr16 = Gore.NewGore(null, NPC.position + new Vector2(60 + Main.rand.NextFloat(-60, 60f), 40 + Main.rand.NextFloat(-60, 60f)), goreVelocity, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore10").Type, 1f);
					Main.gore[gr16].timeLeft = 900;
					for (int i = 0; i < 8; i++)
					{

						goreVelocity = new Vector2(Main.rand.NextFloat(0.4f), 0).RotatedByRandom(6.283);
						int gr17 = Gore.NewGore(null, NPC.position + new Vector2(Main.rand.NextFloat(-60, 60f), 40), goreVelocity, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore11").Type, 1f);
						Main.gore[gr17].timeLeft = 900;
					}
					for (int i = 0; i < 8; i++)
					{

						goreVelocity = new Vector2(Main.rand.NextFloat(0.4f), 0).RotatedByRandom(6.283);
						int gr18 = Gore.NewGore(null, NPC.position + new Vector2(Main.rand.NextFloat(-60, 60f), 40), goreVelocity, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore12").Type, 1f);
						Main.gore[gr18].timeLeft = 900;
					}
					for (int i = 0; i < 8; i++)
					{

						goreVelocity = new Vector2(Main.rand.NextFloat(0.4f), 0).RotatedByRandom(6.283);
						int gr19 = Gore.NewGore(null, NPC.position + new Vector2(Main.rand.NextFloat(-60, 60f), 40), goreVelocity, ModContent.Find<ModGore>("Everglow/LanternGhostKingGore12").Type, 1f);
						Main.gore[gr19].timeLeft = 900;
					}
				}
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.spriteBatch.Draw(texture, NPC.Center - Main.screenPosition + new Vector2(0f, NPC.gfxOffY), NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, new Vector2(250, 110), NPC.scale, SpriteEffects.None, 0f);
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{

			Player player = Main.player[NPC.target];
			var texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Texture2D tg2 = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/NPCs/LanternGhostKing/LanternGhostKingGlow2").Value;
			Texture2D tg3 = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/NPCs/LanternGhostKing/LanternGhostKingGlow3").Value;
			var value = new Vector2(NPC.Center.X, NPC.Center.Y);
			var vector = new Vector2(texture.Width / 2, texture.Height / Main.npcFrameCount[NPC.type] / 2);
			Vector2 vector2 = value - Main.screenPosition;
			vector2 -= new Vector2(tg2.Width, tg2.Height / Main.npcFrameCount[NPC.type]) * 1f / 2f;
			vector2 += vector * 1f + new Vector2(0f, 4f + NPC.gfxOffY);
			Main.spriteBatch.Draw(tg2, vector2 + new Vector2(0, 60), new Rectangle(0, (int)NPC.frameCounter % 4 * 220, 500, 220), new Color((int)(100 * NPC.ai[0]), (int)(100 * NPC.ai[0]), (int)(100 * NPC.ai[0]), 0), NPC.rotation, vector, 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(tg3, vector2 + new Vector2(0, 34), new Rectangle(0, (int)NPC.frameCounter % 3 * 280, 500, 220), new Color((int)(325 * NPC.ai[0]), (int)(325 * NPC.ai[0]), (int)(325 * NPC.ai[0]), 0), NPC.rotation, vector, 1f, SpriteEffects.None, 0f);


			if (!Main.gamePaused)
			{
				float StanL = (900 + (float)(Math.Sin(Main.time / 79d) * 50) + 3.5f) * 0.94f;
				Vector2 v3 = player.Center - RingCenter;
				if (Math.Abs(v3.Length() - StanL) < 40)
				{
					if (!player.dead)
					{
						Projectile.NewProjectile(null, player.Center, Vector2.Zero, ModContent.ProjectileType<Bosses.Acytaea.Projectiles.playerHit>(), NPC.damage / 4, 0, 0, 0, 0);
						player.AddBuff(BuffID.OnFire, 300);
					}
				}
			}

			Texture2D tex0 = MythContent.QuickTexture("LanternMoon/NPCs/LanternFlame0");
			DrawFlameRing((float)Main.timeForVisualEffects * 0.04f, 1300, RingCenter - Main.screenPosition, tex0);

		}
		public void DrawFlameRing(float phase, float range, Vector2 center, Texture2D tex, float TexCoord = 0)
		{
			var Vx = new List<Vertex2D>();
			for (int h = 0; h < 200; h++)
			{
				var color3 = new Color((int)(255 * NPC.ai[0]), (int)(255 * NPC.ai[0]), (int)(255 * NPC.ai[0]), 0);
				Vector2 v0 = new Vector2(0, range).RotatedBy(h / 100d * Math.PI + phase);
				Vector2 v1 = new Vector2(0, range).RotatedBy((h + 1) / 100d * Math.PI + phase);
				Vx.Add(new Vertex2D(center + v0, color3, new Vector3((TexCoord + h / 20f) % 1f, 0, 0)));
				Vx.Add(new Vertex2D(center + v1, color3, new Vector3(Math.Clamp((TexCoord + h / 20f) % 1f + 1f / 20f, 0, 1f), 0, 0)));
				Vx.Add(new Vertex2D(center, color3, new Vector3(Math.Clamp((TexCoord + h / 20f) % 1f + 0.5f / 20f, 0, 1f), 1, 0)));
			}

			Main.graphics.GraphicsDevice.Textures[0] = tex;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
		}
	}
}
