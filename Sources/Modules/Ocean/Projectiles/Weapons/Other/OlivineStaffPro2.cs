using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MythMod.Projectiles.projectile2
{
    public class OlivineStaffPro2 : ModProjectile
	{
		public override void SetDefaults()
		{
			base.projectile.width = 16;
			base.projectile.height = 16;
			base.projectile.aiStyle = 1;
			base.projectile.alpha = 255;
			base.projectile.scale = 1f;
			base.projectile.friendly = false;
			base.projectile.magic = true;
			base.projectile.penetrate = 1;
			base.projectile.timeLeft = 90;
			this.aiType = 14;
		}
		public override void AI()
		{
			if(base.projectile.timeLeft <= 98 - projectile.ai[1])
			{
                if(projectile.timeLeft > 5)
                {
                    projectile.timeLeft = 4;
                }
                int num = Dust.NewDust(new Vector2(base.projectile.Center.X, base.projectile.Center.Y), 10, 10, mod.DustType("Olivine"), 0f, 0f, 100, default(Color), 1.5f);
                int num1 = Dust.NewDust(base.projectile.Center, 10, 10, mod.DustType("Olivine"), (float)Main.rand.Next(-130, 130) / 100f, (float)Main.rand.Next(-130, 130) / 100f, 0, default(Color), 1.5f);
                int num2 = Dust.NewDust(new Vector2(base.projectile.Center.X, base.projectile.Center.Y), 10, 10, mod.DustType("Olivine"), 0f, 0f, 100, default(Color), 1.5f);
                Dust dust = Main.dust[num];
                dust = Main.dust[num];
                dust = Main.dust[num1];
                dust = Main.dust[num2];
                dust.velocity *= 0.04f;
                Main.dust[num].noGravity = true;
                Main.dust[num1].noGravity = true;
                Main.dust[num2].noGravity = true;
                base.projectile.friendly = true;
            }
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 8; i++)
			{
				int num = Dust.NewDust(base.projectile.Center, base.projectile.width, base.projectile.height, mod.DustType("Olivine"), base.projectile.oldVelocity.X, base.projectile.oldVelocity.Y, 0, default(Color), 2.7f);
                int num1 = Dust.NewDust(base.projectile.Center, base.projectile.width, base.projectile.height, mod.DustType("Olivine"), base.projectile.oldVelocity.X, base.projectile.oldVelocity.Y, 0, default(Color), 2.6f);
                int num2 = Dust.NewDust(base.projectile.Center, base.projectile.width, base.projectile.height, mod.DustType("Olivine"), base.projectile.oldVelocity.X, base.projectile.oldVelocity.Y, 0, default(Color), 2.2f);
				Main.dust[num].noGravity = true;
                Main.dust[num1].noGravity = true;
                Main.dust[num2].noGravity = true;
			}
		}
	}
}
