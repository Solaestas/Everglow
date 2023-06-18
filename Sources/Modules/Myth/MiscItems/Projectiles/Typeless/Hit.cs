using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Everglow.Myth.MiscItems.Projectiles.Typeless
{
    public class Hit : ModProjectile
	{
		public override void SetStaticDefaults()
		{
            // DisplayName.SetDefault("伤害");
		}
		public override void SetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 12;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 0;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 1;
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
        }
    }
}
