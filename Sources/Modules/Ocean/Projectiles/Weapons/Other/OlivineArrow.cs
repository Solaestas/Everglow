using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Everglow.Ocean.Projectiles.projectile2
{
    public class OlivineArrow : ModProjectile
	{
		public override void SetStaticDefaults()
		{
            // // base.DisplayName.SetDefault("橄榄石箭");
		}
		public override void SetDefaults()
		{
			base.Projectile.width = 14;
			base.Projectile.height = 32;
			base.Projectile.friendly = true;
			base.Projectile.alpha = 65;
			base.Projectile.penetrate = 1;
			base.Projectile.tileCollide = true;
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
            int num3 = Dust.NewDust(base.Projectile.Center - base.Projectile.velocity - new Vector2(2, 2), 0, 0, ModContent.DustType<Everglow.Ocean.Dusts.Olivine>(), 0, 0, 0, default(Color), 1.6f);
            Main.dust[num3].noGravity = true;
            Main.dust[num3].velocity = new Vector2(0, 0);
            Lighting.AddLight(base.Projectile.Center, (float)(255 - base.Projectile.alpha) * 0.25f / 255f * Projectile.scale, (float)(255 - base.Projectile.alpha) * 0.65f * Projectile.scale / 255f, (float)(255 - base.Projectile.alpha) * 0f / 255f * Projectile.scale);
        }
        public override void Kill(int timeLeft)
        {
            for (int k = 0; k <= 15; k++)
            {
                float a = (float)Main.rand.Next(0, 720) / 360f * 3.141592653589793238f;
                float m = (float)Main.rand.Next(0, 50000);
                float l = (float)Main.rand.Next((int)m, 50000) / 1800f;
                int num4 = Projectile.NewProjectile(base.Projectile.Center.X, base.Projectile.Center.Y, (float)((float)l * Math.Cos((float)a)) * 0.16f, (float)((float)l * Math.Sin((float)a)) * 0.16f,ModContent.ProjectileType<Everglow.Ocean.Projectiles.OlivineShoot>(), base.Projectile.damage, base.Projectile.knockBack, base.Projectile.owner, 0f, 0f);
                Main.projectile[num4].scale = (float)Main.rand.Next(7000, 13000) / 10000f;
            }
            SoundEngine.PlaySound(SoundID.Item27, new Vector2(base.Projectile.position.X, base.Projectile.position.Y));
        }
    }
}
