using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Everglow.Myth.TheMarbleRemains.NPCs.Bosses.EvilBottle.Projectiles
{
    public class DarkBoom0 : ModProjectile
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
            Projectile.extraUpdates = 12;
            Projectile.tileCollide = true;
        }
        private float Y = 0;
        private float X = 0;
        private float ω = 0;
        private bool Orbit = false;
        public override void AI()
		{
        }
        public override void Kill(int timeLeft)
        {
            if (!(timeLeft >= 896))
            {
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<EvilBoom>(), 90, 0f, Main.myPlayer, 0f, 0f);
            }
        }
    }
}
