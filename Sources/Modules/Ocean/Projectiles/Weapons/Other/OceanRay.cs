using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MythMod.Projectiles.projectile2
{
    public class OceanRay : ModProjectile
	{
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("海洋射线");
		}
		public override void SetDefaults()
		{
			base.projectile.width = 6;
			base.projectile.height = 12;
            projectile.hostile = false;
            projectile.friendly = true;
            base.projectile.ignoreWater = true;
			base.projectile.tileCollide = false;
			base.projectile.alpha = 0;
			base.projectile.penetrate = 1;
			base.projectile.timeLeft = 60;
		}
		public override void AI()
		{
            if (base.projectile.ai[0] == 0f)
            {
                base.projectile.ai[0] = 1f;
            }
            float num = projectile.timeLeft;
            float num2 = 1.5f;
            if(base.projectile.localAI[0] > num)
            {
                projectile.ai[1] = 1;
            }
            if (base.projectile.ai[1] == 0f)
            {
                base.projectile.localAI[0] += num2;
                if (base.projectile.localAI[0] > num)
                {
                    base.projectile.localAI[0] = num;
                    projectile.ai[1] = 1;
                }
            }
            else
            {
                base.projectile.localAI[0] -= num2;
                if (base.projectile.localAI[0] <= 0f)
                {
                    base.projectile.Kill();
                }
            }
            //projectile.timeLeft -= 1;
            if (projectile.timeLeft <= 0)
            {
                base.projectile.Kill();
            }
            Lighting.AddLight(base.projectile.Center, (float)(255 - base.projectile.alpha) * 0.5f / 255f, (float)(255 - base.projectile.alpha) * 0.2f / 255f, (float)(255 - base.projectile.alpha) * 0f / 255f);
		}
        public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(new Color(255, 255, 255, 0));
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Color color = Lighting.GetColor((int)((double)base.projectile.position.X + (double)base.projectile.width * 0.5) / 16, (int)(((double)base.projectile.position.Y + (double)base.projectile.height * 0.5) / 16.0));
            int num = 0;
            int num2 = 0;
            float num3 = (float)(Main.projectileTexture[base.projectile.type].Width - base.projectile.width) * 0.5f + (float)base.projectile.width * 0.5f;
            SpriteEffects effects = SpriteEffects.None;
            if (base.projectile.spriteDirection == -1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Rectangle value = new Rectangle((int)Main.screenPosition.X - 500, (int)Main.screenPosition.Y - 500, Main.screenWidth + 1000, Main.screenHeight + 1000);
            if (base.projectile.getRect().Intersects(value))
            {
                Vector2 value2 = new Vector2(base.projectile.position.X - Main.screenPosition.X + num3 + (float)num2, base.projectile.position.Y - Main.screenPosition.Y + (float)(base.projectile.height / 2) + base.projectile.gfxOffY);
                float num4 = 25f;
                if (base.projectile.timeLeft < 60)
                {
                    num4 = 25f / 60f * (float)base.projectile.timeLeft;
                }
                float scaleFactor = 1.5f;
                if (base.projectile.ai[1] == 1f)
                {
                    num4 = (float)((int)base.projectile.localAI[0]);
                }
                for (int i = 1; i <= (int)base.projectile.localAI[0]; i++)
                {
                    Vector2 value3 = Vector2.Normalize(base.projectile.velocity) * (float)i * scaleFactor;
                    Color color2 = base.projectile.GetAlpha(color);
                    color2 *= (num4 - (float)i) / num4;
                    color2.A = 0;
                    Main.spriteBatch.Draw(Main.projectileTexture[base.projectile.type], value2 - value3, null, color2, base.projectile.rotation, new Vector2(num3, (float)(base.projectile.height / 2 + num)), base.projectile.scale, effects, 0f);
                }
            }
            return false;
        }
    }
}
