using Everglow.Sources.Modules.MythModule.TheFirefly.Dusts;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MEACModule;
using Everglow.Sources.Commons.Core.VFX;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.DreamWeaver
{
    public class DreamWeaverII : ModProjectile, IWarpProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 240;
            Projectile.alpha = 0;
            Projectile.penetrate = 30;
            Projectile.scale = 1f;
            Projectile.DamageType = DamageClass.MagicSummonHybrid;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50;
        }
        internal int breakTime = 200;
        internal int Aimnpc = -1;
        public override void OnSpawn(IEntitySource source)
        {
            breakTime = Main.rand.Next(200,232);
        }
        public override void AI()
        {           
            float MinDis = 350;
            foreach (var target in Main.npc)
            {
                if (target.active && Main.rand.NextBool(2))
                {
                    if (!target.dontTakeDamage && !target.friendly && target.CanBeChasedBy())
                    {
                        Vector2 ToTarget = target.Center - Projectile.Center;
                        float dis = ToTarget.Length();
                        if (dis < 250 && ToTarget != Vector2.Zero)
                        {
                            if (dis < MinDis)
                            {
                                MinDis = dis;
                                Aimnpc = target.whoAmI;
                            }
                        }
                    }
                }
            }
            if (Projectile.ai[0] == 3)
            {
                if(Projectile.velocity.Length() > 5f)
                {
                    Projectile.velocity *= 0.95f;
                }
            }
            float kTime = 1f;
            if (Projectile.timeLeft < 90f)
            {
                kTime = Projectile.timeLeft / 90f;
            }
            Lighting.AddLight((int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16), 0.32f * kTime, 0.23f * kTime, 0);
            if (Aimnpc == -1)
            {
                if (Projectile.timeLeft >= breakTime && Projectile.ai[0] != 3 || Projectile.ai[0] == 3)
                {
                    Projectile.velocity.Y += 0.15f;
                }
            }
            else
            {
                NPC npc = Main.npc[Aimnpc];
                if (npc.active && (npc.Center - Projectile.Center).Length() < 350)
                {
                    Vector2 v0 = npc.Center - Projectile.Center;
                    Projectile.velocity += Vector2.Normalize(v0).RotatedBy(Projectile.ai[1]) * 0.5f;

                    Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 5f;
                }
                else if(Projectile.timeLeft >= breakTime && Projectile.ai[0] != 3 || Projectile.ai[0] == 3)
                {
                    Projectile.velocity.Y += 0.15f;
                }
            }
            if (Projectile.timeLeft < breakTime && Projectile.ai[0] != 3)
            {
                if (Projectile.timeLeft == breakTime - 1)
                {
                    for (int x = 0; x < 6; x++)
                    {
                        Vector2 velocity = new Vector2(0, Main.rand.NextFloat(2f, 12f)).RotatedByRandom(6.283);
                        Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + velocity * -2 - Projectile.velocity * 4, velocity + Projectile.velocity, ModContent.ProjectileType<DreamWeaverII>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 3f, Main.rand.NextFloat(0,0.75f)/*If ai[0] equal to 3, another ai will be execute*/);
                        SoundEngine.PlaySound(SoundID.Item54);
                        p.friendly = false;
                        p.damage = Projectile.damage / 4;
                        p.timeLeft = 240;
                        p.extraUpdates = 3;
                    }
                    for (int x = 0; x < 5; x++)
                    {
                        Vector2 BasePos = Projectile.Center - new Vector2(4) - Projectile.velocity;
                        Dust d0 = Dust.NewDustDirect(BasePos, 0, 0, ModContent.DustType<BlueGlowAppearStoppedByTile>(), 0, 0, 0, default, 0.6f);
                        d0.noGravity = true;
                    }
                }
                Projectile.velocity *= 0;
                return;
            }

            if (Main.rand.NextBool((int)Math.Pow(Projectile.extraUpdates + Projectile.ai[0], 3)))
            {
                if (Projectile.extraUpdates == 1)
                {
                    if (Projectile.timeLeft % 3 == 0)
                    {
                        int index = Dust.NewDust(Projectile.position - new Vector2(2), Projectile.width, Projectile.height, ModContent.DustType<BlueGlowAppearStoppedByTile>(), 0f, 0f, 100, default, Main.rand.NextFloat(0.7f, 1.9f));
                        Main.dust[index].velocity = Projectile.velocity * 0.5f;
                    }
                    int index2 = Dust.NewDust(Projectile.position - new Vector2(2), Projectile.width, Projectile.height, ModContent.DustType<BlueParticleDark2StoppedByTile>(), 0f, 0f, 0, default, Main.rand.NextFloat(3.7f, 5.1f));
                    Main.dust[index2].velocity = Projectile.velocity * 0.5f;
                    Main.dust[index2].alpha = (int)(Main.dust[index2].scale * 50);
                }
            }
            if(Projectile.timeLeft < 239)
            {
                if (Collision.SolidCollision(Projectile.Center, 0, 0))
                {
                    Projectile.velocity *= 0.1f;

                    if (Projectile.extraUpdates == 1)
                    {
                        for (int x = 0; x < 15 / (Projectile.ai[0] + 1); x++)
                        {
                            Vector2 BasePos = Projectile.Center - new Vector2(4) - Projectile.velocity;
                            Dust d1 = Dust.NewDustDirect(BasePos, 0, 0, ModContent.DustType<BlueGlowAppearStoppedByTile>(), 0, 0, 0, default, 0.6f);
                            d1.velocity = new Vector2(0, Main.rand.NextFloat(0f, 3f)).RotatedByRandom(6.283);
                            d1.noGravity = true;
                        }
                        if (Projectile.ai[0] != 3)
                        {
                            for (int x = 0; x < 3; x++)
                            {
                                Vector2 velocity = new Vector2(0, Main.rand.NextFloat(2f, 6f)).RotatedByRandom(6.283) - Projectile.velocity * 0.2f;
                                Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + velocity * -2 - Projectile.velocity * 2, velocity, ModContent.ProjectileType<DreamWeaverII>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 3f/*If ai[0] equal to 3, another ai will be execute*/);
                                SoundEngine.PlaySound(SoundID.Item54);
                                p.friendly = false;
                            }
                        }
                        Projectile.extraUpdates = 2;
                        SoundEngine.PlaySound(SoundID.Drip, Projectile.Center);
                    }
                }
                else
                {
                    if (Projectile.extraUpdates == 2)
                    {
                        Projectile.extraUpdates = 1;
                    }
                }
            }
            if(Projectile.timeLeft == 210)
            {
                Projectile.friendly = true;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            SoundEngine.PlaySound(SoundID.Drip, Projectile.Center);
            for (int x = 0; x < 3; x++)
            {
                Vector2 BasePos = Projectile.Center - new Vector2(4) - Projectile.velocity;
                Dust d0 = Dust.NewDustDirect(BasePos, 0, 0, ModContent.DustType<BlueGlowAppearStoppedByTile>(), 0, 0, 0, default, 0.6f);
                d0.noGravity = true;
            }
            if(Projectile.ai[0] != 3)
            {
                for (int x = 0; x < 3; x++)
                {
                    Vector2 velocity = new Vector2(0, Main.rand.NextFloat(2f, 6f)).RotatedByRandom(6.283) - Projectile.velocity * 0.2f;
                    Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + velocity * -2, velocity, ModContent.ProjectileType<DreamWeaverII>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 3f/*If ai[0] equal to 3, another ai will be execute*/);
                    SoundEngine.PlaySound(SoundID.Item54);
                    p.friendly = false;
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            float k1 = 60f;
            float k0 = (240 - Projectile.timeLeft) / k1;

            if (Projectile.timeLeft <= 240 - k1)
            {
                k0 = 1;
            }

            Color c0 = new Color(0, k0 * k0 * 0.4f + 0.2f, k0 * 0.8f + 0.2f, 0);
            List<Vertex2D> bars = new List<Vertex2D>();


            int TrueL = 0;
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                {
                    break;
                }

                TrueL++;
            }
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                float width = 36;
                if (Projectile.timeLeft <= 40)
                {
                    width = Projectile.timeLeft * 0.9f;
                }
                if (i < 10)
                {
                    width *= i / 10f;
                }
                if (Projectile.ai[0] == 3)
                {
                    width *= 0.5f;
                }
                if (Projectile.oldPos[i] == Vector2.Zero)
                {
                    break;
                }

                var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
                var factor = i / (float)TrueL;
                var w = MathHelper.Lerp(1f, 0.05f, factor);
                float x0 = factor * 0.6f - (float)(Main.timeForVisualEffects / 35d) + 10000;
                x0 %= 1f;
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width * (1 - factor) + new Vector2(5f, 5f) - Main.screenPosition, c0, new Vector3(x0, 1, w)));
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width * (1 - factor) + new Vector2(5f, 5f) - Main.screenPosition, c0, new Vector3(x0, 0, w)));
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Texture2D t = Common.MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/GoldLine");
            Main.graphics.GraphicsDevice.Textures[0] = t;
            if (bars.Count > 3)
            {
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }

        public void DrawWarp(VFXBatch spriteBatch)
        {
            float width = 16;

            int TrueL = 0;
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                {
                    break;
                }
                TrueL++;
            }
            List<Vertex2D> bars = new List<Vertex2D>();
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                {
                    break;
                }
                float MulColor = 1f;
                var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
                if (i == 1)
                {
                    MulColor = 0f;
                }
                if (i >= 2)
                {
                    var normalDirII = Projectile.oldPos[i - 2] - Projectile.oldPos[i - 1];
                    normalDirII = Vector2.Normalize(new Vector2(-normalDirII.Y, normalDirII.X));
                    if (Vector2.Dot(normalDirII, normalDir) <= 0.965f)
                    {
                        MulColor = 0f;
                    }
                }
                if (i < Projectile.oldPos.Length - 1)
                {
                    var normalDirII = Projectile.oldPos[i] - Projectile.oldPos[i + 1];
                    normalDirII = Vector2.Normalize(new Vector2(-normalDirII.Y, normalDirII.X));
                    if (Vector2.Dot(normalDirII, normalDir) <= 0.965f)
                    {
                        MulColor = 0f;
                    }
                }

                float k0 = (float)(Math.Atan2(normalDir.Y, normalDir.X));
                k0 += 3.14f + 1.57f;
                if (k0 > 6.28f)
                {
                    k0 -= 6.28f;
                }
                Color c0 = new Color(k0, 0.4f, 0, 0);

                var factor = i / (float)TrueL;
                float x0 = factor * 1.3f - (float)(Main.timeForVisualEffects / 15d) + 100000;
                x0 %= 1f;
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width * (1 - factor) + new Vector2(5f) - Main.screenPosition, c0 * MulColor, new Vector3(x0, 1, 0)));
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width * (1 - factor) + new Vector2(5f) - Main.screenPosition, c0 * MulColor, new Vector3(x0, 0, 0)));
                var factorII = factor;
                factor = (i + 1) / (float)TrueL;
                var x1 = factor * 1.3f - (float)(Main.timeForVisualEffects / 15d) + 100000;
                x1 %= 1f;
                if (x0 > x1)
                {
                    float DeltaValue = 1 - x0;
                    var factorIII = factorII * x0 + factor * DeltaValue;
                    bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width * (1 - factorIII) + new Vector2(5f) - Main.screenPosition, c0 * MulColor, new Vector3(1, 1, 0)));
                    bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width * (1 - factorIII) + new Vector2(5f) - Main.screenPosition, c0 * MulColor, new Vector3(1, 0, 0)));
                    bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width * (1 - factorIII) + new Vector2(5f) - Main.screenPosition, c0 * MulColor, new Vector3(0, 1, 0)));
                    bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width * (1 - factorIII) + new Vector2(5f) - Main.screenPosition, c0 * MulColor, new Vector3(0, 0, 0)));
                }
            }
            Texture2D t = Common.MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/FogTraceLight");

            if (bars.Count > 3)
            {
                spriteBatch.Draw(t, bars, PrimitiveType.TriangleStrip);
            }
        }
    }
}