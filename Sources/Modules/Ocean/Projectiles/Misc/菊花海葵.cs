using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace Everglow.Ocean.Projectiles
{
	// Token: 0x0200057F RID: 1407
    public class 菊花海葵 : ModProjectile
	{
		// Token: 0x06001EC3 RID: 7875 RVA: 0x0000C81D File Offset: 0x0000AA1D
		public override void SetStaticDefaults()
		{
            // // base.DisplayName.SetDefault("菊花海葵");
			Main.projFrames[base.Projectile.type] = 4;
		}
        private bool A = true;
		// Token: 0x06001EC4 RID: 7876 RVA: 0x0018A990 File Offset: 0x00188B90
		public override void SetDefaults()
		{
			base.Projectile.width = 32;
			base.Projectile.height = 34;
			base.Projectile.hostile = false;
			base.Projectile.friendly = true;
			base.Projectile.ignoreWater = true;
			base.Projectile.tileCollide = true;
			base.Projectile.penetrate = -1;
			base.Projectile.timeLeft = 300;
			base.Projectile.scale = 1f;
			this.CooldownSlot = 1;
		}
        public override void PostDraw(Color lightColor)
        {
            Texture2D texture2D = TextureAssets.Projectile[base.Projectile.type].Value;
            int num17 = TextureAssets.Projectile[base.Projectile.type].Value.Height / Main.projFrames[base.Projectile.type];
            int y = num17 * base.Projectile.frame;
            Vector2 origin = new Vector2(16f, 17f);
            spriteBatch.Draw(base.Mod.GetTexture("Projectiles/菊花海葵Glow"), base.Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, y, texture2D.Width, num17)), new Color(255,255,255,base.Projectile.alpha), base.Projectile.rotation, origin, base.Projectile.scale, SpriteEffects.None, 0f);
        }
        // Token: 0x06001EC5 RID: 7877 RVA: 0x0018AA00 File Offset: 0x00188C00
        public override void AI()
		{
            if (base.Projectile.timeLeft == 299)
            {
                int num = 36;
                for (int i = 0; i < num; i++)
                {
                    Vector2 vector = Vector2.Normalize(base.Projectile.velocity) * new Vector2((float)base.Projectile.width / 2f, (float)base.Projectile.height) * 0.75f;
                    vector = Utils.RotatedBy(vector, (double)((float)(i - (num / 2 - 1)) * 6.28318548f / (float)num), default(Vector2)) + base.Projectile.Center;
                    Vector2 vector2 = vector - base.Projectile.Center;
                    int num2 = Dust.NewDust(vector + vector2, 0, 0, 174, vector2.X * 0.3f, vector2.Y * 0.3f, 100, default(Color), 1.4f);
                    Main.dust[num2].noGravity = true;
                }
            }
            if (base.Projectile.timeLeft % 30 == 0 && !A)
            {
                Projectile.NewProjectile(base.Projectile.Center.X, base.Projectile.Center.Y - 5f, Main.rand.Next(-100, 100) / 150f, -5f,ModContent.ProjectileType<Everglow.Ocean.Projectiles.海葵粒子>(), base.Projectile.damage, base.Projectile.knockBack, Main.myPlayer, 0f, 0f);
            }
            base.Projectile.rotation = (float)Math.Atan2((double)base.Projectile.velocity.Y, (double)base.Projectile.velocity.X) + (float)Math.PI * 1.5f;
			base.Projectile.frameCounter++;
			if (base.Projectile.frameCounter > 4)
			{
				base.Projectile.frame++;
				base.Projectile.frameCounter = 0;
			}
			if (base.Projectile.frame > 3)
			{
				base.Projectile.frame = 0;
			}
			if (Projectile.timeLeft < 30)
            {
                Projectile.alpha += 9;
            }
			Lighting.AddLight(base.Projectile.Center, (float)(255 - base.Projectile.alpha) * 0.6f / 255f, (float)(255 - base.Projectile.alpha) * 0.375f / 255f, (float)(255 - base.Projectile.alpha) * 0f / 255f);
            if(A)
            {
                base.Projectile.velocity.Y += 0.15f;
            }
            else
            {
                base.Projectile.velocity = new Vector2(0, 0);
                base.Projectile.rotation = 0;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            base.Projectile.tileCollide = false;
            A = false;
            return false;
        }
    }
}
