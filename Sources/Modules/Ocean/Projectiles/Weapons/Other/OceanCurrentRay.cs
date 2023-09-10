using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Everglow.Ocean.Projectiles
{
	public class OceanCurrentRay : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// base.DisplayName.SetDefault("洋流射线");
		}
		public override void SetDefaults()
		{
			base.Projectile.width = 14;
			base.Projectile.height = 14;
			base.Projectile.friendly = true;
			base.Projectile.alpha = 255;
			base.Projectile.penetrate = 5;
			base.Projectile.timeLeft = 1080;
			base.Projectile.DamageType = DamageClass.Magic;
            Projectile.extraUpdates = 100;

        }
		public override void AI()
		{
			Projectile projectile = base.Projectile;
			projectile.velocity.X = projectile.velocity.X * 1.01f;
			Projectile projectile2 = base.Projectile;
			projectile2.velocity.Y = projectile2.velocity.Y * 1.01f;
			if (base.Projectile.ai[0] == 0f)
			{
				base.Projectile.ai[0] = base.Projectile.velocity.X;
				base.Projectile.ai[1] = base.Projectile.velocity.Y;
			}
			if (Math.Sqrt((double)(base.Projectile.velocity.X * base.Projectile.velocity.X + base.Projectile.velocity.Y * base.Projectile.velocity.Y)) > 2.0)
			{
				base.Projectile.velocity *= 0.98f;
			}
            if(projectile.timeLeft < 1067)
            {
                int i = Dust.NewDust(projectile.Center, 0, 0, ModContent.DustType<Everglow.Ocean.Dusts.Aquamarine>(), 0f, 0f, 0, default(Color), 2f);
                Main.dust[i].velocity *= 0;
            }
            float num20 = base.Projectile.Center.X;
            float num30 = base.Projectile.Center.Y;
            float num4 = 400f;
            bool flag = false;
            for (int j = 0; j < 200; j++)
            {
                if (Main.npc[j].CanBeChasedBy(base.Projectile, false) && Collision.CanHit(base.Projectile.Center, 1, 1, Main.npc[j].Center, 1, 1))
                {
                    float num5 = Main.npc[j].position.X + (float)(Main.npc[j].width / 2);
                    float num6 = Main.npc[j].position.Y + (float)(Main.npc[j].height / 2);
                    float num7 = Math.Abs(base.Projectile.position.X + (float)(base.Projectile.width / 2) - num5) + Math.Abs(base.Projectile.position.Y + (float)(base.Projectile.height / 2) - num6);
                    if (num7 < num4)
                    {
                        num4 = num7;
                        num20 = num5;
                        num30 = num6;
                        flag = true;
                    }
                    if (num7 < 50)
                    {
                        Main.npc[j].StrikeNPC((int)(projectile.damage * Main.rand.NextFloat(0.85f, 1.15f)), Projectile.knockBack, projectile.direction, Main.rand.Next(200) > 150 ? true : false);
                        projectile.penetrate--;
                        if(projectile.penetrate < 0)
                        {
                            projectile.Kill();
                        }
                    }
                }
            }
            if (flag)
            {
                float num8 = 20f;
                Vector2 vector1 = new Vector2(base.Projectile.position.X + (float)base.Projectile.width * 0.5f, base.Projectile.position.Y + (float)base.Projectile.height * 0.5f);
                float num9 = num20 - vector1.X;
                float num10 = num30 - vector1.Y;
                Vector2 v = new Vector2(num20, num30) - projectile.Center;
                float num11 = (float)Math.Sqrt((double)(num9 * num9 + num10 * num10));
                num11 = num8 / num11;
                num9 *= num11;
                num10 *= num11;
                base.Projectile.velocity.X = (base.Projectile.velocity.X * v.Length() * 2 + num9) / (v.Length() * 2 + 1);
                base.Projectile.velocity.Y = (base.Projectile.velocity.Y * v.Length() * 2 + num10) / (v.Length() * 2 + 1);
            }
        }
	}
}
