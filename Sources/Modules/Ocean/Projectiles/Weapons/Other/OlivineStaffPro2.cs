using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Everglow.Ocean.Projectiles.projectile2
{
    public class OlivineStaffPro2 : ModProjectile
	{
		public override void SetDefaults()
		{
			base.Projectile.width = 16;
			base.Projectile.height = 16;
			base.Projectile.aiStyle = 1;
			base.Projectile.alpha = 255;
			base.Projectile.scale = 1f;
			base.Projectile.friendly = false;
			base.Projectile.DamageType = DamageClass.Magic;
			base.Projectile.penetrate = 1;
			base.Projectile.timeLeft = 90;
			this.AIType = 14;
		}
		public override void AI()
		{
			if(base.Projectile.timeLeft <= 98 - Projectile.ai[1])
			{
                if(Projectile.timeLeft > 5)
                {
                    Projectile.timeLeft = 4;
                }
                int num = Dust.NewDust(new Vector2(base.Projectile.Center.X, base.Projectile.Center.Y), 10, 10, ModContent.DustType<Everglow.Ocean.Dusts.Olivine>(), 0f, 0f, 100, default(Color), 1.5f);
                int num1 = Dust.NewDust(base.Projectile.Center, 10, 10, ModContent.DustType<Everglow.Ocean.Dusts.Olivine>(), (float)Main.rand.Next(-130, 130) / 100f, (float)Main.rand.Next(-130, 130) / 100f, 0, default(Color), 1.5f);
                int num2 = Dust.NewDust(new Vector2(base.Projectile.Center.X, base.Projectile.Center.Y), 10, 10, ModContent.DustType<Everglow.Ocean.Dusts.Olivine>(), 0f, 0f, 100, default(Color), 1.5f);
                Dust dust = Main.dust[num];
                dust = Main.dust[num];
                dust = Main.dust[num1];
                dust = Main.dust[num2];
                dust.velocity *= 0.04f;
                Main.dust[num].noGravity = true;
                Main.dust[num1].noGravity = true;
                Main.dust[num2].noGravity = true;
                base.Projectile.friendly = true;
            }
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 8; i++)
			{
				int num = Dust.NewDust(base.Projectile.Center, base.Projectile.width, base.Projectile.height, ModContent.DustType<Everglow.Ocean.Dusts.Olivine>(), base.Projectile.oldVelocity.X, base.Projectile.oldVelocity.Y, 0, default(Color), 2.7f);
                int num1 = Dust.NewDust(base.Projectile.Center, base.Projectile.width, base.Projectile.height, ModContent.DustType<Everglow.Ocean.Dusts.Olivine>(), base.Projectile.oldVelocity.X, base.Projectile.oldVelocity.Y, 0, default(Color), 2.6f);
                int num2 = Dust.NewDust(base.Projectile.Center, base.Projectile.width, base.Projectile.height, ModContent.DustType<Everglow.Ocean.Dusts.Olivine>(), base.Projectile.oldVelocity.X, base.Projectile.oldVelocity.Y, 0, default(Color), 2.2f);
				Main.dust[num].noGravity = true;
                Main.dust[num1].noGravity = true;
                Main.dust[num2].noGravity = true;
			}
		}
	}
}
