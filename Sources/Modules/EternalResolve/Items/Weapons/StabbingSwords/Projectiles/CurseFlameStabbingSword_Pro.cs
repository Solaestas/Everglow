using Everglow.Commons.VFX.CommonVFXDusts;
using Everglow.Commons.Weapons.StabbingSwords;
using Terraria.GameContent;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
    public class CurseFlameStabbingSword_Pro : StabbingProjectile
    {
		public override void SetDefaults()
		{
			Color = new Color(87, 224, 0);
			base.SetDefaults();
			TradeLength = 9;
			TradeShade = 0.3f;
			Shade = 0.6f;
			FadeShade = 0.9f;
			FadeScale = 1;
			TradeLightColorValue = 1f;
			FadeLightColorValue = 0.8f;
			MaxLength = 1.15f;
			DrawWidth = 0.4f;
		}
		public override void VisualParticle()
		{
			if (Main.rand.NextBool(2))
				GenerateVFX(1);
		}
		public void GenerateVFX(int Frequency)
		{
			float mulVelocity = Main.rand.NextFloat(0.75f, 1.5f);
			for (int g = 0; g < Frequency; g++)
			{
				Vector2 newVelocity = new Vector2(0, mulVelocity * Main.rand.NextFloat(0f, 2f)).RotatedByRandom(MathHelper.TwoPi) + Projectile.velocity * 0.6f;
				var fire = new CurseFlameDust
				{
					velocity = newVelocity,
					Active = true,
					Visible = true,
					position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) + Vector2.Normalize(Projectile.velocity) * Main.rand.NextFloat(72f),
					maxTime = Main.rand.Next(16, 45),
					scale = Main.rand.NextFloat(10f, 40f),
					rotation = Main.rand.NextFloat(6.283f),
					ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0, 1f }
				};
				Ins.VFXManager.Add(fire);
			}
			for (int g = 0; g < Frequency * 2; g++)
			{
				Vector2 newVelocity = new Vector2(0, mulVelocity * Main.rand.NextFloat(0f, 4f)).RotatedByRandom(MathHelper.TwoPi) + Projectile.velocity * 0.2f;
				var spark = new CurseFlameSparkDust
				{
					velocity = newVelocity,
					Active = true,
					Visible = true,
					position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) + Vector2.Normalize(Projectile.velocity) * Main.rand.NextFloat(72f),
					maxTime = Main.rand.Next(37, 145),
					scale = Main.rand.NextFloat(0.1f, Main.rand.NextFloat(4f, 47.0f)),
					rotation = Main.rand.NextFloat(6.283f),
					ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), Main.rand.NextFloat(-0.03f, 0.03f) }
				};
				Ins.VFXManager.Add(spark);
			}
		}
		public override void PostDraw(Color lightColor)
		{
			float value = Projectile.timeLeft / TradeLength / (Projectile.extraUpdates + 1);
			Lighting.AddLight(Projectile.Center + Projectile.velocity, 0.2f * value, 0.24f * value, 0.3f * value);
			Player player = Main.player[Projectile.owner];
			Texture2D itemTexture = TextureAssets.Item[Main.player[Projectile.owner].HeldItem.type].Value;
			Texture2D Shadow = Commons.ModAsset.StabbingProjectileShade.Value;
			Texture2D light = Commons.ModAsset.StabbingProjectile.Value;
			Vector2 drawOrigin = light.Size() / 2f;
			Vector2 drawShadowOrigin = Shadow.Size() / 2f;
			Main.spriteBatch.Draw(itemTexture, ItemDraw.Postion - Main.screenPosition + Projectile.velocity, null, lightColor, ItemDraw.Rotation, itemTexture.Size() / 2f, ItemDraw.Size, ItemDraw.SpriteEffect, 0f);
			if (TradeShade > 0)
			{
				for (int f = TradeLength - 1; f > -1; f--)
				{
					Main.spriteBatch.Draw(Shadow, DarkDraw[f].Postion - Main.screenPosition, null, Color.White * (DarkDraw[f].Color.A / 255f), DarkDraw[f].Rotation, drawShadowOrigin, DarkDraw[f].Size, SpriteEffects.None, 0f);
					Color fadeLight = Color * (DarkDraw[f].Color.A / 255f);
					fadeLight.A = 0;
					fadeLight = fadeLight * TradeLightColorValue * MathF.Pow(FadeLightColorValue, f);
					fadeLight = new Color(fadeLight.R / 255f, fadeLight.G / 255f, fadeLight.B / 255f, 0);
					Main.spriteBatch.Draw(light, DarkDraw[f].Postion - Main.screenPosition, null, fadeLight, DarkDraw[f].Rotation, drawOrigin, DarkDraw[f].Size, SpriteEffects.None, 0f);
					if (GlowColor != Color.Transparent)
					{
						Main.spriteBatch.Draw(light, DarkDraw[f].Postion - Main.screenPosition, light.Frame(1, 6, 1, 1), GlowColor * MathF.Pow(FadeGlowColorValue, f), DarkDraw[f].Rotation, drawShadowOrigin, DarkDraw[f].Size, SpriteEffects.None, 0f);
					}
				}
			}
			if (Main.myPlayer == Projectile.owner)
			{
				if (player.channel && !player.noItems && !player.CCed)
				{
					if (Shade > 0)
					{
						Main.spriteBatch.Draw(Shadow, LightDraw.Postion - Main.screenPosition, null, Color.White * Shade, LightDraw.Rotation, drawShadowOrigin, LightDraw.Size, SpriteEffects.None, 0f);
					}
					Color glowColor = Color;
					glowColor.A = 0;
					Main.spriteBatch.Draw(light, LightDraw.Postion - Main.screenPosition, null, glowColor, LightDraw.Rotation, drawOrigin, LightDraw.Size, SpriteEffects.None, 0f);
					if (GlowColor != Color.Transparent)
					{
						Main.spriteBatch.Draw(light, LightDraw.Postion - Main.screenPosition, null, GlowColor, LightDraw.Rotation, drawShadowOrigin, LightDraw.Size, SpriteEffects.None, 0f);
					}
				}
			}
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.CursedInferno, 600);
			base.OnHitNPC(target, hit, damageDone);
		}
	}
}