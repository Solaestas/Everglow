using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Everglow.Myth.LanternMoon.Projectiles.DashCore;

namespace Everglow.Myth.Bosses.EvilBottle.Projectiles
{
    public class littleEvilfire0 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
            // DisplayName.SetDefault("影炸弹");
		}
		public override void SetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 12;
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
			Projectile.penetrate = 11;
			Projectile.timeLeft = 900;
            Projectile.extraUpdates = 96;
            Projectile.tileCollide = true;
        }
        private float Y = 0;
        private float X = 0;
        private float ω = 0;
        private bool Orbit = false;
        public override void AI()
		{
            Projectile.velocity.Y += 0.1f;
        }
        public override void Kill(int timeLeft)
        {
            if (!(timeLeft >= 896))
            {
				if (!(timeLeft <= 10))
				{
                    Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<PurpleFlame>(), 20, 0f, Main.myPlayer, Projectile.timeLeft / 200f * Main.rand.NextFloat(0.60f, 1.50f), 0f);
                }
            }
        }
    }
}
