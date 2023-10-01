using Everglow.EternalResolve.Buffs;
using Everglow.EternalResolve.VFXs;
using Terraria;
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
			FadeTradeShade = 0.74f;
			FadeScale = 1;
			TradeLightColorValue = 1f;
			FadeLightColorValue = 0.8f;
			MaxLength = 1.15f;
			DrawWidth = 0.4f;
		}
		public override void AI()
		{
			base.AI();
			GenerateVFX(1);
			if(Projectile.wet)
			{
				GenerateVFX(2);
			}
			Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Electric);
			Vector2 addPos = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(-0.1f, 12.4f);
			dust.position += addPos;
			dust.velocity = new Vector2(0, Main.rand.NextFloat(5f)).RotatedByRandom(6.283);
			dust.scale = Main.rand.NextFloat(0.55f, 0.85f) * (300 - addPos.Length()) / 300f;
			dust.noGravity = true;
			if(Main.timeForVisualEffects % 10 == 0)
			{
				SoundEngine.PlaySound(new SoundStyle("Everglow/EternalResolve/Sounds/ElectricCurrency").WithVolume(0.6f), Projectile.Center);
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
			float mulVelocity = 1f;
			for (int g = 0; g < Frequency; g++)
			{

				Vector2 afterVelocity = Projectile.velocity;
				afterVelocity.RotatedBy(Main.rand.NextFloat(-0.3f, 0.3f));
				float mulWidth = 1f;
				var yoenLeZedElecticFlowDust = new YoenLeZedElecticFlowDust
				{
					velocity = afterVelocity * Main.rand.NextFloat(1.5f,1.6f) * mulVelocity,
					Active = true,
					Visible = true,
					position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) + Projectile.velocity * Main.rand.NextFloat(-2.0f, -1.2f),
					maxTime = Main.rand.Next(6, 11),
					ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), Main.rand.NextFloat(-0.08f, 0.08f), Main.rand.NextFloat(26.6f, 28f) * mulWidth, Projectile.owner }
				};
				Ins.VFXManager.Add(yoenLeZedElecticFlowDust);
			}
		}
		public void SplitVFX(int Frequency)
		{
			float mulVelocity = 1f;
			for (int g = 0; g < Frequency; g++)
			{

				Vector2 afterVelocity = Projectile.velocity;
				afterVelocity.RotatedBy(Main.rand.NextFloat(-0.3f, 0.3f));
				float mulWidth = 1f;
				var yoenLeZedElecticFlowDust = new YoenLeZedElecticFlowDust
				{
					velocity = afterVelocity * Main.rand.NextFloat(1.5f, 1.6f) * mulVelocity,
					Active = true,
					Visible = true,
					position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) + Projectile.velocity * Main.rand.NextFloat(-2.0f, -1.2f),
					maxTime = Main.rand.Next(6, 11),
					ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), Main.rand.NextFloat(-0.08f, 0.08f), Main.rand.NextFloat(26.6f, 28f) * mulWidth, Projectile.owner }
				};
				Ins.VFXManager.Add(yoenLeZedElecticFlowDust);
			}
		}
		public override void PostDraw(Color lightColor)
		{
			Lighting.AddLight(Projectile.Center + Projectile.velocity, 0.2f * Projectile.timeLeft / TradeLength, 0.24f * Projectile.timeLeft / TradeLength, 0.3f * Projectile.timeLeft / TradeLength);
			Player player = Main.player[Projectile.owner];
			Texture2D itemTexture = TextureAssets.Item[Main.player[Projectile.owner].HeldItem.type].Value;
			Texture2D Shadow = ModAsset.StabbingProjectileShade.Value;
			Texture2D light = ModAsset.StabbingProjectile.Value;
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
						Main.spriteBatch.Draw(light, DarkDraw[f].Postion - Main.screenPosition, light.Frame(1,6,1,1), GlowColor * MathF.Pow(FadeGlowColorValue, f), DarkDraw[f].Rotation, drawShadowOrigin, DarkDraw[f].Size, SpriteEffects.None, 0f);
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
			target.AddBuff(ModContent.BuffType<OnElectric>(), 180);
			base.OnHitNPC(target, hit, damageDone);
		}
	}
}