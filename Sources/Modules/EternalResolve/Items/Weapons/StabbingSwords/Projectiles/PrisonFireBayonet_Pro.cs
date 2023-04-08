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
			FadeTradeShade = 0.84f;
			FadeScale = 1;
			TradeLightColorValue = 1f;
			FadeLightColorValue = 0.4f;
			MaxLength = 0.90f;
			DrawWidth = 0.6f;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			if (!player.wet || player.lavaWet)
			{
				target.AddBuff(BuffID.OnFire, 150);
			}
        }
		public override void PostDraw(Color lightColor)
		{
			Player player = Main.player[Projectile.owner];
			base.PostDraw(lightColor);
			Lighting.AddLight(Projectile.Center, 1f * Projectile.timeLeft / TradeLength, 0.4f * Projectile.timeLeft / TradeLength, 0f);
			Texture2D light = ModAsset.StabbingProjectile.Value;
			Vector2 drawOrigin = light.Size() / 2f;
			if (Main.myPlayer == Projectile.owner)
			{
				if (player.channel && !player.noItems && !player.CCed)
				{
					if(!player.wet || player.lavaWet)
					{
						Color = Color.Orange;
						TradeLength = 6;
						TradeShade = 1f;
						Shade = 0.65f;
						FadeTradeShade = 0.84f;
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
						FadeTradeShade = 0.74f;
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