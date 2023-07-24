﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MythMod.Projectiles.projectile3
{
    public class YellowSponge : ModProjectile
	{
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("黄色海绵喷流");
        }
		public override void SetDefaults()
		{
			base.projectile.width = 20;
			base.projectile.height = 20;
			base.projectile.friendly = true;
			base.projectile.melee = true;
			base.projectile.penetrate = 1;
			base.projectile.aiStyle = -1;
			base.projectile.timeLeft = 300;
            base.projectile.hostile = false;
            projectile.extraUpdates = 3;
		}
        private float omega = 0;
		public override void AI()
		{
			base.projectile.localAI[0] += 1f;
            if (Math.Abs(omega) < 0.07f)
            {
                omega += Main.rand.NextFloat(-0.005f, 0.005f);
            }
            else
            {
                omega *= 0.99f;
            }
            projectile.velocity = projectile.velocity.RotatedBy(omega * 2 * Math.Sin(projectile.localAI[0] * 0.05));
            if (projectile.timeLeft >= 60)
            {
                if (base.projectile.localAI[0] > 6f)
                {
                    int num = Dust.NewDust(new Vector2(base.projectile.position.X, base.projectile.position.Y), 0, 0, 87, 0f, 0f, 100, Color.Black, 1.333f);
                    Main.dust[num].noGravity = true;
                    Main.dust[num].velocity *= 0f;
                    for (int i = 0; i < 6; i++)
                    {
                        float k = Main.rand.NextFloat(0, 4f);
                        Vector2 v = new Vector2(0, k).RotatedByRandom(MathHelper.TwoPi);
                        int num2 = Dust.NewDust(new Vector2(base.projectile.position.X + v.X, base.projectile.position.Y + v.Y), 0, 0, 87, 0f, 0f, 100, Color.Black, 1f / (k + 0.75f));
                        Main.dust[num2].noGravity = true;
                        Main.dust[num2].velocity *= 0f;
                    }
                }
            }
            else
            {
                if (base.projectile.localAI[0] > 6f)
                {
                    int num = Dust.NewDust(new Vector2(base.projectile.position.X, base.projectile.position.Y), 0, 0, 87, 0f, 0f, 100, Color.Black, 1.333f * projectile.timeLeft / 60f);
                    Main.dust[num].noGravity = true;
                    Main.dust[num].velocity *= 0f;
                    for (int i = 0; i < 6; i++)
                    {
                        float k = Main.rand.NextFloat(0, 4f);
                        Vector2 v = new Vector2(0, k).RotatedByRandom(MathHelper.TwoPi);
                        int num2 = Dust.NewDust(new Vector2(base.projectile.position.X + v.X, base.projectile.position.Y + v.Y), 0, 0, 87, 0f, 0f, 100, Color.Black, 1f / (k + 0.75f) * projectile.timeLeft / 60f);
                        Main.dust[num2].noGravity = true;
                        Main.dust[num2].velocity *= 0f;
                    }
                }
            }
		}
	}
}
