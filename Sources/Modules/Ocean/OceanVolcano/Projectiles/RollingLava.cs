using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace Everglow.Ocean.Projectiles.projectile3
{
    public class RollingLava : ModProjectile
	{
		public override void SetStaticDefaults()
		{
            // base.DisplayName.SetDefault("熔岩滚石");
		}
        private float num = 0;
        public override void SetDefaults()
		{
			base.Projectile.width = 100;
			base.Projectile.height = 100;
			base.Projectile.friendly = true;
            base.Projectile.hostile = true;
            base.Projectile.alpha = 0;
			base.Projectile.penetrate = 4;
			base.Projectile.tileCollide = true;
			base.Projectile.timeLeft = 900;
            base.Projectile.DamageType = DamageClass.Ranged;
            base.Projectile.aiStyle = 25;
		}
        float timer = 0;
        static float j = 0;
        static float m = 0.15f;
        static float n = 0;
        private bool x = false;
        Vector2 pc2 = Vector2.Zero;
        public override void AI()
        {
            if(Projectile.velocity.Length() >= 6)
            {
                Projectile.velocity *= 0.99f;
            }
            if(Projectile.velocity.Length() < 0.1f && Projectile.timeLeft < 895)
            {
                Projectile.Kill();
            }
            if(Main.rand.Next(3) == 2)
            {
                Dust.NewDust(new Vector2((float)Projectile.position.X, (float)Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 0, default(Color), Main.rand.NextFloat(0.9f, 1.6f));
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.rotation -= Projectile.velocity.X / 27f;
            if (base.Projectile.penetrate <= 0)
            {
                base.Projectile.Kill();
            }
            else
            {
                if(Projectile.velocity.Length() > 0.5f)
                {
                    if (base.Projectile.velocity.X != oldVelocity.X)
                    {
                        base.Projectile.velocity.X = -oldVelocity.X * 0.8f;
                    }
                    if (base.Projectile.velocity.Y != oldVelocity.Y)
                    {
                        base.Projectile.velocity.Y = -oldVelocity.Y * 0.4f;
                    }
                }
                else
                {
                    base.Projectile.velocity.Y *= 0;
                    base.Projectile.velocity.X *= 0;
                    x = true;
                }
            }
            return false;
        }
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, new Vector2(base.Projectile.position.X, base.Projectile.position.Y));
            for (int i = 0; i < 10; i++)
            {
                float scaleFactor5 = (float)(Main.rand.Next(-20, 20) / 100f);
                Gore.NewGore(base.Projectile.position, base.Projectile.velocity * scaleFactor5, base.Mod.GetGoreSlot("Gores/火山浮石碎块" + (i % 5 + 1).ToString()), 0.8f);
            }
            for (int i = 0; i < 5; i++)
            {
                float scaleFactor5 = (float)(Main.rand.Next(-20, 20) / 100f);
                Gore.NewGore(base.Projectile.position, base.Projectile.velocity * scaleFactor5, base.Mod.GetGoreSlot("Gores/火山浮石碎块" + (i % 2 + 6).ToString()), 0.8f);
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
		}
        public override void PostDraw(Color lightColor)
        {
            spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Ocean/Projectiles/projectile3/熔岩滚石Glow"), base.Projectile.Center - Main.screenPosition, null, new Color(255,255,255,0), base.Projectile.rotation, new Vector2(50f, 50f), 1f, SpriteEffects.None, 0f);
        }
    }
}
