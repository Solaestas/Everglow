using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Dusts;
using Terraria.GameContent;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
    public class DreamStar_Pro : StabbingProjectile
    {
        public override int SoundTimer => 10;
        public override void SetDefaults()
        {
            Color = Color.Gold;
			TradeLength = 6;
			TradeShade = 0.4f;
			Shade = 0.2f;
			FadeTradeShade = 0.74f;
			FadeScale = 1;
			TradeLightColorValue = 1f;
			FadeLightColorValue = 0.9f;
			MaxLength = 1.05f;
			base.SetDefaults();
        }
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
		}
		public override void AI()
		{
			base.AI();
			Vector2 pos = Projectile.position + Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(0.4f, 8f);
			Vector2 vel = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(0.1f, 0.2f);
			if (Collision.CanHit(Projectile.Center - Projectile.velocity, 0, 0, pos + vel, 0, 0))
			{
				int type = ModContent.DustType<StarShine_yellow>();
				if(Main.rand.NextBool(3))
				{
					type = ModContent.DustType<StarShine_purple>();
				}
				Dust dust = Dust.NewDustDirect(pos, Projectile.width, Projectile.height, type, 0, 0, 0, default, Main.rand.NextFloat(0.75f, 1.25f));
				dust.velocity = vel;
			}
		}
		public override void PostDraw(Color lightColor)
		{
			Player player = Main.player[Projectile.owner];
			Texture2D itemTexture = TextureAssets.Item[Main.player[Projectile.owner].HeldItem.type].Value;
			Texture2D Shadow = ModAsset.StabbingProjectileShade.Value;
			Texture2D light = ModAsset.StabbingProjectile.Value;
			Vector2 drawOrigin = light.Size() / 2f;
			Vector2 drawShadowOrigin = Shadow.Size() / 2f;
			Main.spriteBatch.Draw(itemTexture, ItemDraw.Postion - Main.screenPosition, null, lightColor, ItemDraw.Rotation, itemTexture.Size() / 2f, ItemDraw.Size, ItemDraw.SpriteEffect, 0f);
			if (TradeShade > 0)
			{
				for (int f = TradeLength - 1; f > -1; f--)
				{
					Main.spriteBatch.Draw(Shadow, DarkDraw[f].Postion - Main.screenPosition, null, Color.White * (DarkDraw[f].Color.A / 255f), DarkDraw[f].Rotation, drawShadowOrigin, DarkDraw[f].Size, SpriteEffects.None, 0f);
					Color fadeLight = new Color(140, 120, 195) * (DarkDraw[f].Color.A / 255f);
					fadeLight.A = 0;
					fadeLight = fadeLight * TradeLightColorValue * MathF.Pow(FadeLightColorValue, f);
					if(fadeLight.G > 20)
					{
						fadeLight.G -= 20;
					}
					else
					{
						fadeLight.G = 0;
					}
					fadeLight = new Color(lightColor.R / 255f * fadeLight.R / 255f, lightColor.G / 255f * fadeLight.G / 255f, lightColor.B / 255f * fadeLight.B / 255f, 0);
					Main.spriteBatch.Draw(light, DarkDraw[f].Postion - Main.screenPosition, null, fadeLight, DarkDraw[f].Rotation, drawOrigin, DarkDraw[f].Size, SpriteEffects.None, 0f);
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
					Main.spriteBatch.Draw(light, LightDraw.Postion - Main.screenPosition, null, new Color(Color.R / 255f, Color.G / 255f, lightColor.B / 255f * Color.B / 255f, 0), LightDraw.Rotation, drawOrigin, LightDraw.Size, SpriteEffects.None, 0f);
				}
			}
		}
	}
}
