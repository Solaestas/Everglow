using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace MythMod.Projectiles.projectile3
{
    public class RollingLava : ModProjectile
	{
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("熔岩滚石");
		}
        private float num = 0;
        public override void SetDefaults()
		{
			base.projectile.width = 100;
			base.projectile.height = 100;
			base.projectile.friendly = true;
            base.projectile.hostile = true;
            base.projectile.alpha = 0;
			base.projectile.penetrate = 4;
			base.projectile.tileCollide = true;
			base.projectile.timeLeft = 900;
            base.projectile.ranged = true;
            base.projectile.aiStyle = 25;
		}
        float timer = 0;
        static float j = 0;
        static float m = 0.15f;
        static float n = 0;
        private bool x = false;
        Vector2 pc2 = Vector2.Zero;
        public override void AI()
        {
            if(projectile.velocity.Length() >= 6)
            {
                projectile.velocity *= 0.99f;
            }
            if(projectile.velocity.Length() < 0.1f && projectile.timeLeft < 895)
            {
                projectile.Kill();
            }
            if(Main.rand.Next(3) == 2)
            {
                Dust.NewDust(new Vector2((float)projectile.position.X, (float)projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 0, default(Color), Main.rand.NextFloat(0.9f, 1.6f));
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.rotation -= projectile.velocity.X / 27f;
            if (base.projectile.penetrate <= 0)
            {
                base.projectile.Kill();
            }
            else
            {
                if(projectile.velocity.Length() > 0.5f)
                {
                    if (base.projectile.velocity.X != oldVelocity.X)
                    {
                        base.projectile.velocity.X = -oldVelocity.X * 0.8f;
                    }
                    if (base.projectile.velocity.Y != oldVelocity.Y)
                    {
                        base.projectile.velocity.Y = -oldVelocity.Y * 0.4f;
                    }
                }
                else
                {
                    base.projectile.velocity.Y *= 0;
                    base.projectile.velocity.X *= 0;
                    x = true;
                }
            }
            return false;
        }
        public override void Kill(int timeLeft)
        {
            Main.PlaySound(2, (int)base.projectile.position.X, (int)base.projectile.position.Y, 14, 1f, 0f);
            for (int i = 0; i < 10; i++)
            {
                float scaleFactor5 = (float)(Main.rand.Next(-20, 20) / 100f);
                Gore.NewGore(base.projectile.position, base.projectile.velocity * scaleFactor5, base.mod.GetGoreSlot("Gores/火山浮石碎块" + (i % 5 + 1).ToString()), 0.8f);
            }
            for (int i = 0; i < 5; i++)
            {
                float scaleFactor5 = (float)(Main.rand.Next(-20, 20) / 100f);
                Gore.NewGore(base.projectile.position, base.projectile.velocity * scaleFactor5, base.mod.GetGoreSlot("Gores/火山浮石碎块" + (i % 2 + 6).ToString()), 0.8f);
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
		}
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            spriteBatch.Draw(base.mod.GetTexture("Projectiles/projectile3/熔岩滚石Glow"), base.projectile.Center - Main.screenPosition, null, new Color(255,255,255,0), base.projectile.rotation, new Vector2(50f, 50f), 1f, SpriteEffects.None, 0f);
        }
    }
}
