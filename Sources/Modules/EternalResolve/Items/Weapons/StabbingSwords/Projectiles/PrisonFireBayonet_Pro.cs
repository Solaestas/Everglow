using Everglow.Commons.VFX.CommonVFXDusts;
using Everglow.Commons.Weapons.StabbingSwords;
using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Dusts;
using Everglow.EternalResolve.VFXs;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
	public class PrisonFireBayonet_Pro : StabbingProjectile
	{
		public override void SetDefaults()
		{
			Color = Color.Orange;
			base.SetDefaults();
			TradeLength = 6;
			TradeShade = 1f;
			Shade = 0.65f;
			FadeShade = 0.84f;
			FadeScale = 1;
			TradeLightColorValue = 1f;
			FadeLightColorValue = 0.4f;
			MaxLength = 0.90f;
			DrawWidth = 0.4f;
		}
		public int SuddenCooling = 0;
		public override void OnSpawn(IEntitySource source)
		{
			Player player = Main.player[Projectile.owner];
			if (!player.wet || player.lavaWet)
			{
				SuddenCooling = 0;
			}
			else
			{
				SuddenCooling = 60;
			}
		}
		public void GenerateSmoke(int frequency)
		{
			for (int g = 0; g < frequency; g++)
			{
				Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(0f, 4f)).RotatedByRandom(MathHelper.TwoPi) + new Vector2(0, -4f);
				;
				var somg = new VaporDust
				{
					velocity = newVelocity,
					Active = true,
					Visible = true,
					position = Projectile.Center + new Vector2(Main.rand.NextFloat(0, 6f), 0).RotatedByRandom(6.283) + newVelocity * 3 + Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.24f, 0.24f)) * MathF.Sqrt(Main.rand.NextFloat(1f)) * 9f,
					maxTime = Main.rand.Next(10, 90),
					scale = Main.rand.NextFloat(20f, 135f),
					rotation = Main.rand.NextFloat(6.283f),
					ai = new float[] { Main.rand.NextFloat(-0.05f, -0.01f), 0 }
				};
				Ins.VFXManager.Add(somg);
			}
		}
		public override void AI()
		{
			base.AI();
			Player player = Main.player[Projectile.owner];
			if (!player.wet || player.lavaWet)
			{
				for (int x = 0; x < 4; x++)
				{
					Vector2 pos = Projectile.position + Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(0.4f, 6f);
					Vector2 vel = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(0.2f, 0.4f);
					if (Collision.CanHit(Projectile.Center - Projectile.velocity, 0, 0, pos + vel, 0, 0))
					{
						Dust dust = Dust.NewDustDirect(pos, Projectile.width, Projectile.height, ModContent.DustType<FlameShine>(), 0, 0, 0, default, Main.rand.NextFloat(0.35f, 0.6f));
						dust.velocity = vel;
					}
				}
				if (SuddenCooling > 0)
				{
					for (int x = 0; x < 4; x++)
					{
						Vector2 posII = Projectile.position + Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(0.4f, 8f);
						Vector2 velII = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(0.04f, 0.08f) + new Vector2(Main.rand.NextFloat(-1f, 1f), -SuddenCooling / 12f);
						if (Collision.CanHit(Projectile.Center - Projectile.velocity, 0, 0, posII + velII, 0, 0))
						{
							GenerateSmoke(1);
						}
					}
					SuddenCooling -= 1;
				}
				else
				{
					SuddenCooling = 0;
				}
				Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(0f, 2f)).RotatedByRandom(MathHelper.TwoPi) + Projectile.velocity * 0.2f + player.velocity * 0.5f;
				var fire = new BayonetFlameDust
				{
					velocity = newVelocity,
					Active = true,
					Visible = true,
					position = player.Center + new Vector2(Main.rand.NextFloat(0, 6f), 0).RotatedByRandom(6.283) + newVelocity * 3 + Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.24f, 0.24f)) * MathF.Sqrt(Main.rand.NextFloat(1f)) * 6f,
					maxTime = Main.rand.Next(6, 25),
					scale = Main.rand.NextFloat(10f, 50f),
					rotation = Main.rand.NextFloat(6.283f),
					ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 }
				};
				Ins.VFXManager.Add(fire);
			}
			else
			{
				Vector2 pos = Projectile.position + Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(0.4f, 8f);
				Vector2 vel = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(0.04f, 0.08f);
				if (Collision.CanHit(Projectile.Center - Projectile.velocity, 0, 0, pos + vel, 0, 0))
				{
					GenerateSmoke(1);
				}
				if (SuddenCooling < 60)
				{
					SuddenCooling += 1;
					for (int x = 0; x < 4; x++)
					{
						Vector2 posII = Projectile.position + Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(0.4f, 8f);
						Vector2 velII = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(0.04f, 0.08f) + new Vector2(Main.rand.NextFloat(-1f, 1f), (SuddenCooling - 60f) / 12f);
						if (Collision.CanHit(Projectile.Center - Projectile.velocity, 0, 0, posII + velII, 0, 0))
						{
							GenerateSmoke(1);
						}
					}
					if (SuddenCooling == 1)
					{
						SoundEngine.PlaySound(new SoundStyle("Everglow/EternalResolve/Sounds/CoolingSword"), Projectile.Center);
					}
				}
				else
				{
					SuddenCooling = 60;
				}
			}
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			if (!player.wet || player.lavaWet)
			{
				target.AddBuff(BuffID.OnFire, 150);
			}
		}
		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			Player player = Main.player[Projectile.owner];
			if (player.lavaWet)
			{
				modifiers.FinalDamage *= 2f;
			}
		}
		public override void PostDraw(Color lightColor)
		{
			Player player = Main.player[Projectile.owner];
			base.PostDraw(lightColor);
			Lighting.AddLight(Projectile.Center + Projectile.velocity, 1f * Projectile.timeLeft / TradeLength, 0.4f * Projectile.timeLeft / TradeLength, 0f);
			Texture2D light = Commons.ModAsset.StabbingProjectile.Value;
			Vector2 drawOrigin = light.Size() / 2f;
			if (Main.myPlayer == Projectile.owner)
			{
				if (player.channel && !player.noItems && !player.CCed)
				{
					if (!player.wet || player.lavaWet)
					{
						Color = Color.Orange;
						TradeLength = 6;
						TradeShade = 1f;
						Shade = 0.65f;
						FadeShade = 0.84f;
						FadeScale = 1;
						TradeLightColorValue = 1f;
						FadeLightColorValue = 0.4f;
						MaxLength = 0.90f;
						DrawWidth = 0.6f;

						for (int f = Projectile.timeLeft - 1; f > -1; f--)
						{
							float value = (TradeLength - f) / (float)TradeLength;
							Main.spriteBatch.Draw(light, DarkDraw[f].Postion - Main.screenPosition, null, new Color(value, value * value * value * 0.5f, 0, 0), DarkDraw[f].Rotation, drawOrigin, DarkDraw[f].Size, SpriteEffects.None, 0f);
						}
						Main.spriteBatch.Draw(light, LightDraw.Postion - Main.screenPosition, null, new Color(255, 155, 0, 0), LightDraw.Rotation, drawOrigin, LightDraw.Size, SpriteEffects.None, 0f);
					}
					else
					{
						Color = new Color(80, 0, 0, 0);
						TradeLength = 6;
						TradeShade = 0.4f;
						Shade = 0.65f;
						FadeShade = 0.74f;
						FadeScale = 1;
						TradeLightColorValue = 1f;
						FadeLightColorValue = 0.4f;
						MaxLength = 0.90f;
						DrawWidth = 0.6f;
						for (int f = Projectile.timeLeft - 1; f > -1; f--)
						{
							float value = (TradeLength - f) / (float)TradeLength;
							value *= value;
							Main.spriteBatch.Draw(light, DarkDraw[f].Postion - Main.screenPosition, null, new Color(value * 0.1f, 0, 0, 0), DarkDraw[f].Rotation, drawOrigin, DarkDraw[f].Size, SpriteEffects.None, 0f);
						}
						Main.spriteBatch.Draw(light, LightDraw.Postion - Main.screenPosition, null, new Color(20, 0, 0, 0), LightDraw.Rotation, drawOrigin, LightDraw.Size, SpriteEffects.None, 0f);
					}
				}
			}
		}
	}
}