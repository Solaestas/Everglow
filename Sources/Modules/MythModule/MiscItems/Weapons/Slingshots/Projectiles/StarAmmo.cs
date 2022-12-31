﻿using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MythModule.Common;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Slingshots.Projectiles
{
    public class StarAmmo : SlingshotAmmo
    {
        public override void OnSpawn(IEntitySource source)
        {
            Player player = Main.player[Projectile.owner];
            if(player.position.Y > Main.UnderworldLayer * 16f)
            {
                Projectile.CritChance -= 15;
            }
            else
            {
                Projectile.CritChance += 15;
            }
        }
        public override void AI()
        {
            if (TimeTokill >= 0 && TimeTokill <= 2)
            {
                Projectile.Kill();
            }
            if (TimeTokill <= 15 && TimeTokill > 0)
            {
                Projectile.velocity = Projectile.oldVelocity;
            }
            TimeTokill--;
            if (TimeTokill < 0)
            {
                Projectile.velocity.Y += 0.17f;
            }
            else
            {
                if (TimeTokill < 10)
                {
                    Projectile.damage = 0;
                    Projectile.friendly = false;
                }
                Projectile.velocity *= 0f;
            }
        }
        public override void DrawTrail()
        {
            float DrawC = Projectile.ai[0] * Projectile.ai[0];

            List<Vertex2D> bars = new List<Vertex2D>();
            int TrueL = 0;
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                {
                    if (i == 1)
                    {
                        return;
                    }
                    break;
                }

                TrueL = i;
            }
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                {
                    break;
                }

                float width = 6;
                if (Projectile.timeLeft <= 30)
                {
                    width *= Projectile.timeLeft / 30f;
                }
                var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                normalDir = Utils.SafeNormalize(new Vector2(-normalDir.Y, normalDir.X), Vector2.Zero);

                var factor = i / (float)TrueL;
                var color = Color.Lerp(new Color(DrawC * 0.2f, DrawC * 0.2f, DrawC * 0.2f + 0.7f, 0), new Color(0, 0, 0, 0), factor);

                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3(1, 0, 0)));
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(10, 10) - Main.screenPosition, color, new Vector3(1, 1, 0)));

            }

            if (bars.Count > 2)
            {
                Texture2D t = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/EShoot");
                Main.graphics.GraphicsDevice.Textures[0] = t;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            float Power = Projectile.ai[0] * 0.5f + 0.5f;
            Color Light = new Color(Power, Power / 2.1f, 0, 0);
            Texture2D star = MythContent.QuickTexture("MiscItems/Weapons/Slingshots/Projectiles/Textures/SlingshotHitStar");
            float kSize = 1f;
            if(TimeTokill > 0)
            {
                kSize = TimeTokill / 30f;
            }
            Main.spriteBatch.Draw(star, Projectile.Center - Main.screenPosition - Projectile.velocity, null, Light, 0, star.Size() / 2f, new Vector2(0.06f, 0.23f + MathF.Sin((float)(Main.timeForVisualEffects * 0.1)) * 0.2f) * Power * 2f * kSize, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(star, Projectile.Center - Main.screenPosition - Projectile.velocity, null, Light, MathF.PI / 2, star.Size() / 2f, new Vector2(0.06f, 0.23f + MathF.Sin((float)(Main.timeForVisualEffects * 0.1)) * 0.2f) * Power * 2f * kSize, SpriteEffects.None, 0);

            Lighting.AddLight(Projectile.Center, Light.R / 555f, Light.G / 555f, Light.B / 555f);
            return base.PreDraw(ref lightColor);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, 0);
        }
        public override void AmmoHit()
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);
            TimeTokill = 30;
            float DrawC = Projectile.ai[0] + 0.5f;
            Projectile.velocity = Projectile.oldVelocity;
            int StepLength;
            Player player = Main.player[Projectile.owner];
            for (int x = 0; x < ((DrawC + 0.25f) * 2) * 7; x = StepLength + 1)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 58, Projectile.velocity.X * 0.1f * DrawC, Projectile.velocity.Y * 0.1f * DrawC * 2, 150, default(Color), 0.8f);
                StepLength = x;
            }
            for (float x = 0f; x < (DrawC + 0.25f) * 2; x += 0.125f)
            {
                Dust.NewDustPerfect(Projectile.Center, 278, new Vector2?(Vector2.UnitY.RotatedBy((double)(x * 6.28318548f + Main.rand.NextFloat() * 0.5f), default(Vector2)) * (4f + Main.rand.NextFloat() * 4f)) * DrawC * 2, 150, Color.CornflowerBlue, 1f).noGravity = true;
            }
            for (float x = 0f; x < (DrawC + 0.25f) * 2; x += 0.25f)
            {
                Dust.NewDustPerfect(Projectile.Center, 278, new Vector2?(Vector2.UnitY.RotatedBy((double)(x * 6.28318548f + Main.rand.NextFloat() * 0.5f), default(Vector2)) * (2f + Main.rand.NextFloat() * 3f)) * DrawC * 2, 150, Color.Gold, 1f).noGravity = true;
            }
            Vector2 value21 = new Vector2(Main.screenWidth, Main.screenHeight);
            bool flag6 = Projectile.Hitbox.Intersects(Utils.CenteredRectangle(Main.screenPosition + value21 / 2f, value21 + new Vector2(400f)));
            if (flag6)
            {
                for (int x = 0; x < 7; x = StepLength + 1)
                {
                    Gore.NewGore(null, Projectile.position, Main.rand.NextVector2CircularEdge(0.5f, 0.5f) * Projectile.velocity.Length() * DrawC, Utils.SelectRandom<int>(Main.rand, new int[]
                    {
                                    16,
                                    17,
                                    17,
                                    17,
                                    17,
                                    17,
                                    17,
                                    17
                    }), 1f);
                    StepLength = x;
                }
            }
            for (int j = 0; j < 200; j++)
            {
                if ((Main.npc[j].Center - Projectile.Center).Length() < 25 * (1 + DrawC * 3) && !Main.npc[j].dontTakeDamage && !Main.npc[j].friendly)
                {
                    Main.npc[j].StrikeNPC((int)(Projectile.damage * Main.rand.NextFloat(0.85f, 1.15f)), 2, Math.Sign(Projectile.velocity.X), Main.rand.Next(100) < Projectile.ai[0]);
                    player.addDPS((int)(Projectile.damage * (1 + Projectile.ai[0] / 100f) * 1f));
                }
            }
            Projectile.friendly = false;
            Projectile.velocity *= 0f;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (!Main.dayTime)
            {
                damage = (int)(damage * 1.25f);
            }
            else
            {
                damage = (int)(damage * 0.75f);
            }
        }
    }
}
