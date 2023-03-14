using Everglow.Sources.Modules.MythModule.Bosses.Acytaea.Projectiles;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.TheTusk.NPCs.Bosses.BloodTusk
{
	public class BloodyMouth2 : ModNPC
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("");
			DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "");
		}

		private int Coo = 720;
		private Vector2[] V = new Vector2[10];
		private Vector2[] VMax = new Vector2[10];
		public override void SetDefaults()
		{
			NPC.behindTiles = true;
			NPC.damage = 0;
			NPC.width = 10;
			NPC.height = 40;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			NPC.knockBackResist = 0f;
			NPC.value = Item.buyPrice(0, 0, 0, 0);
			NPC.aiStyle = -1;
			NPC.alpha = 255;
			NPC.lavaImmune = true;
			NPC.noGravity = false;
			NPC.noTileCollide = false;
			NPC.dontTakeDamage = true;
			NPC.damage = 0;
		}
		private int wait = 90;
		private bool squ = false;
		private bool Down = true;
		public override void AI()
		{
			VMax[0] = new Vector2(0, 166);
			VMax[1] = new Vector2(0, 166);
			NPC.TargetClosest(false);

			Player player = Main.player[NPC.target];
			if (NPC.collideX && Down)
			{
				NPC.active = false;
			}
			if (NPC.collideY)
			{
				if (V[0] == VMax[0])
				{

					NPC.rotation = 0;
					VMax[2].X = 1.57f;
				}
				if (V[0].Y > 0.5f)
				{

				}
			}
			if (NPC.collideY && NPC.alpha > 0 && !squ)
			{

				if (Main.tile[(int)(NPC.Bottom.X / 16d), (int)(NPC.Bottom.Y / 16d)].IsHalfBlock && Down)
				{
					Down = false;
					NPC.position.Y += 16;
				}
				startFight = true;
				V[0] = VMax[0];
				V[1] = VMax[1];
				NPC.alpha -= 25;
			}
			if (NPC.alpha <= 0)
			{
				NPC.alpha = 0;
				wait--;
			}
			if (wait <= 0 && !squ)
			{
				V[0] *= 0.9f;
				if ((player.Center - NPC.Center).Length() < 400)
				{


				}
				if (V[0].Y <= 0.5f)
				{
					Coo--;
					if (Coo < 480)
					{
						if (VMax[2].Y <= 0)
						{
							V[1] *= 0.99f;
							if (V[1].Y <= 15f)
							{
								V[1] *= 0.96f;
							}
							TuskModPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<TuskModPlayer>();
							mplayer.Shake = 4;
							float Str = 1;
							if ((player.Center - NPC.Center).Length() > 100)
							{
								Str = 100 / (player.Center - NPC.Center).Length();
							}
							mplayer.ShakeStrength = Str;
						}

						if (V[1].Y <= 2.5f)
						{
							VMax[2].X *= 0.9f;
							if (NPC.rotation < 1.9f)
							{
								NPC.rotation = 1.57f - VMax[2].X;
							}
							else
							{
								if (VMax[2].Y == 0)
								{
									VMax[2].Y = 1;
								}
							}
						}
					}
					if (Coo <= 0)
					{
						squ = true;
					}
				}
			}
			if (squ)
			{
				V[0].Y += 0.9f;
				if (V[0].Y > 80)
				{
					NPC.alpha += 15;
					if (NPC.alpha > 240)
					{
						NPC.active = false;
					}
				}
			}
			if (!Main.gamePaused)
			{
				if (NPC.rotation != 0)
				{
					if (Coo >= 120)
					{
						if ((player.Center - NPC.Center).Length() < 120)
						{
							// 弹幕
							Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center, new Vector2(0), ModContent.ProjectileType<playerHit>(), 90, 3f, Main.myPlayer, 0);
						}
					}
				}
			}
			if (Dam == 0)
			{
				Dam = 150;
				if (Main.expertMode)
				{
					Dam = 200;
				}
				if (Main.masterMode)
				{
					Dam = 300;
				}
			}
		}

		private int Dam = 0;
		public override void OnHitPlayer(Player player, int damage, bool crit)
		{
		}
		private bool startFight = false;
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (!startFight)
			{
				return false;
			}
			Color color = Lighting.GetColor((int)(NPC.Center.X / 16d), (int)(NPC.Center.Y / 16d));
			color = NPC.GetAlpha(color) * ((255 - NPC.alpha) / 255f);
			Texture2D t0 = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/NPCs/Bosses/BloodTusk/BloodyMouth2").Value;
			if (!Main.gamePaused)
			{
				for (int x = 0; x < t0.Width - (int)V[1].Y - 40; x += 20)
				{
					if (Main.LocalPlayer.active)
					{
						if (!Main.LocalPlayer.dead)
						{
							if ((Main.LocalPlayer.Center - NPC.Center + new Vector2(-x, 0).RotatedBy(NPC.rotation)).Length() < 30)
							{
								// 弹幕
								Projectile.NewProjectile(null, Main.LocalPlayer.Center, Vector2.Zero, ModContent.ProjectileType<playerHit>(), Dam / 8, 1, 0, 0, 0);
							}
							if ((Main.LocalPlayer.Center - NPC.Center + new Vector2(-x, 0).RotatedBy(NPC.rotation)).Length() < 30)
							{
								// 弹幕
								Projectile.NewProjectile(null, Main.LocalPlayer.Center, Vector2.Zero, ModContent.ProjectileType<playerHit>(), Dam / 8, 1, 0, 0, 0);
							}
						}
					}
				}
			}
			if (Coo < 405)
			{
				if (Coo >= 120)
				{
					int xz = 910 - (Coo * 2);
					if (xz > t0.Height)
					{
						xz = t0.Height;
					}
					Main.spriteBatch.Draw(t0, NPC.position - Main.screenPosition + new Vector2(-96, 0).RotatedBy(NPC.rotation) + V[1] - new Vector2(0, -18), new Rectangle(0, 0, t0.Width, xz), color, NPC.rotation, new Vector2(t0.Width / 2f, t0.Height / 2f), 1f, SpriteEffects.FlipHorizontally, 0f);
				}
				else
				{
					if (!Main.gamePaused)
					{
						V[1].Y += 4;
						for (int i = 0; i < 10; i++)
						{
							int k = Dust.NewDust(NPC.Bottom + new Vector2(-60, 0), 120, 0, DustID.Blood, 0, 0, 0, default, Main.rand.NextFloat(1.3f, 2.3f));
							Main.dust[k].noGravity = true;
						}

						if (V[1].Y > VMax[1].Y + 32)
						{
							NPC.active = false;
						}
					}

					Main.spriteBatch.Draw(t0, NPC.position - Main.screenPosition + new Vector2(-96, 0).RotatedBy(NPC.rotation) + V[1] + new Vector2(0, -58), new Rectangle((int)V[1].Y + 180, 0, t0.Width - (int)V[1].Y - 60, t0.Height), color, NPC.rotation, new Vector2((t0.Width - (int)V[1].Y - 60) / 2f, t0.Height / 2f), 1f, SpriteEffects.FlipHorizontally, 0f);
				}
			}


			if (Coo >= 200)
			{
				Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/NPCs/Bosses/BloodTusk/BloodyMouth2Tusk").Value;
				Main.spriteBatch.Draw(t, NPC.position - Main.screenPosition + new Vector2(-96, 0).RotatedBy(NPC.rotation) + V[0] - new Vector2(0, -18), new Rectangle(0, 0, t.Width, t.Height - (int)V[0].Y), color, NPC.rotation, new Vector2(t.Width / 2f, t.Height / 2f), 1f, SpriteEffects.FlipHorizontally, 0f);
			}

			return false;
		}
	}
}