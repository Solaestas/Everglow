using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Everglow.Ocean.Projectiles.projectile3
{
    public class SeadraStaff : ModProjectile
	{
		public override void SetDefaults()
		{
			base.Projectile.width = 16;
			base.Projectile.height = 16;
			base.Projectile.alpha = 255;
			base.Projectile.scale = 1f;
			base.Projectile.friendly = true;
            base.Projectile.hostile = false;
            base.Projectile.DamageType = DamageClass.Magic;
			base.Projectile.penetrate = 1;
			base.Projectile.timeLeft = 3600;
            base.Projectile.ignoreWater = false;
            base.Projectile.tileCollide = true;
        }
		public override void AI()
		{
            int num = Dust.NewDust(new Vector2(base.Projectile.position.X, base.Projectile.position.Y), base.Projectile.width, base.Projectile.height, 33, 0f, 0f, 100, Color.White, 2f);
            if (Projectile.wet)
            {
                Projectile.timeLeft = 0;
            }
		}
		public override void Kill(int timeLeft)
		{
            OceanContentPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<OceanContentPlayer>();
            int num = Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<Everglow.Ocean.Projectiles.Storm>(), Projectile.damage, 10f, Main.myPlayer, 10f, 25f);
            Main.projectile[num].hostile = false;
            Main.projectile[num].friendly = true;
            for (int i = 0; i < 65; i++)
            {
                Vector2 v = new Vector2(0, Main.rand.NextFloat(0.5f, 4f)).RotatedByRandom(Math.PI * 2);
                int num5 = Dust.NewDust(new Vector2(base.Projectile.position.X, base.Projectile.position.Y), base.Projectile.width, base.Projectile.height, 33, 0f, 0f, 100, Color.White, 2f);
            }
        }
	}
}
