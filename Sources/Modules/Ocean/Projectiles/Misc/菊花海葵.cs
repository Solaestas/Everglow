using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MythMod.Projectiles
{
	// Token: 0x0200057F RID: 1407
    public class 菊花海葵 : ModProjectile
	{
		// Token: 0x06001EC3 RID: 7875 RVA: 0x0000C81D File Offset: 0x0000AA1D
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("菊花海葵");
			Main.projFrames[base.projectile.type] = 4;
		}
        private bool A = true;
		// Token: 0x06001EC4 RID: 7876 RVA: 0x0018A990 File Offset: 0x00188B90
		public override void SetDefaults()
		{
			base.projectile.width = 32;
			base.projectile.height = 34;
			base.projectile.hostile = false;
			base.projectile.friendly = true;
			base.projectile.ignoreWater = true;
			base.projectile.tileCollide = true;
			base.projectile.penetrate = -1;
			base.projectile.timeLeft = 300;
			base.projectile.scale = 1f;
			this.cooldownSlot = 1;
		}
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture2D = Main.projectileTexture[base.projectile.type];
            int num17 = Main.projectileTexture[base.projectile.type].Height / Main.projFrames[base.projectile.type];
            int y = num17 * base.projectile.frame;
            Vector2 origin = new Vector2(16f, 17f);
            spriteBatch.Draw(base.mod.GetTexture("Projectiles/菊花海葵Glow"), base.projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, y, texture2D.Width, num17)), new Color(255,255,255,base.projectile.alpha), base.projectile.rotation, origin, base.projectile.scale, SpriteEffects.None, 0f);
        }
        // Token: 0x06001EC5 RID: 7877 RVA: 0x0018AA00 File Offset: 0x00188C00
        public override void AI()
		{
            if (base.projectile.timeLeft == 299)
            {
                int num = 36;
                for (int i = 0; i < num; i++)
                {
                    Vector2 vector = Vector2.Normalize(base.projectile.velocity) * new Vector2((float)base.projectile.width / 2f, (float)base.projectile.height) * 0.75f;
                    vector = Utils.RotatedBy(vector, (double)((float)(i - (num / 2 - 1)) * 6.28318548f / (float)num), default(Vector2)) + base.projectile.Center;
                    Vector2 vector2 = vector - base.projectile.Center;
                    int num2 = Dust.NewDust(vector + vector2, 0, 0, 174, vector2.X * 0.3f, vector2.Y * 0.3f, 100, default(Color), 1.4f);
                    Main.dust[num2].noGravity = true;
                }
            }
            if (base.projectile.timeLeft % 30 == 0 && !A)
            {
                Projectile.NewProjectile(base.projectile.Center.X, base.projectile.Center.Y - 5f, Main.rand.Next(-100, 100) / 150f, -5f, base.mod.ProjectileType("海葵粒子"), base.projectile.damage, base.projectile.knockBack, Main.myPlayer, 0f, 0f);
            }
            base.projectile.rotation = (float)Math.Atan2((double)base.projectile.velocity.Y, (double)base.projectile.velocity.X) + (float)Math.PI * 1.5f;
			base.projectile.frameCounter++;
			if (base.projectile.frameCounter > 4)
			{
				base.projectile.frame++;
				base.projectile.frameCounter = 0;
			}
			if (base.projectile.frame > 3)
			{
				base.projectile.frame = 0;
			}
			if (projectile.timeLeft < 30)
            {
                projectile.alpha += 9;
            }
			Lighting.AddLight(base.projectile.Center, (float)(255 - base.projectile.alpha) * 0.6f / 255f, (float)(255 - base.projectile.alpha) * 0.375f / 255f, (float)(255 - base.projectile.alpha) * 0f / 255f);
            if(A)
            {
                base.projectile.velocity.Y += 0.15f;
            }
            else
            {
                base.projectile.velocity = new Vector2(0, 0);
                base.projectile.rotation = 0;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            base.projectile.tileCollide = false;
            A = false;
            return false;
        }
    }
}
