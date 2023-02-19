using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MythModule.Common;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Projectiles
{
    public class DarkEffect : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("");
        }

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.timeLeft = 60;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 60;
        }

        public override void AI()
        {
            Projectile.velocity *= 0.9f;
        }

        private int TrueL = 1;

        public override void PostDraw(Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            List<Vertex2D> bars = new List<Vertex2D>();
            float width = Projectile.ai[0];
            if (Projectile.timeLeft < 60)
            {
                width = Projectile.timeLeft / (60f / Projectile.ai[0]);
            }
            TrueL = 0;
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
                var factor = 1f;
                if (Projectile.oldPos.Length > 0)
                {
                    factor = i / (float)TrueL;
                }
                var w = MathHelper.Lerp(1f, 0.05f, factor);
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(0.5f, 0.5f) - Main.screenPosition, Color.White, new Vector3(factor, 1, w)));
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(0.5f, 0.5f) - Main.screenPosition, Color.White, new Vector3(factor, 0, w)));
            }
            List<Vertex2D> Vx = new List<Vertex2D>();
            if (bars.Count > 2)
            {
                Vx.Add(bars[0]);
                var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f + Vector2.Normalize(Projectile.velocity) * 1, Color.White, new Vector3(0, 0.5f, 1));
                Vx.Add(bars[1]);
                Vx.Add(vertex);
                for (int i = 0; i < bars.Count - 2; i += 2)
                {
                    Vx.Add(bars[i]);
                    Vx.Add(bars[i + 2]);
                    Vx.Add(bars[i + 1]);

                    Vx.Add(bars[i + 1]);
                    Vx.Add(bars[i + 2]);
                    Vx.Add(bars[i + 3]);
                }
            }
            Texture2D t = MythContent.QuickTexture("TheFirefly/Projectiles/EvilChrysalisTex/BlackLine");
            Main.graphics.GraphicsDevice.Textures[0] = t;//GoldenBloodScaleMirror
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
        }
    }
}