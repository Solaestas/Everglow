using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MythMod.Projectiles.projectile2
{
    public class OceanBlue : ModProjectile
	{
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("海蓝");
		}
		public override void SetDefaults()
		{
			base.projectile.width = 28;
            base.projectile.tileCollide = false;
            base.projectile.height = 28;
			base.projectile.friendly = true;
			base.projectile.penetrate = 6;
			base.projectile.timeLeft = 200;
			base.projectile.melee = true;
            base.projectile.aiStyle = 27;
			base.projectile.scale = 1.5f;
		}
		public override void AI()
		{
            projectile.velocity *= 0.99f;
			float num = base.projectile.Center.X;
			float num2 = base.projectile.Center.Y;
            if (projectile.timeLeft > 120 && projectile.timeLeft < 193)
            {
                int num3 = Dust.NewDust(base.projectile.Center - base.projectile.velocity * 4f - new Vector2(4, 4), 0, 0, 59, 0, 0, 0, default(Color), 1.6f);
                Main.dust[num3].noGravity = true;
                Main.dust[num3].velocity = new Vector2(0, 0);
                Lighting.AddLight(base.projectile.Center, (float)(255 - base.projectile.alpha) * 0f / 255f * projectile.scale, (float)(255 - base.projectile.alpha) * 0.23f * projectile.scale / 255f, (float)(255 - base.projectile.alpha) * 2.55f / 255f * projectile.scale);
            }
            if (projectile.timeLeft <= 120)
            {
                int num3 = Dust.NewDust(base.projectile.Center - base.projectile.velocity * 4f - new Vector2(4, 4), 0, 0, 59, 0, 0, 0, default(Color), 1.6f * projectile.timeLeft / 120f);
                Main.dust[num3].noGravity = true;
                Main.dust[num3].velocity = new Vector2(0, 0);
                Lighting.AddLight(base.projectile.Center, (float)(255 - base.projectile.alpha) * 0f / 255f * projectile.scale * projectile.timeLeft / 120f, (float)(255 - base.projectile.alpha) * 0.23f * projectile.scale / 255f * projectile.timeLeft / 120f, (float)(255 - base.projectile.alpha) * 2.55f / 255f * projectile.scale * projectile.timeLeft / 120f);
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
		}
        public override Color? GetAlpha(Color lightColor)
        {
            if (projectile.timeLeft > 60)
            {
                return new Color?(new Color(255, 255, 255, 0));
            }
            else
            {
                return new Color?(new Color(1 * projectile.timeLeft / 60f, 1 * projectile.timeLeft / 60f, 1 * projectile.timeLeft / 60f, 0));
            }
        }
        // Token: 0x060028C0 RID: 10432 RVA: 0x00208E28 File Offset: 0x00207028
        public override void Kill(int timeLeft)
        {
        }
	}
}
