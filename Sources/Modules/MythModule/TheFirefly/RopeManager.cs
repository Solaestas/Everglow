
using Everglow.Sources.Commons.Function.ImageReader;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Modules.MythModule.TheFirefly.Physics;

using SixLabors.ImageSharp.PixelFormats;

using Terraria.GameContent;

namespace Everglow.Sources.Modules.MythModule.TheFirefly
{
    internal class Rope
    {
        public Mass[] mass;
        public Spring[] spring;
        public Func<Vector2> GetOffset;
        private Rope() { }
        public Rope(Vector2 position, float scale, int count, Func<Vector2> offset)
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

            GetOffset = offset;
        }
        public Rope Clone(Vector2 deltaPosition)
        {
            Rope clone = new Rope
            {
                mass = new Mass[mass.Length],
                spring = new Spring[mass.Length - 1]
            };
            for (int i = 0; i < mass.Length; i++)
            {
                clone.mass[i] = new Mass(mass[i].mass, mass[i].position + deltaPosition, mass[i].isStatic);
            }
            for (int i = 0; i < spring.Length; i++)
            {
                clone.spring[i] = new Spring(0.3f, 20, 0.05f, clone.mass[i], clone.mass[i + 1]);
            }
            clone.GetOffset = GetOffset;
            return clone;
        }
    }
    internal class RopeManager
    {

        private float luminance = 1;
        private float gravity = 1;
        private List<Rope> ropes = new List<Rope>(100);
        /// <summary>
        /// 根据图片加载Rope，并返回由本次Load所增加的Rope组成List
        /// </summary>
        /// <param name="ropeImagePath"></param>
        /// <param name="rectangle"></param>
        /// <param name="basePosition"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public List<Rope> LoadRope(string ropeImagePath, Rectangle? rectangle, Vector2 basePosition, Func<Vector2> offset)
        {
            List<Rope> result = new List<Rope>();
            var image = ImageReader.Read<Rgb24>(ropeImagePath);
            Rectangle rect;
            rect = rectangle ?? new Rectangle(0, 0, image.Width, image.Height);
            image.ProcessPixelRows(accessor =>
            {
                for (int j = 0; j < rect.Height; j++)
                {
                    var span = accessor.GetRowSpan(j + rect.Y);
                    for (int i = 0; i < rect.Width; i++)
                    {
                        var pixel = span[i + rect.X];
                        if (pixel.R == 255)
                        {
                            var rope = new Rope(new Vector2(i * 5, j * 5) + basePosition, (pixel.B + 140) / 300f, Math.Max((int)pixel.G, 3), offset);
                            ropes.Add(rope);
                            result.Add(rope);
                        }
                    }
                }
            });
            return result;
        }
        public void LoadRope(IEnumerable<Rope> ropes)
        {
            this.ropes.AddRange(ropes);
        }
        /// <summary>
        /// 清除屏幕外的Rope
        /// </summary>
        /// <param name="outRange"></param>
        public void Clear(int outRange)
        {
            Rectangle validRange = new Rectangle((int)Main.screenPosition.X - outRange, (int)Main.screenPosition.Y - outRange,
                Main.screenWidth + outRange * 2, Main.screenHeight + outRange * 2);
            for (int i = 0; i < ropes.Count; i++)
            {
                if (!validRange.Contains((ropes[i].mass[0].position + ropes[i].GetOffset()).ToPoint()))
                {
                    ropes.RemoveAt(i--);
                }
            }
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
                    m.force += new Vector2(0.02f + 0.1f * (float)(Math.Sin(Main.timeForVisualEffects / 72f + m.position.X / 13d + m.position.Y / 4d)), 0)
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
            const int extraRange = 500;
            Rectangle drawRange = new Rectangle((int)Main.screenPosition.X - extraRange, (int)Main.screenPosition.Y - extraRange,
                Main.screenWidth + extraRange * 2, Main.screenHeight + extraRange * 2);
            foreach (var rope in ropes)
            {
                Vector2 offset = rope.GetOffset();
                if(!drawRange.Contains((offset + rope.mass[0].position).ToPoint()))
                {
                    continue;
                }
                List<Vector2> massPositionsSmooth = Commons.Function.Curves.CatmullRom.SmoothPath(rope.mass.Select(m => m.position + offset), 4);
                DrawRope(massPositionsSmooth, vertices, indices);
            }
            if(vertices.Count < 3)
            {
                return;
            }
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null,
                Matrix.CreateTranslation(-Main.screenPosition.X, -Main.screenPosition.Y, 0) * Main.GameViewMatrix.TransformationMatrix);
            gd.Textures[0] = MythContent.QuickTexture("TheFirefly/Tiles/Branch");
            //gd.Textures[0] = TextureAssets.MagicPixel.Value;
            gd.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices.ToArray(), 0, vertices.Count, indices.ToArray(), 0, indices.Count / 3);
            sb.End();
        }

        private void DrawRope(List<Vector2> path, List<Vertex2D> vertices, List<int> indices)
        {
            const float baseWidth = 4f;
            int count = path.Count;
            int baseIndex = vertices.Count;
            for (int i = 1; i < count; i++)
            {
                Vector2 normal = Vector2.Normalize(path[i] - path[i - 1]);
                (normal.X, normal.Y) = (-normal.Y, normal.X);
                float width = baseWidth * (1 - (float)i / (count - 1));
                float factor = (i - 1f) / (count - 2);
                vertices.Add(new Vertex2D(path[i] - normal * width, GetLuminace(Color.White), new Vector3(0, factor, 0)));
                vertices.Add(new Vertex2D(path[i] + normal * width, GetLuminace(Color.White), new Vector3(1, factor, 0)));
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
