using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MythMod.Projectiles.projectile2
{
    public class OlivineStaffPro : ModProjectile
	{
		public override void SetDefaults()
		{
			base.projectile.width = 16;
			base.projectile.height = 16;
			base.projectile.aiStyle = 1;
			base.projectile.alpha = 255;
			base.projectile.scale = 1f;
			base.projectile.friendly = true;
			base.projectile.magic = true;
			base.projectile.penetrate = 2;
			base.projectile.timeLeft = 3600;
			this.aiType = 14;
		}
		public override void AI()
		{
			if(base.projectile.timeLeft <= 3594)
			{
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
                    }
                }
            }
            if (flag)
            {
                float num8 = 20f;
                Vector2 vector1 = new Vector2(base.projectile.position.X + (float)base.projectile.width * 0.5f, base.projectile.position.Y + (float)base.projectile.height * 0.5f);
                float num9 = num20 - vector1.X;
                float num10 = num30 - vector1.Y;
                float num11 = (float)Math.Sqrt((double)(num9 * num9 + num10 * num10));
                num11 = num8 / num11;
                num9 *= num11;
                num10 *= num11;
                base.projectile.velocity.X = (base.projectile.velocity.X * 40f + num9) / 41f;
                base.projectile.velocity.Y = (base.projectile.velocity.Y * 40f + num10) / 41f;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if(projectile.penetrate >= 1)
            {
                for (int i = 0; i < 10; i++)
                {
                    int num = Dust.NewDust(base.projectile.Center, base.projectile.width, base.projectile.height, mod.DustType("Olivine"), base.projectile.oldVelocity.X, base.projectile.oldVelocity.Y, 0, default(Color), 2.7f);
                    int num1 = Dust.NewDust(base.projectile.Center, base.projectile.width, base.projectile.height, mod.DustType("Olivine"), base.projectile.oldVelocity.X, base.projectile.oldVelocity.Y, 0, default(Color), 2.6f);
                    int num2 = Dust.NewDust(base.projectile.Center, base.projectile.width, base.projectile.height, mod.DustType("Olivine"), base.projectile.oldVelocity.X, base.projectile.oldVelocity.Y, 0, default(Color), 2.2f);
                    Main.dust[num].noGravity = true;
                    Main.dust[num1].noGravity = true;
                    Main.dust[num2].noGravity = true;
                    Vector2 v = projectile.oldVelocity.RotatedBy(Main.rand.NextFloat(-0.8f, 0.8f)) * Main.rand.NextFloat(0.2f, 0.8f) * i;
                    Projectile.NewProjectile(projectile.Center.X + v.X, projectile.Center.Y + v.Y, 0, 0, mod.ProjectileType("OlivineStaffPro2"), projectile.damage, projectile.knockBack, Main.myPlayer, 0, i * 9);
                }
            }
        }
        public override void Kill(int timeLeft)
		{
            if (projectile.penetrate < 1)
            {
                for (int i = 0; i < 20; i++)
                {
                    int num = Dust.NewDust(base.projectile.Center, base.projectile.width, base.projectile.height, mod.DustType("Olivine"), base.projectile.oldVelocity.X, base.projectile.oldVelocity.Y, 0, default(Color), 2.7f);
                    int num1 = Dust.NewDust(base.projectile.Center, base.projectile.width, base.projectile.height, mod.DustType("Olivine"), base.projectile.oldVelocity.X, base.projectile.oldVelocity.Y, 0, default(Color), 2.6f);
                    int num2 = Dust.NewDust(base.projectile.Center, base.projectile.width, base.projectile.height, mod.DustType("Olivine"), base.projectile.oldVelocity.X, base.projectile.oldVelocity.Y, 0, default(Color), 2.2f);
                    Main.dust[num].noGravity = true;
                    Main.dust[num1].noGravity = true;
                    Main.dust[num2].noGravity = true;
                    Vector2 v = projectile.oldVelocity.RotatedBy(Main.rand.NextFloat(-0.8f, 0.8f)) * Main.rand.NextFloat(0.2f, 2.4f) * i;
                    Projectile.NewProjectile(projectile.Center.X + v.X, projectile.Center.Y + v.Y, 0, 0, mod.ProjectileType("OlivineStaffPro2"), projectile.damage, projectile.knockBack, Main.myPlayer, 0, i * 4.5f);
                }
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    int num = Dust.NewDust(base.projectile.Center, base.projectile.width, base.projectile.height, mod.DustType("Olivine"), base.projectile.oldVelocity.X, base.projectile.oldVelocity.Y, 0, default(Color), 2.7f);
                    int num1 = Dust.NewDust(base.projectile.Center, base.projectile.width, base.projectile.height, mod.DustType("Olivine"), base.projectile.oldVelocity.X, base.projectile.oldVelocity.Y, 0, default(Color), 2.6f);
                    int num2 = Dust.NewDust(base.projectile.Center, base.projectile.width, base.projectile.height, mod.DustType("Olivine"), base.projectile.oldVelocity.X, base.projectile.oldVelocity.Y, 0, default(Color), 2.2f);
                    Main.dust[num].noGravity = true;
                    Main.dust[num1].noGravity = true;
                    Main.dust[num2].noGravity = true;
                    Vector2 v = projectile.oldVelocity.RotatedBy(Main.rand.NextFloat(-0.8f, 0.8f)) * Main.rand.NextFloat(0.2f, 0.8f) * i;
                    Projectile.NewProjectile(projectile.Center.X + v.X, projectile.Center.Y + v.Y, 0, 0, mod.ProjectileType("OlivineStaffPro2"), projectile.damage, projectile.knockBack, Main.myPlayer, 0, i * 9);
                }
            }
		}
	}
}
