using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Everglow.Ocean.OceanDeep.Projectiles
{
	// Token: 0x02000512 RID: 1298
    public class AbyssBomb : ModProjectile
	{
		// Token: 0x06001C81 RID: 7297 RVA: 0x0000BBD6 File Offset: 0x00009DD6
		public override void SetStaticDefaults()
		{
            // // base.DisplayName.SetDefault("渊海爆炸");
            Main.projFrames[Projectile.type] = 1; /*【帧数为6】对应的贴图也要画6帧哦*/
		}

		// Token: 0x06001C82 RID: 7298 RVA: 0x0016F518 File Offset: 0x0016D718
		public override void SetDefaults()
		{
			base.Projectile.width = 50;
			base.Projectile.height = 50;
			base.Projectile.friendly = true;
			base.Projectile.alpha = 65;
			base.Projectile.penetrate = -1;
			base.Projectile.tileCollide = false;
			base.Projectile.timeLeft = 1;
			base.Projectile.DamageType = DamageClass.Magic;
		}

		// Token: 0x06001C83 RID: 7299 RVA: 0x0016F58C File Offset: 0x0016D78C

		// Token: 0x06001C84 RID: 7300 RVA: 0x0016F648 File Offset: 0x0016D848
        public override void AI()
        {
        }
        // Token: 0x0600298B RID: 10635 RVA: 0x00213848 File Offset: 0x00211A48
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item14, new Vector2(base.Projectile.position.X, base.Projectile.position.Y));
			base.Projectile.position.X = base.Projectile.position.X + (float)(base.Projectile.width / 2);
			base.Projectile.position.Y = base.Projectile.position.Y + (float)(base.Projectile.height / 2);
			base.Projectile.width = 160;
			base.Projectile.height = 160;
			base.Projectile.position.X = base.Projectile.position.X - (float)(base.Projectile.width / 2);
			base.Projectile.position.Y = base.Projectile.position.Y - (float)(base.Projectile.height / 2);
			for (int i = 0; i < 30; i++)
			{
                int num = Dust.NewDust(new Vector2(base.Projectile.position.X, base.Projectile.position.Y), base.Projectile.width, base.Projectile.height, 96, 0f, 0f, 100, default(Color), 1f);
				Main.dust[num].velocity *= 3f;
				if (Main.rand.Next(2) == 0)
				{
					Main.dust[num].scale = 0.5f;
					Main.dust[num].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
				}
			}
			for (int j = 0; j < 60; j++)
			{
                int num2 = Dust.NewDust(new Vector2(base.Projectile.position.X, base.Projectile.position.Y), base.Projectile.width, base.Projectile.height, 96, 0f, 0f, 100, default(Color), 2f);
				Main.dust[num2].noGravity = false;
				Main.dust[num2].velocity *= 5f;
                num2 = Dust.NewDust(new Vector2(base.Projectile.position.X, base.Projectile.position.Y), base.Projectile.width, base.Projectile.height, 96, 0f, 0f, 100, default(Color), 2f);
				Main.dust[num2].velocity *= 2f;
			}
            for (int j = 0; j < 200; j++)
            {
                if (Main.npc[j].CanBeChasedBy(base.Projectile, false) && Collision.CanHit(base.Projectile.Center, 1, 1, Main.npc[j].Center, 1, 1))
                {
                    float num5 = Main.npc[j].position.X + (float)(Main.npc[j].width / 2);
                    float num6 = Main.npc[j].position.Y + (float)(Main.npc[j].height / 2);
                    float num7 = Math.Abs(base.Projectile.position.X + (float)(base.Projectile.width / 2) - num5) + Math.Abs(base.Projectile.position.Y + (float)(base.Projectile.height / 2) - num6);
                    if (num7 < 250)
                    {
                        Main.npc[j].StrikeNPC((int)((Projectile.damage * Main.rand.NextFloat(0.85f, 1.15f)) * (25 / (num7 + 10))), Projectile.knockBack, Projectile.direction, Main.rand.Next(200) > 150 ? true : false);
                        Projectile.penetrate--;
                    }
                }
            }
        }

		// Token: 0x06001C85 RID: 7301 RVA: 0x0000BBE8 File Offset: 0x00009DE8
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
		}
	}
}
