using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MythMod.Projectiles.projectile3
{
    public class SeadraStaff : ModProjectile
	{
		public override void SetDefaults()
		{
			base.projectile.width = 16;
			base.projectile.height = 16;
			base.projectile.alpha = 255;
			base.projectile.scale = 1f;
			base.projectile.friendly = true;
            base.projectile.hostile = false;
            base.projectile.magic = true;
			base.projectile.penetrate = 1;
			base.projectile.timeLeft = 3600;
            base.projectile.ignoreWater = false;
            base.projectile.tileCollide = true;
        }
		public override void AI()
		{
            int num = Dust.NewDust(new Vector2(base.projectile.position.X, base.projectile.position.Y), base.projectile.width, base.projectile.height, 33, 0f, 0f, 100, Color.White, 2f);
            if (projectile.wet)
            {
                projectile.timeLeft = 0;
            }
		}
		public override void Kill(int timeLeft)
		{
            MythPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<MythPlayer>();
            int num = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("Storm"), projectile.damage, 10f, Main.myPlayer, 10f, 25f);
            Main.projectile[num].hostile = false;
            Main.projectile[num].friendly = true;
            for (int i = 0; i < 65; i++)
            {
                Vector2 v = new Vector2(0, Main.rand.NextFloat(0.5f, 4f)).RotatedByRandom(Math.PI * 2);
                int num5 = Dust.NewDust(new Vector2(base.projectile.position.X, base.projectile.position.Y), base.projectile.width, base.projectile.height, 33, 0f, 0f, 100, Color.White, 2f);
            }
        }
	}
}
