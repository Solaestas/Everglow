using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace MythMod.Projectiles.projectile3
{
    public class Wave : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            base.DisplayName.SetDefault("波动");
        }

        public override void SetDefaults()
        {
            base.projectile.width = 54;
            base.projectile.height = 54;
            base.projectile.aiStyle = -1;
            base.projectile.friendly = true;
            base.projectile.melee = true;
            base.projectile.ignoreWater = true;
            base.projectile.penetrate = -1;
            projectile.alpha = 255;
            base.projectile.extraUpdates = 2;
            base.projectile.timeLeft = 610;
            base.projectile.usesLocalNPCImmunity = true;
            base.projectile.localNPCHitCooldown = 1;
        }

        public override void AI()
        {
            if(projectile.alpha > 5)
            {
                projectile.alpha -= 5;
            }
            float num2 = base.projectile.Center.X;
            float num3 = base.projectile.Center.Y;
            float num4 = 400f;
            bool flag = false;
            for (int j = 0; j < 200; j++)
            {
                if (Main.npc[j].CanBeChasedBy(base.projectile, false) && Collision.CanHit(base.projectile.Center, 1, 1, Main.npc[j].Center, 1, 1) && Main.npc[j].active)
                {
                    float num5 = Main.npc[j].position.X + (float)(Main.npc[j].width / 2);
                    float num6 = Main.npc[j].position.Y + (float)(Main.npc[j].height / 2);
                    float num7 = Math.Abs(base.projectile.position.X + (float)(base.projectile.width / 2) - num5) + Math.Abs(base.projectile.position.Y + (float)(base.projectile.height / 2) - num6);
                    if (num7 < num4)
                    {
                        num4 = num7;
                        num2 = num5;
                        num3 = num6;
                        flag = true;
                    }
                }
            }
            projectile.rotation = (float)(Math.Atan2(projectile.velocity.Y, projectile.velocity.X)) + (float)Math.PI * 0.25f;
            if (flag)
            {
                float num8 = 20f;
                Vector2 vector1 = new Vector2(base.projectile.position.X + (float)base.projectile.width * 0.5f, base.projectile.position.Y + (float)base.projectile.height * 0.5f);
                float num9 = num2 - vector1.X;
                float num10 = num3 - vector1.Y;
                float num11 = (float)Math.Sqrt((double)(num9 * num9 + num10 * num10));
                num11 = num8 / num11;
                num9 *= num11;
                num10 *= num11;
                base.projectile.velocity.X = (base.projectile.velocity.X * 90f + num9) / 91f;
                base.projectile.velocity.Y = (base.projectile.velocity.Y * 90f + num10) / 91f;
            }
            else
            {
                projectile.velocity = projectile.velocity.RotatedBy(Math.Sin(Main.time / 3d) * 0.025);
            }
            Lighting.AddLight(base.projectile.Center, (float)(255 - base.projectile.alpha) * 0.0f / 255f, (float)(255 - base.projectile.alpha) * 0.1f / 255f, (float)(255 - base.projectile.alpha) * 0.7f / 255f);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            if (projectile.timeLeft > 600)
            {
                return new Color?(new Color(0f, 0f, 0f, 0));
            }
            else
            {
                if (projectile.timeLeft > 500 && projectile.timeLeft > 600)
                {
                    return new Color?(new Color((600 - projectile.timeLeft) / 100f * 0.3f, (600 - projectile.timeLeft) / 100f * 0.3f, (600 - projectile.timeLeft) / 100f * 0.3f, 0));
                }
                else
                {
                    return new Color?(new Color(0.3f, 0.3f, 0.3f, 0));
                }
            }
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0;i < 15;i++)
            {
                Vector2 v = new Vector2(0, Main.rand.NextFloat(0, 7f)).RotatedByRandom(Math.PI * 2f);
                int num9 = Dust.NewDust(new Vector2(base.projectile.Center.X, base.projectile.Center.Y) - new Vector2(4, 4), 0, 0, 113, v.X, v.Y, 100, default(Color), 2.4f);
                Main.dust[num9].noGravity = true;
                Main.dust[num9].velocity *= 0.0f;
            }
            for (int i = 0; i < 9; i++)
            {
                Vector2 v = new Vector2(0, Main.rand.NextFloat(0, 7f)).RotatedByRandom(Math.PI * 2f);
                int num9 = Dust.NewDust(new Vector2(base.projectile.Center.X, base.projectile.Center.Y) - new Vector2(4, 4), 0, 0, 113, v.X, v.Y, 100, default(Color), 1.8f);
                Main.dust[num9].noGravity = true;
                Main.dust[num9].velocity *= 0.0f;
            }

            float num60 = (float)Main.rand.Next(0, 10000);
            int num80 = Main.rand.Next(-1000, 1000) / 100;
            double num90 = (double)Math.Sqrt(100 - (int)num80 * (int)num80);
            Vector2 v1 = Vector2.Normalize(new Vector2((float)num80, (float)num90)) * 5;
            Vector2 mc = Main.screenPosition + new Vector2((float)num80, (float)num90);
            float num100 = (float)Main.rand.Next(0, 10000) / 1000f;
            float T = (float)(Main.rand.Next(0, 10000) / 5000f * Math.PI);
            for (int i = 0; i < 50; i++)
            {
                v1 = v1.RotatedBy(Math.PI / 25f);
                Vector2 v2 = new Vector2(v1.X * (float)num60 / 10000f, v1.Y);
                int p = Dust.NewDust(new Vector2(base.projectile.position.X, base.projectile.position.Y), base.projectile.width, base.projectile.height, mod.DustType("Wave"), 0, 0, 0, default(Color), 1.8f);
                Main.dust[p].velocity = v2.RotatedBy(Math.Atan2((float)num80, (float)num90)) * 0.5f;
                Main.dust[p].scale = 1.4f + Math.Abs((float)Math.Atan2(-v1.Y, -v1.X) / (1 + (float)num60 / 2000f));
                Main.dust[p].noGravity = true;
            }
            for (int i = 0; i < 200; i++)
            {
                if ((Main.npc[i].Center - projectile.position).Length() < Main.npc[i].Hitbox.Width / 2f + 20)
                {
                    Main.npc[i].StrikeNPC((int)(projectile.damage / 2f), 0, 1,Main.rand.Next(100) > 20 ? false : true);
                }
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Mod mod = ModLoader.GetMod("MythMod");
            Texture2D texture = Main.projectileTexture[projectile.type];
            Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), null, base.projectile.GetAlpha(drawColor), projectile.rotation, new Vector2(27, 27), projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}
