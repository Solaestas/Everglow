using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Everglow.Myth.Common;

namespace Everglow.Myth.Bosses.EvilBottle.Projectiles
{
    public class PurpleRush : ModProjectile
	{
		public override void SetStaticDefaults()
		{
            // DisplayName.SetDefault("影炸弹");
		}
		public override void SetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 12;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
			Projectile.penetrate = 11;
			Projectile.timeLeft = 3600;
            Projectile.extraUpdates = 2;
            Projectile.tileCollide = true;
        }
        private float St = 0;
        private int BoomTime = 240;
        private float T = 0;
        private Vector2 vd = Vector2.Zero;
        private bool ON = false; 
        public override void AI()
		{
            base.Projectile.rotation = (float)Math.Atan2((double)base.Projectile.velocity.Y, (double)base.Projectile.velocity.X) - (float)Math.PI * 0.5f;
            if(Projectile.timeLeft == 3599)
            {
                vd = new Vector2(0, 1).RotatedByRandom(Math.PI * 2);
            }
            if (Projectile.timeLeft < 3599)
            {
                Vector2 vector = base.Projectile.Center;
                int num = Dust.NewDust(vector - new Vector2(4, 4), 2, 2, 86, 50f, 50f, 0, default(Color), (float)Projectile.scale * 1.2f * (1 + Projectile.ai[0] / 4f));
                Main.dust[num].velocity *= 0.0f;
                Main.dust[num].noGravity = true;
                Main.dust[num].alpha = 150;
            }
            if(BoomTime > 0)
            {
                T += 5f / (float)BoomTime + 0.1f;
            }
            /*if (projectile.velocity.Length() < 2.5f)
            {
                projectile.velocity += projectile.velocity / projectile.velocity.Length() * 0.04f;
            }
            */
            if(BoomTime > 0)
            {
                BoomTime -= 1;
            }
            else
            {
                if(!ON)
                {
                    ON = true;
                    Projectile.velocity += vd * Projectile.ai[0] * 8f;
                    Projectile.damage = 20;
                }
                BoomTime = 0;
            }
            Projectile.velocity *= 0.97f;
            if(Projectile.velocity.Length() < 0.97f && ON)
            {
                Projectile.Kill();
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if(BoomTime > 0)
            {
                int B = 0;
                if (T % 2 > 1)
                {
                    B = 1;
                }
                Main.spriteBatch.Draw(MythContent.QuickTexture("Bosses/EvilBottle/Projectiles/PurpleCircle"), Projectile.Center - Main.screenPosition, null, new Color(0.8f, 0.8f, 0.8f, 0) * B, 0f, new Vector2(26, 26), 1, SpriteEffects.None, 0f);
                int[] C = new int[8];
                for (int G = 0; G < 3; G++)
                {
                    if ((int)T % 8 == G)
                    {
                        C[G] = 1;
                    }
                    else
                    {
                        C[G] = 0;
                    }
                }
                for (int G = 0; G < Projectile.ai[0]; G++)
                {
                    Vector2 v = vd * 8 * (G + 1.5f);
                    Main.spriteBatch.Draw(MythContent.QuickTexture("Bosses/EvilBottle/Projectiles/PurpleArrow"), Projectile.Center - Main.screenPosition + v, null, new Color(0.8f, 0.8f, 0.8f, 0) * C[G], (float)Math.Atan2(vd.Y, vd.X), new Vector2(26, 26), 1, SpriteEffects.None, 0f);
                }
            }
            return false;
        }
        public override void Kill(int timeLeft)
        {
        }
    }
}
