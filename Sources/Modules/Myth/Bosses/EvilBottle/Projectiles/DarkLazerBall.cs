using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Localization;
using Everglow.Myth.Common;
using Everglow.Myth.Bosses.EvilBottle.Dusts;
using Everglow.Myth.MiscItems.Projectiles.Typeless;

namespace Everglow.Myth.Bosses.EvilBottle.Projectiles
{
    public class DarkLazerBall : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("诡光球");
        }
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
        }
        float r = 0;
        private Vector2 v0;
        private int Fra = 0;
        private int FraX = 0;
        private int FraY = 0;
        public override void AI()
        {
            if (Projectile.timeLeft == 600)
            {
                v0 = Projectile.Center;
            }
            if (Projectile.timeLeft > 540)
            {
                r += 1f;
            }
            if (Projectile.timeLeft <= 540 && Projectile.timeLeft >= 60)
            {
                r = 60 + (float)(10 * Math.Sin((Projectile.timeLeft - 60) / 60d * Math.PI));
            }
            if (Projectile.timeLeft < 60 && r > 0.5f)
            {
                r -= 1f;
            }
            int Dx = (int)(r * 1.5f);
            int Dy = (int)(r * 1.5f);
            Fra = ((600 - Projectile.timeLeft) / (int)3) % 30;
            FraX = (Fra % 6) * 270;
                FraY = (Fra / (int)6) * 290;
            if (v0 != Vector2.Zero)
            {
                Projectile.position = v0 - new Vector2(Dx, Dy) / 2f;
            }

            Projectile.width = Dx;
            Projectile.height = Dy;
            if (Projectile.timeLeft < 480)
            {
                n *= 0.99f;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }
        float n = 0.2f;
        public override bool PreDraw(ref Color lightColor)
        {
            float Leng = 1000;
            Main.spriteBatch.Draw(MythContent.QuickTexture("Bosses/EvilBottle/Projectiles/DarkLazerBall"), base.Projectile.Center - Main.screenPosition, new Rectangle(FraX,10 + FraY, 270,270), new Color(1f, 1f, 1f, 0), base.Projectile.rotation, new Vector2(135f, 135f), r / 60f, SpriteEffects.None, 0f);
            for(int i = 0;i < 4;i++)
            {
                float Sca = (i * 20 - Projectile.timeLeft + 500) / 80f;
                if(Sca > 0.2f)
                {
                    Sca = 0.2f;
                }
                if(Sca > 0)
                {
                    Main.spriteBatch.Draw(MythContent.QuickTexture("Bosses/EvilBottle/heatmapColdPurpleCircle"), base.Projectile.Center - Main.screenPosition + new Vector2(0, -r).RotatedBy(Math.PI * n * (i + 1)), null, new Color(1f, 1f, 1f, 0), base.Projectile.rotation, new Vector2(128f, 128f), Sca, SpriteEffects.None, 0f);
                    Main.spriteBatch.Draw(MythContent.QuickTexture("Bosses/EvilBottle/heatmapColdPurpleCircle"), base.Projectile.Center - Main.screenPosition + new Vector2(0, -r).RotatedBy(Math.PI * n * -(i + 1)), null, new Color(1f, 1f, 1f, 0), base.Projectile.rotation, new Vector2(128f, 128f), Sca, SpriteEffects.None, 0f);
                }
                if (Projectile.timeLeft > 32)
                {
                    if (Sca > 0.1f)
                    {
                        for (int k = 0; k < Leng - 52; k += 16)
                        {
                            float[] samples = new float[2];
                            Vector2 unit = new Vector2(0, -1).RotatedBy(Math.PI * n * (i + 1));
                            Collision.LaserScan(Projectile.Center, unit, 3f, 1000f, samples);
                            float maxDis = (samples[0] + samples[1]) * 0.5f;
                            Leng = maxDis;
                            int thi = (int)((Sca - 0.1f) * 80f);
                            if (k < Leng - 68)
                            {
                                Main.spriteBatch.Draw(MythContent.QuickTexture("Bosses/EvilBottle/Projectiles/EvilLazer"), base.Projectile.Center - Main.screenPosition + new Vector2(0, -r - k - 8).RotatedBy(Math.PI * n * (i + 1)), new Rectangle(0, 0, 16, 16), new Color((Sca - 0.1f) * 10, (Sca - 0.1f) * 10, (Sca - 0.1f) * 10, 0), -(float)(Math.PI * n * -(i + 1) + Math.PI / 2d), new Vector2(8f, 8f), 1, SpriteEffects.None, 0f);
                            }
                            else
                            {
                                Main.spriteBatch.Draw(MythContent.QuickTexture("Bosses/EvilBottle/Projectiles/EvilLazerEnd"), base.Projectile.Center - Main.screenPosition + new Vector2(0, -r - k - 8).RotatedBy(Math.PI * n * (i + 1)), new Rectangle(0, 0, 16, 16), new Color((Sca - 0.1f) * 10, (Sca - 0.1f) * 10, (Sca - 0.1f) * 10, 0), -(float)(Math.PI * n * -(i + 1) + Math.PI / 2d), new Vector2(8f, 8f), 1, SpriteEffects.None, 0f);
                                if (!Main.gamePaused)
                                {
                                    int num = Dust.NewDust(Projectile.Center + new Vector2(0, -r - k - 8).RotatedBy(Math.PI * n * (i + 1)) - new Vector2(4, 4), 2, 2, ModContent.DustType<DarkF>(), 50f, 50f, 0, default(Color), (float)Projectile.scale * 2.4f);
                                    Main.dust[num].velocity *= 0.0f;
                                    Main.dust[num].noGravity = true;
                                    Main.dust[num].alpha = 150;
                                    Vector2 O = Projectile.Center;
                                    Vector2 P1 = O;
                                    Vector2 P2 = O + new Vector2(0, -r - k - 8).RotatedBy(Math.PI * n * (i + 1));
                                    Vector2 P4 = new Vector2(0, -r - k - 8).RotatedBy(Math.PI * n * (i + 1));
                                    int pl = (int)Player.FindClosest(base.Projectile.Center, 1, 1);
                                    Vector2 P3 = Main.player[pl].Center - O;
                                    float K = (P1.Y - P2.Y) / (P1.X - P2.X);
                                    float D = (float)((P3.X * K - P3.Y) / Math.Sqrt(K * K + 1));
                                    float D2 = P3.Length();
                                    float Sig = P4.X * P3.X + P4.Y * P3.Y;
                                    if (Math.Abs(D) < 15 && D2 < Leng - 52 && Sig > 0)
                                    {
                                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), Main.player[pl].Center.X, Main.player[pl].Center.Y, 0, 0, ModContent.ProjectileType<Hit>(), Projectile.damage, 0f, Main.myPlayer, 0f, 0f);
                                    }
                                }
                            }
                        }
                        for (int k = 0; k < Leng - 52; k += 16)
                        {
                            float[] samples = new float[2];
                            Vector2 unit = new Vector2(0, -1).RotatedBy(Math.PI * n * -(i + 1));
                            Collision.LaserScan(Projectile.Center, unit, 3f, 1000f, samples);
                            float maxDis = (samples[0] + samples[1]) * 0.5f;
                            Leng = maxDis;
                            int thi = (int)((Sca - 0.1f) * 80f);
                            if (k < Leng - 68)
                            {
                                Main.spriteBatch.Draw(MythContent.QuickTexture("Bosses/EvilBottle/Projectiles/EvilLazer"), base.Projectile.Center - Main.screenPosition + new Vector2(0, -r - k - 8).RotatedBy(Math.PI * n * -(i + 1)), new Rectangle(0, 0, 16, 16), new Color((Sca - 0.1f) * 10, (Sca - 0.1f) * 10, (Sca - 0.1f) * 10, 0), -(float)(Math.PI * n * (i + 1) + Math.PI / 2d), new Vector2(8f, 8f), 1, SpriteEffects.None, 0f);
                            }
                            else
                            {
                                Main.spriteBatch.Draw(MythContent.QuickTexture("Bosses/EvilBottle/Projectiles/EvilLazerEnd"), base.Projectile.Center - Main.screenPosition + new Vector2(0, -r - k - 8).RotatedBy(Math.PI * n * -(i + 1)), new Rectangle(0, 0, 16, 16), new Color((Sca - 0.1f) * 10, (Sca - 0.1f) * 10, (Sca - 0.1f) * 10, 0), -(float)(Math.PI * n * (i + 1) + Math.PI / 2d), new Vector2(8f, 8f), 1, SpriteEffects.None, 0f);
                                if (!Main.gamePaused)
                                {
                                    int num = Dust.NewDust(Projectile.Center + new Vector2(0, -r - k - 8).RotatedBy(Math.PI * n * -(i + 1)) - new Vector2(4, 4), 2, 2, ModContent.DustType<DarkF>(), 50f, 50f, 0, default(Color), (float)Projectile.scale * 2.4f);
                                    Main.dust[num].velocity *= 0.0f;
                                    Main.dust[num].noGravity = true;
                                    Main.dust[num].alpha = 150;
                                    Vector2 O = Projectile.Center;
                                    Vector2 P1 = O;
                                    Vector2 P2 = O + new Vector2(0, -r - k - 8).RotatedBy(Math.PI * n * -(i + 1));
                                    Vector2 P4 = new Vector2(0, -r - k - 8).RotatedBy(Math.PI * n * -(i + 1));
                                    int pl = (int)Player.FindClosest(base.Projectile.Center, 1, 1);
                                    Vector2 P3 = Main.player[pl].Center - O;
                                    float K = (P1.Y - P2.Y) / (P1.X - P2.X);
                                    float D = (float)((P3.X * K - P3.Y) / Math.Sqrt(K * K + 1));
                                    float D2 = P3.Length();
                                    float Sig = P4.X * P3.X + P3.Y * P4.Y;
                                    if (Math.Abs(D) < 15 && D2 < Leng - 52 && Sig > 0)
                                    {
                                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), Main.player[pl].Center.X, Main.player[pl].Center.Y, 0, 0, ModContent.ProjectileType<Hit>(), Projectile.damage, 0f, Main.myPlayer, 0f, 0f);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (Sca > 0.1f)
                    {
                        int G = 8 - Projectile.timeLeft / 4;
                        for (int k = 0; k < Leng - 52; k += 16)
                        {
                            float[] samples = new float[2];
                            Vector2 unit = new Vector2(0, -1).RotatedBy(Math.PI * n * (i + 1));
                            Collision.LaserScan(Projectile.Center, unit, 3f, 1000f, samples);
                            float maxDis = (samples[0] + samples[1]) * 0.5f;
                            Leng = maxDis;
                            int thi = (int)((Sca - 0.1f) * 80f);
                            if (k < Leng - 68)
                            {
                                Main.spriteBatch.Draw(MythContent.QuickTexture("Bosses/EvilBottle/Projectiles/EvilLazer"), base.Projectile.Center - Main.screenPosition + new Vector2(0, -r - k - 8).RotatedBy(Math.PI * n * (i + 1)), new Rectangle(G * 16, 0, 16, 16), new Color((Sca - 0.1f) * 10 / 30f * Projectile.timeLeft, (Sca - 0.1f) * 10 / 30f * Projectile.timeLeft, (Sca - 0.1f) * 10 / 30f * Projectile.timeLeft, 0), -(float)(Math.PI * n * -(i + 1) + Math.PI / 2d), new Vector2(8f, 8f), 1, SpriteEffects.None, 0f);
                            }
                            else
                            {
                                Main.spriteBatch.Draw(MythContent.QuickTexture("Bosses/EvilBottle/Projectiles/EvilLazerEnd"), base.Projectile.Center - Main.screenPosition + new Vector2(0, -r - k - 8).RotatedBy(Math.PI * n * (i + 1)), new Rectangle(G * 16, 0, 16, 16), new Color((Sca - 0.1f) * 10 / 30f * Projectile.timeLeft, (Sca - 0.1f) * 10 / 30f * Projectile.timeLeft, (Sca - 0.1f) * 10 / 30f * Projectile.timeLeft, 0), -(float)(Math.PI * n * -(i + 1) + Math.PI / 2d), new Vector2(8f, 8f), 1, SpriteEffects.None, 0f);
                                if (!Main.gamePaused)
                                {
                                    int num = Dust.NewDust(Projectile.Center + new Vector2(0, -r - k - 8).RotatedBy(Math.PI * n * (i + 1)) - new Vector2(4, 4), 2, 2, ModContent.DustType<DarkF>(), 50f, 50f, 0, default(Color), (float)Projectile.scale * 2.4f / 30f * Projectile.timeLeft);
                                    Main.dust[num].velocity *= 0.0f;
                                    Main.dust[num].noGravity = true;
                                    Main.dust[num].alpha = 150;
                                    Vector2 O = Projectile.Center;
                                    Vector2 P1 = O;
                                    Vector2 P2 = O + new Vector2(0, -r - k - 8).RotatedBy(Math.PI * n * (i + 1));
                                    Vector2 P4 = new Vector2(0, -r - k - 8).RotatedBy(Math.PI * n * (i + 1));
                                    int pl = (int)Player.FindClosest(base.Projectile.Center, 1, 1);
                                    Vector2 P3 = Main.player[pl].Center - O;
                                    float K = (P1.Y - P2.Y) / (P1.X - P2.X);
                                    float D = (float)((P3.X * K - P3.Y) / Math.Sqrt(K * K + 1));
                                    float D2 = P3.Length();
                                    float Sig = P4.X * P3.X + P4.Y * P3.Y;
                                    if (Math.Abs(D) < 15 && D2 < Leng - 52 && Sig > 0)
                                    {
                                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), Main.player[pl].Center.X, Main.player[pl].Center.Y, 0, 0, ModContent.ProjectileType<Hit>(), Projectile.damage, 0f, Main.myPlayer, 0f, 0f);
                                    }
                                }
                            }
                        }
                        for (int k = 0; k < Leng - 52; k += 16)
                        {
                            float[] samples = new float[2];
                            Vector2 unit = new Vector2(0, -1).RotatedBy(Math.PI * n * -(i + 1));
                            Collision.LaserScan(Projectile.Center, unit, 3f, 1000f, samples);
                            float maxDis = (samples[0] + samples[1]) * 0.5f;
                            Leng = maxDis;
                            int thi = (int)((Sca - 0.1f) * 80f);
                            if (k < Leng - 68)
                            {
                                Main.spriteBatch.Draw(MythContent.QuickTexture("Bosses/EvilBottle/Projectiles/EvilLazer"), base.Projectile.Center - Main.screenPosition + new Vector2(0, -r - k - 8).RotatedBy(Math.PI * n * -(i + 1)), new Rectangle(G * 16, 0, 16, 16), new Color((Sca - 0.1f) * 10 / 30f * Projectile.timeLeft, (Sca - 0.1f) * 10 / 30f * Projectile.timeLeft, (Sca - 0.1f) * 10 / 30f * Projectile.timeLeft, 0), -(float)(Math.PI * n * (i + 1) + Math.PI / 2d), new Vector2(8f, 8f), 1, SpriteEffects.None, 0f);
                            }
                            else
                            {
                                Main.spriteBatch.Draw(MythContent.QuickTexture("Bosses/EvilBottle/Projectiles/EvilLazerEnd"), base.Projectile.Center - Main.screenPosition + new Vector2(0, -r - k - 8).RotatedBy(Math.PI * n * -(i + 1)), new Rectangle(G * 16, 0, 16, 16), new Color((Sca - 0.1f) * 10 / 30f * Projectile.timeLeft, (Sca - 0.1f) * 10 / 30f * Projectile.timeLeft, (Sca - 0.1f) * 10 / 30f * Projectile.timeLeft, 0), -(float)(Math.PI * n * (i + 1) + Math.PI / 2d), new Vector2(8f, 8f), 1, SpriteEffects.None, 0f);
                                if (!Main.gamePaused)
                                {
                                    int num = Dust.NewDust(Projectile.Center + new Vector2(0, -r - k - 8).RotatedBy(Math.PI * n * -(i + 1)) - new Vector2(4, 4), 2, 2, ModContent.DustType<DarkF>(), 50f, 50f, 0, default(Color), (float)Projectile.scale * 2.4f / 30f * Projectile.timeLeft);
                                    Main.dust[num].velocity *= 0.0f;
                                    Main.dust[num].noGravity = true;
                                    Main.dust[num].alpha = 150;
                                    Vector2 O = Projectile.Center;
                                    Vector2 P1 = O;
                                    Vector2 P2 = O + new Vector2(0, -r - k - 8).RotatedBy(Math.PI * n * -(i + 1));
                                    Vector2 P4 = new Vector2(0, -r - k - 8).RotatedBy(Math.PI * n * -(i + 1));
                                    int pl = (int)Player.FindClosest(base.Projectile.Center, 1, 1);
                                    Vector2 P3 = Main.player[pl].Center - O;
                                    float K = (P1.Y - P2.Y) / (P1.X - P2.X);
                                    float D = (float)((P3.X * K - P3.Y) / Math.Sqrt(K * K + 1));
                                    float D2 = P3.Length();
                                    float Sig = P4.X * P3.X + P4.Y * P3.Y;
                                    if (Math.Abs(D) < 15 && D2 < Leng - 52 && Sig > 0)
                                    {
                                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), Main.player[pl].Center.X, Main.player[pl].Center.Y, 0, 0, ModContent.ProjectileType<Hit>(), Projectile.damage, 0f, Main.myPlayer, 0f, 0f);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
