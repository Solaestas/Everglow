using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Everglow.Myth.TheMarbleRemains.NPCs.Bosses.EvilBottle.Projectiles
{
    public class EvilCloud : ModProjectile
	{
		public override void SetStaticDefaults()
		{
            // DisplayName.SetDefault("魔雷云");
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
			Projectile.penetrate = -1;
			Projectile.timeLeft = 900;
            Projectile.extraUpdates = 12;
            Projectile.tileCollide = true;
        }
        private float Y = 0;
        private float X = 0;
        private float ω = 0;
        private bool Orbit = false;
        public override void AI()
		{
            if(Projectile.timeLeft >= 899)
            {
                Y = Projectile.Center.Y - 380;
                X = Projectile.Center.X;
            }
            if(Projectile.Center.Y > Y)
            {
                if(!Orbit)
                {
                    if (Math.Abs(ω) < 0.03f)
                    {
                        ω += Main.rand.NextFloat(-0.005f, 0.005f);
                    }
                    else
                    {
                        ω *= 0.96f;
                    }
                    if (Projectile.velocity.Y < 0.5f)
                    {
                        Projectile.velocity = Projectile.velocity.RotatedBy(ω);
                    }
                    else
                    {
                        Projectile.velocity.Y -= 0.05f;
                        Projectile.velocity.X *= 0.96f;
                    }
                }
            }
            else
            {
                Orbit = true;
            }
            if(Orbit)
            {
                float x = Projectile.Center.X - X;
                float y = Projectile.Center.Y - Y;
                if (x * x / 40000 + y * y / 2500 > 1)
                {
                    Projectile.velocity *= 0.99f;
                    Projectile.velocity += (new Vector2(X, Y) - Projectile.Center) / 2000f;
                }
                else
                {
                    Projectile.velocity = Projectile.velocity.RotatedBy(ω);
                    if (Math.Abs(ω) < 0.03f)
                    {
                        ω += Main.rand.NextFloat(-0.005f, 0.005f);
                    }
                    else
                    {
                        ω *= 0.96f;
                    }
                    if(Projectile.velocity.Y > 0.5f)
                    {
                        Projectile.velocity.Y *= 0.98f;
                    }
                    if (Projectile.velocity.Length() < 2f)
                    {
                        Projectile.velocity *= 1.05f;
                    }
                }
            }
            if(Projectile.timeLeft == 60)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<EvilLightingbolt>(), 30, 0f, Main.myPlayer, 0f, 0f);
            }
            if(Main.rand.Next(100) < 3)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<EvilLightingbolt2>(), 0, 0f, Main.myPlayer, Main.rand.Next(8, 60), 0f);
            }
            int num = Dust.NewDust(Projectile.Center - new Vector2(4, 4) + new Vector2(0, 12).RotatedBy(Projectile.timeLeft / 4f), 2, 2, 109, 0, 0, 0, default(Color), Main.rand.NextFloat(2.5f, 5f));
            Main.dust[num].noGravity = true;
            Main.dust[num].velocity = Projectile.velocity;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            base.Projectile.ai[0] += 0.1f;
            if (base.Projectile.velocity.X != oldVelocity.X)
            {
                base.Projectile.velocity.X = -oldVelocity.X;
            }
            if (base.Projectile.velocity.Y != oldVelocity.Y)
            {
                base.Projectile.velocity.Y = -oldVelocity.Y;
            }
            return false;
        }
        public override void Kill(int timeLeft)
        {
            if (timeLeft > 60)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<EvilLightingbolt>(), 30, 0f, Main.myPlayer, 0f, 0f);
            }
        }
    }
}
