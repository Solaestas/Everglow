using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace MythMod.Projectiles.projectile2
{
	// Token: 0x0200058D RID: 1421
    public class OceanWave2 : ModProjectile
	{
		// Token: 0x06001F14 RID: 7956 RVA: 0x0000C97C File Offset: 0x0000AB7C
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("海洋波纹");
			Main.projFrames[base.projectile.type] = 1;
		}
        private Vector2 v3 = new Vector2(0, 0);
        // Token: 0x06001F15 RID: 7957 RVA: 0x0018D09C File Offset: 0x0018B29C
        public override void SetDefaults()
		{
			base.projectile.width = 40;
			base.projectile.height = 40;
			base.projectile.hostile = false;
			base.projectile.ignoreWater = true;
			base.projectile.tileCollide = false;
			base.projectile.penetrate = -1;
			base.projectile.timeLeft = 450;
			base.projectile.alpha = 0;
            base.projectile.friendly = true;
			this.cooldownSlot = 1;
			ProjectileID.Sets.TrailCacheLength[base.projectile.type] = 20;
            ProjectileID.Sets.TrailingMode[base.projectile.type] = 0;
		}
		// Token: 0x06001F16 RID: 7958 RVA: 0x0018D118 File Offset: 0x0018B318
		public override void AI()
		{
			Lighting.AddLight(base.projectile.Center, (float)(255 - base.projectile.alpha) * 0.0f / 255f * base.projectile.scale, (float)(255 - base.projectile.alpha) * 0.3f / 255f * base.projectile.scale, (float)(255 - base.projectile.alpha) * 1f / 255f);
			base.projectile.spriteDirection = 1;
			base.projectile.rotation = (float)Math.Atan2((double)base.projectile.velocity.Y, (double)base.projectile.velocity.X);
			base.projectile.velocity *= 0.99f;
		}

		// Token: 0x06001F17 RID: 7959 RVA: 0x0000C841 File Offset: 0x0000AA41
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(new Color(255, 255, 255, base.projectile.alpha));
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture2D = Main.projectileTexture[base.projectile.type];
            int num = Main.projectileTexture[base.projectile.type].Height;
			Main.spriteBatch.Draw(texture2D, base.projectile.Center - Main.screenPosition + new Vector2(0f, base.projectile.gfxOffY), new Rectangle?(new Rectangle(0, 0, texture2D.Width, num)), base.projectile.GetAlpha(lightColor), base.projectile.rotation, new Vector2((float)texture2D.Width / 2f, (float)num / 2f), base.projectile.scale, SpriteEffects.None, 1f);
			SpriteEffects effects = SpriteEffects.None;
			if (base.projectile.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            if(projectile.timeLeft == 449)
            {
                v3 = projectile.Center;
            }
			int frameHeight = 10;
            if (projectile.timeLeft < 400)
            {
                frameHeight = (int)(10 + (400 - projectile.timeLeft) / 40f * 3f);
            }
            Vector2 value = new Vector2(base.projectile.Center.X, base.projectile.Center.Y);
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            Vector2 vector2 = value - Main.screenPosition;
			if(projectile.timeLeft > 10)
			{
                float num100 = 2 / (v3 - projectile.Center).Length();
                for (int k = 0; k < 60; k++)
                {
                    Vector2 v = (projectile.Center - v3) * num100;
                    v = v.RotatedBy(-k / 150f * num100 * 200f + (float)Math.PI / 2) * k;
                    Vector2 v2 = (projectile.Center - v3) * num100 * 1.2f * (float)Math.Sin(projectile.timeLeft / 10f + 0.25f * k) * ((60 - k) / 30f);
                    Vector2 drawPos = projectile.Center + v2 - v - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                    Color color = new Color(100 * projectile.timeLeft / 24f + 155, 100 * projectile.timeLeft / 24f + 155, 100 * projectile.timeLeft / 24f + 155, base.projectile.alpha) * ((60 - k) / 60f) * (projectile.timeLeft / (projectile.timeLeft < 120 ? 450f * (125 - projectile.timeLeft) * (125 - projectile.timeLeft) / 25f : 450f));
                    spriteBatch.Draw(base.mod.GetTexture("Projectiles/海洋波纹~"), drawPos, new Rectangle(0, frameHeight * projectile.frame, base.mod.GetTexture("Projectiles/海洋波纹~").Width, frameHeight), color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
                }
                for (int k = 0; k < 60; k++)
                {
                    Vector2 v = (projectile.Center - v3) * num100;
                    v = v.RotatedBy(k / 150f * num100 * 200f + (float)Math.PI / 2) * k;
                    Vector2 v2 = (projectile.Center - v3) * num100 * 1.2f * (float)Math.Sin(projectile.timeLeft / 10f + 0.25f * -k) * ((60 - k) / 30f);
                    Vector2 drawPos = projectile.Center + v2 + v - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                    Color color = new Color(100 * projectile.timeLeft / 24f + 155, 100 * projectile.timeLeft / 24f + 155, 100 * projectile.timeLeft / 24f + 155, base.projectile.alpha) * ((60 - k) / 60f) * (projectile.timeLeft / (projectile.timeLeft < 120 ? 450f * (125 - projectile.timeLeft) * (125 - projectile.timeLeft) / 25f: 450f));
                    spriteBatch.Draw(base.mod.GetTexture("Projectiles/海洋波纹~"), drawPos, new Rectangle(0, frameHeight * projectile.frame, base.mod.GetTexture("Projectiles/海洋波纹~").Width, frameHeight), color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
                }
            }
            return true;
		}
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return ((targetHitbox.TopLeft() + targetHitbox.Size() * 0.5f - projectile.Center).Length() < 150);
        }
    }
}
