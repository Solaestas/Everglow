using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MythMod.Projectiles.projectile2
{
    public class OlivineShoot : ModProjectile
	{
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("橄榄石箭");
		}
		public override void SetDefaults()
		{
			base.projectile.width = 14;
			base.projectile.height = 14;
			base.projectile.friendly = true;
			base.projectile.alpha = 65;
			base.projectile.penetrate = -1;
			base.projectile.tileCollide = false;
			base.projectile.timeLeft = 300;
            base.projectile.ranged = true;
            base.projectile.aiStyle = 1;
            this.aiType = 1;
		}
        float timer = 0;
        static float j = 0;
        static float m = 0;
        static float n = 0;
        Vector2 pc2 = Vector2.Zero;
        public override void AI()
        {
            float m = (150 - projectile.timeLeft);
            m *= m;
            m = 22500 - m;
            m /= 18000;
            projectile.timeLeft -= 2;
            int num3 = Dust.NewDust(base.projectile.Center - base.projectile.velocity - new Vector2(2, 2), 0, 0, mod.DustType("Olivine"), 0, 0, 0, default(Color), m);
            Main.dust[num3].noGravity = true;
            Main.dust[num3].velocity = new Vector2(0, 0);
            Lighting.AddLight(base.projectile.Center, (float)(255 - base.projectile.alpha) * 0.25f / 255f * projectile.scale, (float)(255 - base.projectile.alpha) * 0.65f * projectile.scale / 255f, (float)(255 - base.projectile.alpha) * 0f / 255f * projectile.scale);
            projectile.velocity *= 0.99f;
        }
    }
}
