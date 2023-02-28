using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Commons.Core.VFX;
using Everglow.Sources.Modules.MEACModule;
using Terraria.DataStructures;
using Terraria.Audio;

namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.CrystalStorm
{
    public class CrystalStormII : ModProjectile, IWarpProjectile//���ӿڸ�Ϊʹ��IWarpProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 120;
            Projectile.alpha = 0;
            Projectile.penetrate = 5;
            Projectile.scale = 1f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.DamageType = DamageClass.Magic;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.ai[0] = Main.rand.NextFloat(0f, 1f);
            Projectile.position.Y -= 15f;
        }
        public override void AI()
        {
            float MulScale = 1f;
            if (Projectile.timeLeft < 30)
            {
                MulScale = Projectile.timeLeft / 30f;
            }
            if(Main.rand.NextBool(5))
            {
                Vector2 BasePos = Projectile.Center - new Vector2(4) - Projectile.velocity + new Vector2(0, Main.rand.NextFloat(6f)).RotatedByRandom(6.283);
                Dust d0 = Dust.NewDustDirect(BasePos, 0, 0, DustID.CrystalPulse2, 0, 0, 0, default, 0.8f * MulScale);
                d0.noGravity = true;
                d0.velocity = Projectile.velocity * 0.2f + new Vector2(0, Main.rand.NextFloat(2f)).RotatedByRandom(6.283);
            }
            if (Main.rand.NextBool(3))
            {
                Vector2 v0 = new Vector2(Main.rand.NextFloat(9, 11f), 0).RotatedByRandom(6.283) * Projectile.scale * MulScale * 0.2f;
                int dust0 = Dust.NewDust(Projectile.Center - Projectile.velocity * 3 + Vector2.Normalize(Projectile.velocity) * 16f - new Vector2(4), 0, 0, ModContent.DustType<Dusts.CrystalAppearStoppedByTile>(), v0.X, v0.Y, 100, default(Color), Main.rand.NextFloat(0.1f, 1f) * Projectile.scale * MulScale);
                Main.dust[dust0].noGravity = true;
                Main.dust[dust0].alpha = 175;
            }


            Projectile.velocity *= 0.99f;
            if (Projectile.penetrate != 1 && Projectile.friendly)
            {
                Lighting.AddLight((int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16), 0.5f * Projectile.ai[0] + 0.1f, 0.3f, 0.7f * (1 - Projectile.ai[0]) + 0.3f);

                Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);

            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D Light = Common.MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/CrystalStorm/CrystalStormII");

            Color c0 = new Color(0.5f * Projectile.ai[0] + 0.1f, 0.3f, 0.7f * (1 - Projectile.ai[0]) + 0.3f, 0);
            Color c1 = new Color(1f, 1f, 1f, 0.2f);
            if (Projectile.timeLeft < 30)
            {
                c1 *= Projectile.timeLeft / 30f;
            }


            float width = 4;
           
            int TrueL = 0;
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                {
                    break;
                }
                TrueL++;
            }


            DrawFlameTrail(TrueL, width, true, Color.White * 0.2f);

            DrawFlameTrail(TrueL, width, false, c0);

            if(Projectile.penetrate != 1 && Projectile.friendly)
            {
                if (Projectile.velocity.X < 0)
                {
                    Main.spriteBatch.Draw(Light, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY) - Projectile.velocity, null, c1, Projectile.rotation, Light.Size() / 2f, Projectile.scale, SpriteEffects.FlipVertically, 0);
                }
                else
                {
                    Main.spriteBatch.Draw(Light, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY) - Projectile.velocity, null, c1, Projectile.rotation, Light.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
                }
            }
            return false;
        }
        private void DrawFlameTrail(int TrueL, float width, bool Shade = false, Color c0 = new Color(), float Mulfactor = 1.6f)
        {
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
                var factor = i / (float)TrueL;
                float x0 = factor * Mulfactor - (float)(Main.timeForVisualEffects / 15d) + 100000;
                x0 %= 1f;
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width * (1 - factor) + new Vector2(13f) - Main.screenPosition, c0 * MulColor, new Vector3(x0, 1, 0)));
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width * (1 - factor) + new Vector2(13f) - Main.screenPosition, c0 * MulColor, new Vector3(x0, 0, 0)));
                var factorII = factor;
                factor = (i + 1) / (float)TrueL;
                var x1 = factor * Mulfactor - (float)(Main.timeForVisualEffects / 15d) + 100000;
                x1 %= 1f;
                if (x0 > x1)
                {
                    float DeltaValue = 1 - x0;
                    var factorIII = factorII * x0 + factor * DeltaValue;
                    bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width * (1 - factorIII) + new Vector2(13f) - Main.screenPosition, c0 * MulColor, new Vector3(1, 1, 0)));
                    bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width * (1 - factorIII) + new Vector2(13f) - Main.screenPosition, c0 * MulColor, new Vector3(1, 0, 0)));
                    bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width * (1 - factorIII) + new Vector2(13f) - Main.screenPosition, c0 * MulColor, new Vector3(0, 1, 0)));
                    bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width * (1 - factorIII) + new Vector2(13f) - Main.screenPosition, c0 * MulColor, new Vector3(0, 0, 0)));
                }
            }
            Texture2D t = Common.MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/ElecLine");
            if(Shade)
            {
                t = Common.MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/Darkline");
            }
            Main.graphics.GraphicsDevice.Textures[0] = t;

            if (bars.Count > 3)
            {
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
            }
        }
        public void DrawWarp(VFXBatch spriteBatch)
        {
            
            /*�����ÿ���shader
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Effect KEx = ModContent.Request<Effect>("Everglow/Sources/Modules/MEACModule/Effects/DrawWarp", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            KEx.CurrentTechnique.Passes[0].Apply();
            */
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
                if (i ==1)
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
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width * (1 - factor) + new Vector2(13f) - Main.screenPosition, c0 * MulColor, new Vector3(x0, 1, 0)));
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width * (1 - factor) + new Vector2(13f) - Main.screenPosition, c0 * MulColor, new Vector3(x0, 0, 0)));
                var factorII = factor;
                factor = (i + 1) / (float)TrueL;
                var x1 = factor * 1.3f - (float)(Main.timeForVisualEffects / 15d) + 100000;
                x1 %= 1f;
                if (x0 > x1)
                {
                    float DeltaValue = 1 - x0;
                    var factorIII = factorII * x0 + factor * DeltaValue;
                    bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width * (1 - factorIII) + new Vector2(13f) - Main.screenPosition, c0 * MulColor, new Vector3(1, 1, 0)));
                    bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width * (1 - factorIII) + new Vector2(13f) - Main.screenPosition, c0 * MulColor, new Vector3(1, 0, 0)));
                    bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width * (1 - factorIII) + new Vector2(13f) - Main.screenPosition, c0 * MulColor, new Vector3(0, 1, 0)));
                    bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width * (1 - factorIII) + new Vector2(13f) - Main.screenPosition, c0 * MulColor, new Vector3(0, 0, 0)));
                }
            }
            Texture2D t = Common.MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/FogTraceLight");

            //��ͼ���������ﴫ
            //Main.graphics.GraphicsDevice.Textures[0] = t;

            if (bars.Count > 3)
            {
                //Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

                //��������л��ƣ�����������ƣ���Ϊ��VFXBatch spriteBatch��Draw������
                spriteBatch.Draw(t, bars, PrimitiveType.TriangleStrip);
            }

            //��β��begin-endҲɾ��
            //Main.spriteBatch.End();
            //Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            
            SoundEngine.PlaySound(SoundID.DD2_CrystalCartImpact.WithVolumeScale(0.2f), Projectile.Center);
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            Projectile.velocity *= 0.6f;
            Projectile.penetrate--;

            return false;
        }     
    }
}