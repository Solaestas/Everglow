using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ID;

namespace Everglow.Ocean.Projectiles
{
    public class OceanWave : ModProjectile
	{
		public override void SetStaticDefaults()
		{
            // base.DisplayName.SetDefault("海洋波纹");
			Main.projFrames[base.Projectile.type] = 1;
		}
		public override void SetDefaults()
		{
			base.Projectile.width = 40;
			base.Projectile.height = 40;
			base.Projectile.hostile = false;
			base.Projectile.ignoreWater = true;
			base.Projectile.tileCollide = false;
			base.Projectile.penetrate = -1;
			base.Projectile.timeLeft = 450;
			base.Projectile.alpha = 0;
            base.Projectile.friendly = true;
			this.CooldownSlot = 1;
			ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 20;
            ProjectileID.Sets.TrailingMode[base.Projectile.type] = 0;
		}
		public override void AI()
		{
			Lighting.AddLight(base.Projectile.Center, (float)(255 - base.Projectile.alpha) * 0.0f / 255f * base.Projectile.scale, (float)(255 - base.Projectile.alpha) * 0.3f / 255f * base.Projectile.scale, (float)(255 - base.Projectile.alpha) * 1f / 255f);
			base.Projectile.spriteDirection = 1;
			base.Projectile.rotation = (float)Math.Atan2((double)base.Projectile.velocity.Y, (double)base.Projectile.velocity.X);
			base.Projectile.velocity *= 0.99f;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(new Color(255, 255, 255, base.Projectile.alpha));
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D = TextureAssets.Projectile[base.Projectile.type].Value;
            int num = TextureAssets.Projectile[base.Projectile.type].Value.Height;
			Main.spriteBatch.Draw(texture2D, base.Projectile.Center - Main.screenPosition + new Vector2(0f, base.Projectile.gfxOffY), new Rectangle?(new Rectangle(0, 0, texture2D.Width, num)), base.Projectile.GetAlpha(lightColor), base.Projectile.rotation, new Vector2((float)texture2D.Width / 2f, (float)num / 2f), base.Projectile.scale, SpriteEffects.None, 1f);
			SpriteEffects effects = SpriteEffects.None;
			if (base.Projectile.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
			int frameHeight = 10;
            if (Projectile.timeLeft < 400)
            {
                frameHeight = (int)(10 + (400 - Projectile.timeLeft) / 40f * 3f);
            }
            Vector2 value = new Vector2(base.Projectile.Center.X, base.Projectile.Center.Y);
            Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
            Vector2 vector2 = value - Main.screenPosition;
			if(Projectile.timeLeft > 10)
			{
                int num5 = (int)Player.FindClosest(base.Projectile.Center, 1, 1);
                float num100 = 2 / (Main.player[num5].Center - Projectile.Center).Length();
                for (int k = 0; k < 60; k++)
                {
                    Vector2 v = (Projectile.Center - Main.player[num5].Center) * num100;
                    v = v.RotatedBy(-k / 150f * num100 * 200f + (float)Math.PI / 2) * k;
                    Vector2 v2 = (Projectile.Center - Main.player[num5].Center) * num100 * 1.2f * (float)Math.Sin(Projectile.timeLeft / 10f + 0.25f * k) * ((60 - k) / 30f);
                    Vector2 drawPos = Projectile.Center + v2 - v - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                    Color color = new Color(100 * Projectile.timeLeft / 24f + 155, 100 * Projectile.timeLeft / 24f + 155, 100 * Projectile.timeLeft / 24f + 155, base.Projectile.alpha) * ((60 - k) / 60f) * (Projectile.timeLeft / (Projectile.timeLeft < 120 ? 450f * (125 - Projectile.timeLeft) * (125 - Projectile.timeLeft) / 25f : 450f));
                    Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("Everglow/Ocean/Projectiles/海洋波纹~"), drawPos, new Rectangle(0, frameHeight * Projectile.frame, ModContent.Request<Texture2D>("Everglow/Ocean/Projectiles/海洋波纹~").Width(), frameHeight), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
                }
                for (int k = 0; k < 60; k++)
                {
                    Vector2 v = (Projectile.Center - Main.player[num5].Center) * num100;
                    v = v.RotatedBy(k / 150f * num100 * 200f + (float)Math.PI / 2) * k;
                    Vector2 v2 = (Projectile.Center - Main.player[num5].Center) * num100 * 1.2f * (float)Math.Sin(Projectile.timeLeft / 10f + 0.25f * -k) * ((60 - k) / 30f);
                    Vector2 drawPos = Projectile.Center + v2 + v - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                    Color color = new Color(100 * Projectile.timeLeft / 24f + 155, 100 * Projectile.timeLeft / 24f + 155, 100 * Projectile.timeLeft / 24f + 155, base.Projectile.alpha) * ((60 - k) / 60f) * (Projectile.timeLeft / (Projectile.timeLeft < 120 ? 450f * (125 - Projectile.timeLeft) * (125 - Projectile.timeLeft) / 25f: 450f));
                    Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("Everglow/Ocean/Projectiles/海洋波纹~"), drawPos, new Rectangle(0, frameHeight * Projectile.frame, ModContent.Request<Texture2D>("Everglow/Ocean/Projectiles/海洋波纹~").Width(), frameHeight), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
                }
            }
            return true;
		}
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            int k = Player.FindClosest(Projectile.Center, 1, 1);
            return ((targetHitbox.TopLeft() + targetHitbox.Size() * 0.5f - Projectile.Center).Length() < 150);
        }
    }
}
