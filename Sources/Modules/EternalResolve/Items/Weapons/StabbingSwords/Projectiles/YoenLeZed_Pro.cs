using Everglow.Commons.Templates.Weapons.StabbingSwords;
using Everglow.Commons.VFX.CommonVFXDusts;
using Everglow.EternalResolve.Buffs;
using Terraria.Audio;
using Terraria.GameContent;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
	public class YoenLeZed_Pro : StabbingProjectile
	{
		public override void SetDefaults()
		{
			Color = new Color(100, 180, 255);
			base.SetDefaults();
			TradeLength = 6;
			TradeShade = 0.2f;
			Shade = 0.2f;
			FadeShade = 0.74f;
			FadeScale = 1;
			TradeLightColorValue = 1f;
			FadeLightColorValue = 0.8f;
			MaxLength = 1.15f;
			DrawWidth = 0.4f;
		}
		public override void VisualParticle()
		{
			GenerateVFX(1);
			SplitVFX(2);
		}
		public override void AI()
		{
			base.AI();
			if (UpdateTimer % Projectile.extraUpdates == 0)
			{
				if (Main.timeForVisualEffects % 10 == 0)
				{
					SoundEngine.PlaySound(new SoundStyle("Everglow/EternalResolve/Sounds/ElectricCurrency").WithVolume(0.6f), Projectile.Center);
				}
			}
		}
		public override void OnKill(int timeLeft)
		{
			ActiveSound sound = SoundEngine.FindActiveSound(new SoundStyle("Everglow/EternalResolve/Sounds/ElectricCurrency").WithVolume(0.6f));
			if (sound != null)
			{
				sound.Stop();
			}
		}
		public void GenerateVFX(int Frequency)
		{
			float mulVelocity = Main.rand.NextFloat(0.25f, 0.5f);
			for (int g = 0; g < Frequency; g++)
			{
				float size = Main.rand.NextFloat(8f, Main.rand.NextFloat(8f, 16f));
				Vector2 afterVelocity = Projectile.velocity;
				var electric = new ElectricCurrent
				{
					velocity = afterVelocity * mulVelocity,
					Active = true,
					Visible = true,
					position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283),
					maxTime = size * size / 8f,
					scale = size,
					ai = new float[] { Main.rand.NextFloat(0.0f, 0.6f), size / 2, 0 }
				};
				Ins.VFXManager.Add(electric);
			}
		}
		public void SplitVFX(int Frequency)
		{
			float mulVelocity = 1f;
			for (int g = 0; g < Frequency; g++)
			{
				float size = Main.rand.NextFloat(8f, Main.rand.NextFloat(4f, 10f));
				Vector2 afterVelocity = Projectile.velocity;
				var electric = new ElectricCurrent
				{
					velocity = afterVelocity * mulVelocity,
					Active = true,
					Visible = true,
					position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) + Projectile.velocity * MathF.Sqrt(Main.rand.NextFloat(1f)) * 6f,
					maxTime = size * size / 8f,
					scale = size,
					ai = new float[] { Main.rand.NextFloat(0.0f, 0.6f), size, Main.rand.NextFloat(0.2f, Main.rand.NextFloat(0.2f, 0.4f)) }
				};
				Ins.VFXManager.Add(electric);
			}
		}
		public override void PostDraw(Color lightColor)
		{
			Lighting.AddLight(Projectile.Center + Projectile.velocity, 0.2f * Projectile.timeLeft / TradeLength, 0.24f * Projectile.timeLeft / TradeLength, 0.3f * Projectile.timeLeft / TradeLength);
			Player player = Main.player[Projectile.owner];
			Texture2D itemTexture = TextureAssets.Item[Main.player[Projectile.owner].HeldItem.type].Value;
			Texture2D Shadow = Commons.ModAsset.Star2_black.Value;
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
			Dust d = Dust.NewDustDirect(target.Center, 0, 0, ModContent.DustType<ElectricMiddleDust>(), 0, 0);
			d.scale = Main.rand.NextFloat(0.85f, 1.15f) * 0.1f;
			target.AddBuff(ModContent.BuffType<OnElectric>(), 180);
			base.OnHitNPC(target, hit, damageDone);
		}
	}
}