using Everglow.Sources.Commons.Function.Vertex;

namespace Everglow.Sources.Modules.MythModule.MiscProjectiles.Weapon.Legendary
{
    public class Chaos : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("混乱粒子");
        }
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 255;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 150;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
        }
        private float omega = 0;
        private int u = 0;
        int TrueL = 1;
        public override void AI()
        {
            ka = 0.2f;
            if (Projectile.timeLeft < 60f)
            {
                ka = Projectile.timeLeft / 300f;
            }
            if (Projectile.timeLeft < 40 && Projectile.timeLeft % 2 == 0)
            {
                u -= 1;
            }
            if (omega == 0)
            {
                omega = Main.rand.NextFloat(-0.035f, 0.035f);
                Projectile.timeLeft = Main.rand.Next(120, 175);
            }
            if (omega < -0.065f)
            {
                omega += 0.001f;
            }
            if (omega > 0.065f)
            {
                omega -= 0.001f;
            }
            if (omega >= -0.065f && omega <= 0.065f)
            {
                omega += Main.rand.NextFloat(-0.015f, 0.015f) / 6f;
            }
            Projectile.velocity = Projectile.velocity.RotatedBy(omega);
            Lighting.AddLight(Projectile.Center, (float)(255 - Projectile.alpha) * 0.5f / 255f, (float)(255 - Projectile.alpha) * 0f / 255f, (float)(255 - Projectile.alpha) * 0.2f / 255f);
            if (Projectile.ai[0] == 0f)
            {
                Projectile.ai[0] = 1f;
            }
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 25;
            }
            if (Projectile.alpha < 0)
            {
                Projectile.alpha = 0;
            }
            float num = 50f;
            float num2 = 1.5f;
            if (Projectile.ai[1] == 0f)
            {
                Projectile.localAI[0] += num2;
                if (Projectile.localAI[0] > num)
                {
                    Projectile.localAI[0] = num;
                    return;
                }
            }
            else
            {
                Projectile.localAI[0] -= num2;
                if (Projectile.localAI[0] <= 0f)
                {
                    Projectile.Kill();
                }
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, 0));
        }
        float ka = 0;
        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            List<Vertex2D> bars = new List<Vertex2D>();
            float width = 2;
            if (Projectile.timeLeft < 60)
            {
                width = Projectile.timeLeft / 30f;
            }
            TrueL = 0;
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;
                TrueL++;
            }
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;
                var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
                var factor = 1f;
                factor = i / (float)TrueL;
                var w = MathHelper.Lerp(1f, 0.05f, factor);
                Lighting.AddLight(Projectile.oldPos[i], (float)(255 - Projectile.alpha) * 0.5f / 50f * ka * (1 - factor), (float)(255 - Projectile.alpha) * 0.8f / 50f * ka * (1 - factor), (float)(255 - Projectile.alpha) * 0.8f / 50f * ka * (1 - factor));
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(2f, 2f) - Main.screenPosition, new Color(254, 254, 254, 0), new Vector3(factor, 1, w)));
                bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(2f, 2f) - Main.screenPosition, new Color(254, 254, 254, 0), new Vector3(factor, 0, w)));
            }
            List<Vertex2D> Vx = new List<Vertex2D>();
            if (bars.Count > 2)
            {
                Vx.Add(bars[0]);
                var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f + Vector2.Normalize(Projectile.velocity), new Color(254, 254, 254, 0), new Vector3(0, 0.5f, 1));
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
            Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscProjectiles/Weapon/Legendary/ChaosLine").Value;
            Main.graphics.GraphicsDevice.Textures[0] = t;//GlodenBloodScaleMirror
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
            return true;
        }
    }
}
