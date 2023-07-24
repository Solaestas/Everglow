using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MythMod.Projectiles.projectile3
{
	public class OceanCurrentRay : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("洋流射线");
		}
		public override void SetDefaults()
		{
			base.projectile.width = 14;
			base.projectile.height = 14;
			base.projectile.friendly = true;
			base.projectile.alpha = 255;
			base.projectile.penetrate = 5;
			base.projectile.timeLeft = 1080;
			base.projectile.magic = true;
            projectile.extraUpdates = 100;

        }
		public override void AI()
		{
			Projectile projectile = base.projectile;
			projectile.velocity.X = projectile.velocity.X * 1.01f;
			Projectile projectile2 = base.projectile;
			projectile2.velocity.Y = projectile2.velocity.Y * 1.01f;
			if (base.projectile.ai[0] == 0f)
			{
				base.projectile.ai[0] = base.projectile.velocity.X;
				base.projectile.ai[1] = base.projectile.velocity.Y;
			}
			if (Math.Sqrt((double)(base.projectile.velocity.X * base.projectile.velocity.X + base.projectile.velocity.Y * base.projectile.velocity.Y)) > 2.0)
			{
				base.projectile.velocity *= 0.98f;
			}
            if(projectile.timeLeft < 1067)
            {
                int i = Dust.NewDust(projectile.Center, 0, 0, mod.DustType("Aquamarine"), 0f, 0f, 0, default(Color), 2f);
                Main.dust[i].velocity *= 0;
            }
            float num20 = base.projectile.Center.X;
            float num30 = base.projectile.Center.Y;
            float num4 = 400f;
            bool flag = false;
            for (int j = 0; j < 200; j++)
            {
                if (Main.npc[j].CanBeChasedBy(base.projectile, false) && Collision.CanHit(base.projectile.Center, 1, 1, Main.npc[j].Center, 1, 1))
                {
                    float num5 = Main.npc[j].position.X + (float)(Main.npc[j].width / 2);
                    float num6 = Main.npc[j].position.Y + (float)(Main.npc[j].height / 2);
                    float num7 = Math.Abs(base.projectile.position.X + (float)(base.projectile.width / 2) - num5) + Math.Abs(base.projectile.position.Y + (float)(base.projectile.height / 2) - num6);
                    if (num7 < num4)
                    {
                        num4 = num7;
                        num20 = num5;
                        num30 = num6;
                        flag = true;
                    }
                    if (num7 < 50)
                    {
                        Main.npc[j].StrikeNPC((int)(projectile.damage * Main.rand.NextFloat(0.85f, 1.15f)), projectile.knockBack, projectile.direction, Main.rand.Next(200) > 150 ? true : false);
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
                Vector2 vector1 = new Vector2(base.projectile.position.X + (float)base.projectile.width * 0.5f, base.projectile.position.Y + (float)base.projectile.height * 0.5f);
                float num9 = num20 - vector1.X;
                float num10 = num30 - vector1.Y;
                Vector2 v = new Vector2(num20, num30) - projectile.Center;
                float num11 = (float)Math.Sqrt((double)(num9 * num9 + num10 * num10));
                num11 = num8 / num11;
                num9 *= num11;
                num10 *= num11;
                base.projectile.velocity.X = (base.projectile.velocity.X * v.Length() * 2 + num9) / (v.Length() * 2 + 1);
                base.projectile.velocity.Y = (base.projectile.velocity.Y * v.Length() * 2 + num10) / (v.Length() * 2 + 1);
            }
        }
	}
}
