using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Everglow.Ocean.Projectiles
{
    public class LavaCup : ModProjectile
    {
        private float num = 0;
        private float num16 = 0;
        private int num12 = 0;
        private int num18 = 0;
        public override void SetStaticDefaults()
        {
            // base.DisplayName.SetDefault("熔岩灌顶");
            ProjectileID.Sets.MinionSacrificable[base.Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[base.Projectile.type] = true;
            Main.projFrames[base.Projectile.type] = 1;
        }
        public override void SetDefaults()
        {
            base.Projectile.width = 42;
            base.Projectile.height = 42;
            base.Projectile.netImportant = true;
            base.Projectile.friendly = true;
            base.Projectile.minionSlots = 4f;
            base.Projectile.aiStyle = -1;
            base.Projectile.timeLeft = 720000;
            base.Projectile.aiStyle = 54;
            base.Projectile.timeLeft *= 5;
            this.AIType = 317;
            base.Projectile.penetrate = -1;
            base.Projectile.minion = true;
            base.Projectile.tileCollide = false;
            base.Projectile.usesLocalNPCImmunity = true;
            base.Projectile.localNPCHitCooldown = 10;
        }
        public override void AI()
        {
            bool flag2 = base.Projectile.type ==ModContent.ProjectileType<Everglow.Ocean.Projectiles.LavaCup>();
            Player player = Main.player[base.Projectile.owner];
            OceanContentPlayer modPlayer = player.GetModPlayer<OceanContentPlayer>();
            if (!player.HasBuff(Mod.Find<ModBuff>("LavaCup").Type))
            {
                for (int i = 0; i < 1000; i++)
                {
                    if (Main.projectile[i].type == ModContent.ProjectileType<Everglow.Ocean.Projectiles.LavaCup>())
                    {
                        Main.projectile[i].Kill();
                    }
                }
            }
            player.AddBuff(base.Mod.Find<ModBuff>("LavaCup").Type, 3600, true);
            bool flag = false;
            float num2 = base.Projectile.Center.X;
            float num3 = base.Projectile.Center.Y;
            Vector2 num16 = new Vector2(0,0);
            float num4 = 800f;
            if (Projectile.wet)
            {
                num4 = 1600f;
            }
            int mk = -1;
            for (int j = 0; j < 200; j++)
            {
                if (Main.npc[j].CanBeChasedBy(base.Projectile, false) && Collision.CanHit(base.Projectile.Center, 1, 1, Main.npc[j].Center, 1, 1))
                {
                    float num5 = Main.npc[j].position.X + (float)(Main.npc[j].width / 2);
                    float num15 = Main.npc[j].position.X + (float)(Main.npc[j].width / 2);
                    float num6 = Main.npc[j].position.Y - (float)(Main.npc[j].height / 2) - 200;
                    float num14 = Main.npc[j].position.Y + (float)(Main.npc[j].height / 2);
                    float num7 = Math.Abs(base.Projectile.position.X + (float)(base.Projectile.width / 2) - num15) + Math.Abs(base.Projectile.position.Y + (float)(base.Projectile.height / 2) - num6);
                    if (num7 < num4)
                    {
                        num4 = num7;
                        num2 = num15;
                        num3 = num6;
                        num16 = Main.npc[j].velocity;
                        flag = true;
                    }
                    if (mk == -1 && num7 < num4)
                    {
                        mk = j;
                    }
                    if (mk != -1)
                    {
                        if ((Main.npc[mk].Center - Projectile.Center).Length() > num7)
                        {
                            mk = j;
                            num4 = num7;
                            num2 = num15;
                            num3 = num6;
                        }
                    }
                    float num20 = 250f;
                    float num21 = 5;
                }
                else
                {
                }
            }


            if (flag)
            {
                if ((new Vector2(num2,num3) - Projectile.Center).Length() < 15)
                {
                    int i = Projectile.NewProjectile(Projectile.GetSource_FromAI(), base.Projectile.Center.X, base.Projectile.Center.Y, 0 + num16.X, 2 + num16.Y,ModContent.ProjectileType<Everglow.Ocean.Projectiles.火山溅射>(), base.Projectile.damage * 4, base.Projectile.knockBack, Main.myPlayer, 0f, 0f);
                    Main.projectile[i].timeLeft = 60;
                    Main.projectile[i].penetrate = 1;
                }
                float num8 = 20f;
                Vector2 vector3 = new Vector2(base.Projectile.position.X + (float)base.Projectile.width * 0.5f, base.Projectile.position.Y + (float)base.Projectile.height * 0.5f);
                float num9 = num2 - vector3.X;
                float num10 = num3 - vector3.Y;
                float num11 = (float)Math.Sqrt((double)(num9 * num9 + num10 * num10));
                num11 = num8 / num11;
                num9 *= num11;
                num10 *= num11;
                base.Projectile.velocity.X = (base.Projectile.velocity.X * 5f + num9) / 6f;
                base.Projectile.velocity.Y = (base.Projectile.velocity.Y * 5f + num10) / 6f;
                num12 += 1;
            }
            else
            {
            }
        }
        public override void PostDraw(Color lightColor)
        {
            Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("Everglow/Ocean/Projectiles/projectile2/LavaCupGlow"), base.Projectile.Center - Main.screenPosition, null, Color.White * 0.7f, base.Projectile.rotation, new Vector2(21f, 21f), 1f, SpriteEffects.None, 0f);
        }
    }
}
