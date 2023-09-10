using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Everglow.Ocean.Projectiles
{
    public class Wave : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // base.DisplayName.SetDefault("波动");
        }

        public override void SetDefaults()
        {
            base.Projectile.width = 54;
            base.Projectile.height = 54;
            base.Projectile.aiStyle = -1;
            base.Projectile.friendly = true;
            base.Projectile.DamageType = DamageClass.Melee;
            base.Projectile.ignoreWater = true;
            base.Projectile.penetrate = -1;
            Projectile.alpha = 255;
            base.Projectile.extraUpdates = 2;
            base.Projectile.timeLeft = 610;
            base.Projectile.usesLocalNPCImmunity = true;
            base.Projectile.localNPCHitCooldown = 1;
        }

        public override void AI()
        {
            if(Projectile.alpha > 5)
            {
                Projectile.alpha -= 5;
            }
            float num2 = base.Projectile.Center.X;
            float num3 = base.Projectile.Center.Y;
            float num4 = 400f;
            bool flag = false;
            for (int j = 0; j < 200; j++)
            {
                if (Main.npc[j].CanBeChasedBy(base.Projectile, false) && Collision.CanHit(base.Projectile.Center, 1, 1, Main.npc[j].Center, 1, 1) && Main.npc[j].active)
                {
                    float num5 = Main.npc[j].position.X + (float)(Main.npc[j].width / 2);
                    float num6 = Main.npc[j].position.Y + (float)(Main.npc[j].height / 2);
                    float num7 = Math.Abs(base.Projectile.position.X + (float)(base.Projectile.width / 2) - num5) + Math.Abs(base.Projectile.position.Y + (float)(base.Projectile.height / 2) - num6);
                    if (num7 < num4)
                    {
                        num4 = num7;
                        num2 = num5;
                        num3 = num6;
                        flag = true;
                    }
                }
            }
            Projectile.rotation = (float)(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X)) + (float)Math.PI * 0.25f;
            if (flag)
            {
                float num8 = 20f;
                Vector2 vector1 = new Vector2(base.Projectile.position.X + (float)base.Projectile.width * 0.5f, base.Projectile.position.Y + (float)base.Projectile.height * 0.5f);
                float num9 = num2 - vector1.X;
                float num10 = num3 - vector1.Y;
                float num11 = (float)Math.Sqrt((double)(num9 * num9 + num10 * num10));
                num11 = num8 / num11;
                num9 *= num11;
                num10 *= num11;
                base.Projectile.velocity.X = (base.Projectile.velocity.X * 90f + num9) / 91f;
                base.Projectile.velocity.Y = (base.Projectile.velocity.Y * 90f + num10) / 91f;
            }
            else
            {
                Projectile.velocity = Projectile.velocity.RotatedBy(Math.Sin(Main.time / 3d) * 0.025);
            }
            Lighting.AddLight(base.Projectile.Center, (float)(255 - base.Projectile.alpha) * 0.0f / 255f, (float)(255 - base.Projectile.alpha) * 0.1f / 255f, (float)(255 - base.Projectile.alpha) * 0.7f / 255f);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.timeLeft > 600)
            {
                return new Color?(new Color(0f, 0f, 0f, 0));
            }
            else
            {
                if (Projectile.timeLeft > 500 && Projectile.timeLeft > 600)
                {
                    return new Color?(new Color((600 - Projectile.timeLeft) / 100f * 0.3f, (600 - Projectile.timeLeft) / 100f * 0.3f, (600 - Projectile.timeLeft) / 100f * 0.3f, 0));
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
                int num9 = Dust.NewDust(new Vector2(base.Projectile.Center.X, base.Projectile.Center.Y) - new Vector2(4, 4), 0, 0, 113, v.X, v.Y, 100, default(Color), 2.4f);
                Main.dust[num9].noGravity = true;
                Main.dust[num9].velocity *= 0.0f;
            }
            for (int i = 0; i < 9; i++)
            {
                Vector2 v = new Vector2(0, Main.rand.NextFloat(0, 7f)).RotatedByRandom(Math.PI * 2f);
                int num9 = Dust.NewDust(new Vector2(base.Projectile.Center.X, base.Projectile.Center.Y) - new Vector2(4, 4), 0, 0, 113, v.X, v.Y, 100, default(Color), 1.8f);
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
                int p = Dust.NewDust(new Vector2(base.Projectile.position.X, base.Projectile.position.Y), base.Projectile.width, base.Projectile.height, ModContent.DustType<Everglow.Ocean.Dusts.Wave>(), 0, 0, 0, default(Color), 1.8f);
                Main.dust[p].velocity = v2.RotatedBy(Math.Atan2((float)num80, (float)num90)) * 0.5f;
                Main.dust[p].scale = 1.4f + Math.Abs((float)Math.Atan2(-v1.Y, -v1.X) / (1 + (float)num60 / 2000f));
                Main.dust[p].noGravity = true;
            }
            for (int i = 0; i < 200; i++)
            {
                if ((Main.npc[i].Center - Projectile.position).Length() < Main.npc[i].Hitbox.Width / 2f + 20)
                {
                    Main.npc[i].StrikeNPC((int)(Projectile.damage / 2f), 0, 1,Main.rand.Next(100) > 20 ? false : true);
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, base.Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(27, 27), Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}
