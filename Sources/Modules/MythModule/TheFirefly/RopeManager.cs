
using Everglow.Sources.Commons.Function.ImageReader;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MythModule.TheFirefly.Physics;

using SixLabors.ImageSharp.PixelFormats;

using Terraria.GameContent;

namespace Everglow.Sources.Modules.MythModule.TheFirefly
{
    internal class RopeManager
    {
        private struct Rope
        {
            public Mass[] mass;
            public Spring[] spring;
            public Rope(Vector2 position, float scale, int count) : this()
            {
                mass = new Mass[count];
                spring = new Spring[count - 1];

                mass[0] = new Mass(scale * Main.rand.NextFloat(0.25f, 0.37f), position, true);
                mass[^1] = new Mass(scale * Main.rand.NextFloat(0.25f, 0.37f) * 1.3f, position + new Vector2(0, 6 * count - 6), false);
                for (int i = 1; i < count - 1; i++)
                {
                    mass[i] = new Mass(scale * Main.rand.NextFloat(0.25f, 0.37f), position + new Vector2(0, 6 * i), false);
                }

                for (int i = 0; i < count - 1; i++)
                {
                    spring[i] = new Spring(0.3f, 20, 0.05f, mass[i], mass[i + 1]);
                }
            }

        }
        private float luminance = 1;
        private float gravity = 1;
        private List<Rope> ropes = new List<Rope>(100);

        public void LoadRope(string ropeImagePath, Rectangle? rectangle, Vector2 basePosition)
        {
            var image = ImageReader.Read<Rgb24>(ropeImagePath);
            Rectangle rect;
            rect = rectangle ?? new Rectangle(0, 0, image.Width, image.Height);
            image.ProcessPixelRows(accessor =>
            {
                for (int j = 0; j < rect.Height; j++)
                {
                    var span = accessor.GetRowSpan(j + rect.X);
                    for (int i = 0; i < rect.Width; i++)
                    {
                        var pixel = span[i + rect.X];
                        if (pixel.R == 255)
                        {
                            ropes.Add(new Rope(new Vector2(i * 5, j * 5) + basePosition, (pixel.B + 140) / 300f, pixel.G));
                        }
                    }
                }

            });
        }
        public void Update(float deltaTime)
        {
            for (int i = 0; i < ropes.Count; i++)
            {
                var rope = ropes[i];
                foreach (var s in rope.spring)
                {
                    s.ApplyForce(deltaTime);
                }
                foreach (var m in rope.mass)
                {
                    m.force += new Vector2(0.02f + 0.2f * (float)(Math.Sin(Main.timeForVisualEffects / 72f + m.position.X / 13d + m.position.Y / 4d)), 0)
                        * (Main.windSpeedCurrent + 1f) * 2f
                        + new Vector2(0, gravity * m.mass);
                    m.Update(deltaTime);
                }
            }
        }
        public void Draw()
        {
            var gd = Main.instance.GraphicsDevice;
            var sb = Main.spriteBatch;
            List<Vertex2D> vertices = new List<Vertex2D>(100);
            List<int> indices = new List<int>(100);
            foreach (var rope in ropes)
            {
                List<Vector2> massPositionsSmooth = Commons.Function.Curves.CatmullRom.SmoothPath(rope.mass.Select(m => m.position));
                DrawRope(massPositionsSmooth, vertices, indices);
            }
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null,
                Matrix.CreateTranslation(-Main.screenPosition.X, -Main.screenPosition.Y, 0) * Main.GameViewMatrix.TransformationMatrix);
            sb.End();
            gd.Textures[0] = TextureAssets.MagicPixel.Value;
            gd.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices.ToArray(), 0, vertices.Count, indices.ToArray(), 0, indices.Count / 3);
        }

        private static void DrawRope(List<Vector2> path, List<Vertex2D> vertices, List<int> indices)
        {
            const float baseWidth = 4f;
            int count = path.Count;
            int baseIndex = vertices.Count;
            for (int i = 1; i < count; i++)
            {
                Vector2 normal = Vector2.Normalize(path[i] - path[i - 1]);
                (normal.X, normal.Y) = (-normal.Y, normal.X);
                float width = baseWidth * (1 - (float)i / (count - 1));
                vertices.Add(new Vertex2D(path[i] - normal * width, Color.White, Vector3.Zero));
                vertices.Add(new Vertex2D(path[i] + normal * width, Color.White, Vector3.Zero));
            }
            for (int i = 0; i < count * 2 - 5; i++)
            {
                indices.Add(baseIndex + i);
                indices.Add(baseIndex + i + 1);
                indices.Add(baseIndex + i + 2);
            }
        }

        private Color GetLuminace(Color color)
        {
            if (luminance != 1)
            {
                byte a = color.A;
                color *= luminance;
                color.A = a;
            }
            return color;
        }
    }
}
