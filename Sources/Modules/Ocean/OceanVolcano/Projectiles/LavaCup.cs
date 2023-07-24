using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythMod.Projectiles.projectile2
{
    public class LavaCup : ModProjectile
    {
        private float num = 0;
        private float num16 = 0;
        private int num12 = 0;
        private int num18 = 0;
        public override void SetStaticDefaults()
        {
            base.DisplayName.SetDefault("熔岩灌顶");
            ProjectileID.Sets.MinionSacrificable[base.projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[base.projectile.type] = true;
            Main.projFrames[base.projectile.type] = 1;
        }
        public override void SetDefaults()
        {
            base.projectile.width = 42;
            base.projectile.height = 42;
            base.projectile.netImportant = true;
            base.projectile.friendly = true;
            base.projectile.minionSlots = 4f;
            base.projectile.aiStyle = -1;
            base.projectile.timeLeft = 720000;
            base.projectile.aiStyle = 54;
            base.projectile.timeLeft *= 5;
            this.aiType = 317;
            base.projectile.penetrate = -1;
            base.projectile.minion = true;
            base.projectile.tileCollide = false;
            base.projectile.usesLocalNPCImmunity = true;
            base.projectile.localNPCHitCooldown = 10;
        }
        public override void AI()
        {
            bool flag2 = base.projectile.type == base.mod.ProjectileType("LavaCup");
            Player player = Main.player[base.projectile.owner];
            MythPlayer modPlayer = player.GetModPlayer<MythPlayer>();
            if (!player.HasBuff(mod.BuffType("LavaCup")))
            {
                for (int i = 0; i < 1000; i++)
                {
                    if (Main.projectile[i].type == mod.ProjectileType("LavaCup"))
                    {
                        Main.projectile[i].Kill();
                    }
                }
            }
            player.AddBuff(base.mod.BuffType("LavaCup"), 3600, true);
            bool flag = false;
            float num2 = base.projectile.Center.X;
            float num3 = base.projectile.Center.Y;
            Vector2 num16 = new Vector2(0,0);
            float num4 = 800f;
            if (projectile.wet)
            {
                num4 = 1600f;
            }
            int mk = -1;
            for (int j = 0; j < 200; j++)
            {
                if (Main.npc[j].CanBeChasedBy(base.projectile, false) && Collision.CanHit(base.projectile.Center, 1, 1, Main.npc[j].Center, 1, 1))
                {
                    float num5 = Main.npc[j].position.X + (float)(Main.npc[j].width / 2);
                    float num15 = Main.npc[j].position.X + (float)(Main.npc[j].width / 2);
                    float num6 = Main.npc[j].position.Y - (float)(Main.npc[j].height / 2) - 200;
                    float num14 = Main.npc[j].position.Y + (float)(Main.npc[j].height / 2);
                    float num7 = Math.Abs(base.projectile.position.X + (float)(base.projectile.width / 2) - num15) + Math.Abs(base.projectile.position.Y + (float)(base.projectile.height / 2) - num6);
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
                        if ((Main.npc[mk].Center - projectile.Center).Length() > num7)
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
                if ((new Vector2(num2,num3) - projectile.Center).Length() < 15)
                {
                    int i = Projectile.NewProjectile(base.projectile.Center.X, base.projectile.Center.Y, 0 + num16.X, 2 + num16.Y, base.mod.ProjectileType("火山溅射"), base.projectile.damage * 4, base.projectile.knockBack, Main.myPlayer, 0f, 0f);
                    Main.projectile[i].timeLeft = 60;
                    Main.projectile[i].penetrate = 1;
                }
                float num8 = 20f;
                Vector2 vector3 = new Vector2(base.projectile.position.X + (float)base.projectile.width * 0.5f, base.projectile.position.Y + (float)base.projectile.height * 0.5f);
                float num9 = num2 - vector3.X;
                float num10 = num3 - vector3.Y;
                float num11 = (float)Math.Sqrt((double)(num9 * num9 + num10 * num10));
                num11 = num8 / num11;
                num9 *= num11;
                num10 *= num11;
                base.projectile.velocity.X = (base.projectile.velocity.X * 5f + num9) / 6f;
                base.projectile.velocity.Y = (base.projectile.velocity.Y * 5f + num10) / 6f;
                num12 += 1;
            }
            else
            {
            }
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            spriteBatch.Draw(base.mod.GetTexture("Projectiles/projectile2/LavaCupGlow"), base.projectile.Center - Main.screenPosition, null, Color.White * 0.7f, base.projectile.rotation, new Vector2(21f, 21f), 1f, SpriteEffects.None, 0f);
        }
    }
}
