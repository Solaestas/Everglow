using Everglow.Sources.Commons.Core;
using Everglow.Sources.Commons.Core.VFX;
using Everglow.Sources.Commons.Core.VFX.Pipelines;
using Everglow.Sources.Commons.Core.VFX.Visuals;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MythModule.Common;

namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.BookofSkulls
{
    [Pipeline(typeof(WCSPipeline))]
    internal class SkullSpriteColdFlame : Visual
    {
        public Vector2[] positon = new Vector2[20];
        public Vector2 velocity;
        public int timeLeft;
        public float size;
        public float omega;

        public override void Update()
        {
            for (int y = 19; y > 0; y--)
            {
                positon[y] = positon[y - 1];
            }
            positon[0] += velocity;
            timeLeft -= 1;
            if (timeLeft <= 0)
            {
                Kill();
            }
            if (timeLeft <= 30)
            {
                velocity *= 0.98f;
            }
            velocity = velocity.RotatedBy(omega);
            omega += Main.rand.NextFloat(-0.05f, 0.05f);
            if (Math.Abs(omega) > 0.15)
            {
                omega *= 0.98f;
            }
            base.Update();
        }

        public override void Draw()
        {
            List<Vertex2D> bars = new List<Vertex2D>();
            List<Vertex2D> barsF = new List<Vertex2D>();
            int TrueL = 0;
            for (int i = 1; i < 20; ++i)
            {
                if (positon[i] == Vector2.Zero)
                {
                    break;
                }

                TrueL++;
            }
            for (int i = 1; i < TrueL; ++i)
            {
                if (positon[i] == Vector2.Zero)
                {
                    break;
                }

                var normalDir = positon[i - 1] - positon[i];
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
                var factor = i / (float)TrueL;
                var w = MathHelper.Lerp(1f, 0.05f, factor);
                float x0 = (10000 - factor - (float)Main.timeForVisualEffects * 0.01f) % 1f;
                float x1 = (10000 - factor - (float)Main.timeForVisualEffects * 0.02f) % 1f;
                float c0 = 1f;
                float c1 = 0.7f;
                float width = (float)(Math.Sin(factor * Math.PI) * factor) * size * 19;
                if (timeLeft < 30)
                {
                    c1 *= timeLeft / 30f;
                    c0 *= timeLeft / 30f;
                }
                bars.Add(new Vertex2D(positon[i] + normalDir * width, new Color(c0, c0, c0, c0), new Vector3(x0, 0.6f, w)));
                bars.Add(new Vertex2D(positon[i] + normalDir * -width, new Color(c0, c0, c0, c0), new Vector3(x0, 0.4f, w)));
                barsF.Add(new Vertex2D(positon[i] + normalDir * width, new Color(c1, c1, c1, 0), new Vector3(x1, 0.6f, w)));
                barsF.Add(new Vertex2D(positon[i] + normalDir * -width, new Color(c1, c1, c1, 0), new Vector3(x1, 0.4f, w)));
            }
            Texture2D t = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/BookofSkulls/SkullFlameLine");
            Main.graphics.GraphicsDevice.Textures[0] = t;
            if (bars.Count > 3)
            {
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
            }

            Texture2D t2 = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/BookofSkulls/GoldLine");
            Main.graphics.GraphicsDevice.Textures[0] = t2;
            if (barsF.Count > 3)
            {
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, barsF.ToArray(), 0, barsF.Count - 2);
            }

            //Dust.NewDust(positon[0],0,0, DustID.Torch);
        }

        public override CallOpportunity DrawLayer => CallOpportunity.PostDrawNPCs;
    }
}