using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MEACModule;
using Terraria.Audio;

namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.GoldenShower
{
    public class GoldenShowerII : ModProjectile, IWarpProjectile
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
            Projectile.penetrate = 1;
            Projectile.scale = 1f;
            Projectile.DamageType = DamageClass.MagicSummonHybrid;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
        }

        public override void AI()
        {
            float kTime = 1f;
            if(Projectile.timeLeft < 90f)
            {
                kTime = Projectile.timeLeft / 90f;
            }
            Lighting.AddLight((int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16), 0.32f * kTime, 0.23f * kTime, 0);
            Vector2 BasePos = Projectile.Center - new Vector2(4) - Projectile.velocity;
            Dust d0 = Dust.NewDustDirect(BasePos, 0, 0, DustID.Ichor, 0, 0, 0, default, 0.6f / (Projectile.ai[0] + 1));
            d0.noGravity = true;
            d0.velocity *= 0;
            Projectile.velocity.Y += 0.15f;
            if(Projectile.timeLeft < 239)
            {
                if (Collision.SolidCollision(Projectile.Center, 0, 0))
                {
                    Projectile.velocity *= 0.1f;

                    if (Projectile.extraUpdates == 1)
                    {
                        for (int x = 0; x < 15; x++)
                        {
                            BasePos = Projectile.Center - new Vector2(4) - Projectile.velocity;
                            Dust d1 = Dust.NewDustDirect(BasePos, 0, 0, DustID.Ichor, 0, 0, 0, default, 0.6f);
                            d1.noGravity = true;
                        }
                        if (Projectile.ai[0] != 3)
                        {
                            for (int x = 0; x < 3; x++)
                            {
                                Vector2 velocity = new Vector2(0, Main.rand.NextFloat(2f, 6f)).RotatedByRandom(6.283) - Projectile.velocity * 0.2f;
                                Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + velocity * -2 - Projectile.velocity * 2, velocity, ModContent.ProjectileType<GoldenShowerII>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 3f/*If ai[0] equal to 3, another ai will be execute*/);
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
            for (int x = 0; x < 15; x++)
            {
                Vector2 BasePos = Projectile.Center - new Vector2(4) - Projectile.velocity;
                Dust d0 = Dust.NewDustDirect(BasePos, 0, 0, DustID.Ichor, 0, 0, 0, default, 0.6f);
                d0.noGravity = true;
            }
            if(Projectile.ai[0] != 3)
            {
                for (int x = 0; x < 3; x++)
                {
                    Vector2 velocity = new Vector2(0, Main.rand.NextFloat(2f, 6f)).RotatedByRandom(6.283) - Projectile.velocity * 0.2f;
                    Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + velocity * -2, velocity, ModContent.ProjectileType<GoldenShowerII>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 3f/*If ai[0] equal to 3, another ai will be execute*/);
                    p.friendly = false;
                }
            }
            target.AddBuff(BuffID.Ichor, 600);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            float k1 = 60f;
            float k0 = (240 - Projectile.timeLeft) / k1;

            if (Projectile.timeLeft <= 240 - k1)
            {
                k0 = 1;
            }

            Color c0 = new Color(k0 * 0.8f + 0.2f, k0 * k0 * 0.4f + 0.2f, 0f, 0);
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

        public void DrawWarp()
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Effect KEx = ModContent.Request<Effect>("Everglow/Sources/Modules/MEACModule/Effects/DrawWarp", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            KEx.CurrentTechnique.Passes[0].Apply();

            Color c0 = new Color(0.2f, 0.2f, 0f);
            List<Vertex2D> bars = new List<Vertex2D>();
            float width = 24;

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
            Texture2D t = Common.MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/GoldLine");
            Main.graphics.GraphicsDevice.Textures[0] = t;
            if (bars.Count > 3)
            {
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}