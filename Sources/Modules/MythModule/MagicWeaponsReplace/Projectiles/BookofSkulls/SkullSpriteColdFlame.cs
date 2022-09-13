using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Commons.Core.VFX.Visuals;
using Everglow.Sources.Commons.Core.VFX.Pipelines;
using Everglow.Sources.Commons.Core;

namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.BookofSkulls
{
    internal class SkullSpriteColdFlame : Visual
    {
        public Vector2[] positon = new Vector2[20];
        public Vector2 velocity;
        public int timeLeft;
        public float size;
        public override void Update()
        {
            for (int y = 19; y > 0; y--)
            {
                positon[y] = positon[y - 1];
            }
            positon[0] += velocity;
            timeLeft -= 1;
            if(timeLeft <= 0)
            {
                Kill();
            }
            base.Update();
        }
        public override void Draw()
        {
            List<Vertex2D> bars = new List<Vertex2D>();
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
                float x0 = 1 - factor;
                float width = (float)(Math.Sin(factor * Math.PI) * factor) * size;
                if(timeLeft < 100)
                {
                    width *= timeLeft / 100f;
                }
                bars.Add(new Vertex2D(positon[i] + normalDir * -width + new Vector2(5f, 5f) - Main.screenPosition, Color.White, new Vector3(x0, 1, w)));
                bars.Add(new Vertex2D(positon[i] + normalDir * width + new Vector2(5f, 5f) - Main.screenPosition, Color.White, new Vector3(x0, 0, w)));

            }
            Texture2D t = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/BookofSkulls/SkullFlameLine");
            Main.graphics.GraphicsDevice.Textures[0] = t;
            if (bars.Count > 3)
            {
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
            }
        }
        public override CallOpportunity DrawLayer => CallOpportunity.PostDrawNPCs;
    }
}
