using Terraria.Localization;
using Everglow.Sources.Commons.Function.Vertex;
using Terraria.Audio;
using Everglow.Sources.Modules.MythModule.TheFirefly.Dusts;
using Everglow.Sources.Modules.MEACModule;

namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.CrystalStorm
{
    public class CrystalWind : ModProjectile,IWarpProjectile
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
            Projectile.extraUpdates = 3;
            Projectile.timeLeft = 120;
            Projectile.alpha = 0;
            Projectile.penetrate = 1; 
            Projectile.scale = 1f;

            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
        }
        Vector2 AimCenter = Vector2.Zero;
        Vector2 OldAimCenter = Vector2.Zero;
        public override void AI()
        {
            if((OldAimCenter - Main.MouseWorld).Length() > 200 && OldAimCenter != Vector2.Zero)
            {
                if(Projectile.timeLeft > 20)
                {
                    Projectile.timeLeft -= 5;
                }
            }
            AimCenter = Main.MouseWorld;
            OldAimCenter = Main.MouseWorld;
            for (int x = -80; x < 808; x += 8)
            {
                if (Collision.SolidCollision(AimCenter + new Vector2(0, x), 1, 1))
                {
                    AimCenter += new Vector2(0, x);
                    break;
                }
            }
            float Dy = AimCenter.Y - Projectile.position.Y;
            float xCoefficient = Dy * Dy / 600f - 0.4f * Dy + 50;
            Vector2 TrueAim = AimCenter + new Vector2(xCoefficient * (float)(Math.Sin(Main.timeForVisualEffects * 0.1 * Projectile.extraUpdates + Projectile.ai[0])), 0) - Projectile.Center;



            Projectile.alpha = (byte)(Projectile.alpha * 0.95 + xCoefficient * 0.05);

            if (!Main.mouseRight)
            {
                Projectile.velocity = Projectile.velocity * 0.75f + new Vector2(Utils.SafeNormalize(TrueAim, new Vector2(0, 0.05f)).X, -Projectile.ai[1] * 0.3f) * 0.25f / (float)(Projectile.alpha) * 500f;
                Projectile.velocity *= Main.rand.NextFloat(0.85f, 1.15f);
                Projectile.extraUpdates = 1;
            }
            else
            {
                Projectile.velocity = Projectile.velocity * 0.75f + new Vector2(Utils.SafeNormalize(TrueAim, new Vector2(0, 0.05f)).X, -Projectile.ai[1] * 0.3f) * 0.25f / (float)(Projectile.alpha) * 500f;
                Projectile.velocity *= Main.rand.NextFloat(0.85f, 1.15f);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            float k1 = 30f;
            float k0 = (120 - Projectile.timeLeft) / k1;

            if (Projectile.timeLeft <= 120 - k1)
            {
                k0 = 1;
            }

            Color c0 = new Color(0, k0 * k0 * 0.2f, k0 * 0.8f + 0.2f, 0);
            List<Vertex2D> bars = new List<Vertex2D>();
            float width = 24;
            if(Projectile.timeLeft <= 40)
            {
                width = Projectile.timeLeft * 0.6f;
            }

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
            return false;
        }
        public void DrawWarp()
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Effect KEx = ModContent.Request<Effect>("Everglow/Sources/Modules/MEACModule/Effects/DrawWarp", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            KEx.CurrentTechnique.Passes[0].Apply();
            float k1 = (100f + Projectile.ai[0] * 25) * 0.3f;
            float k0 = (1000 - Projectile.timeLeft) / k1;

            if (Projectile.timeLeft <= 1000 - k1)
            {
                k0 = 1;
            }

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