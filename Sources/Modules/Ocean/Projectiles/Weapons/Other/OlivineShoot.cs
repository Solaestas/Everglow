using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Everglow.Ocean.Projectiles
{
    public class OlivineShoot : ModProjectile
	{
		public override void SetStaticDefaults()
		{
            // base.DisplayName.SetDefault("橄榄石箭");
		}
		public override void SetDefaults()
		{
			base.Projectile.width = 14;
			base.Projectile.height = 14;
			base.Projectile.friendly = true;
			base.Projectile.alpha = 65;
			base.Projectile.penetrate = -1;
			base.Projectile.tileCollide = false;
			base.Projectile.timeLeft = 300;
            base.Projectile.DamageType = DamageClass.Ranged;
            base.Projectile.aiStyle = 1;
            this.AIType = 1;
		}
        float timer = 0;
        static float j = 0;
        static float m = 0;
        static float n = 0;
        Vector2 pc2 = Vector2.Zero;
        public override void AI()
        {
            float m = (150 - Projectile.timeLeft);
            m *= m;
            m = 22500 - m;
            m /= 18000;
            Projectile.timeLeft -= 2;
            int num3 = Dust.NewDust(base.Projectile.Center - base.Projectile.velocity - new Vector2(2, 2), 0, 0, ModContent.DustType<Everglow.Ocean.Dusts.Olivine>(), 0, 0, 0, default(Color), m);
            Main.dust[num3].noGravity = true;
            Main.dust[num3].velocity = new Vector2(0, 0);
            Lighting.AddLight(base.Projectile.Center, (float)(255 - base.Projectile.alpha) * 0.25f / 255f * Projectile.scale, (float)(255 - base.Projectile.alpha) * 0.65f * Projectile.scale / 255f, (float)(255 - base.Projectile.alpha) * 0f / 255f * Projectile.scale);
            Projectile.velocity *= 0.99f;
        }
    }
}
