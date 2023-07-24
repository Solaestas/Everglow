using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MythMod.Projectiles.projectile2
{
    public class OlivineArrow : ModProjectile
	{
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("橄榄石箭");
		}
		public override void SetDefaults()
		{
			base.projectile.width = 14;
			base.projectile.height = 32;
			base.projectile.friendly = true;
			base.projectile.alpha = 65;
			base.projectile.penetrate = 1;
			base.projectile.tileCollide = true;
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
            int num3 = Dust.NewDust(base.projectile.Center - base.projectile.velocity - new Vector2(2, 2), 0, 0, mod.DustType("Olivine"), 0, 0, 0, default(Color), 1.6f);
            Main.dust[num3].noGravity = true;
            Main.dust[num3].velocity = new Vector2(0, 0);
            Lighting.AddLight(base.projectile.Center, (float)(255 - base.projectile.alpha) * 0.25f / 255f * projectile.scale, (float)(255 - base.projectile.alpha) * 0.65f * projectile.scale / 255f, (float)(255 - base.projectile.alpha) * 0f / 255f * projectile.scale);
        }
        public override void Kill(int timeLeft)
        {
            for (int k = 0; k <= 15; k++)
            {
                float a = (float)Main.rand.Next(0, 720) / 360f * 3.141592653589793238f;
                float m = (float)Main.rand.Next(0, 50000);
                float l = (float)Main.rand.Next((int)m, 50000) / 1800f;
                int num4 = Projectile.NewProjectile(base.projectile.Center.X, base.projectile.Center.Y, (float)((float)l * Math.Cos((float)a)) * 0.16f, (float)((float)l * Math.Sin((float)a)) * 0.16f, base.mod.ProjectileType("OlivineShoot"), base.projectile.damage, base.projectile.knockBack, base.projectile.owner, 0f, 0f);
                Main.projectile[num4].scale = (float)Main.rand.Next(7000, 13000) / 10000f;
            }
            Main.PlaySound(2, (int)base.projectile.position.X, (int)base.projectile.position.Y, 27, 1f, 0f);
        }
    }
}
