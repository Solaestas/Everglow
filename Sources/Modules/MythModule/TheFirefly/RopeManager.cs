using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Everglow.Sources.Commons.Function.ImageReader;
using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Modules.MythModule.TheFirefly.Physics;

using Microsoft.Xna.Framework.Graphics.PackedVector;

using rail;

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
                spring = new Spring[count];

                mass[0] = new Mass(scale * Main.rand.NextFloat(0.25f, 0.37f), position, true);
                mass[^1] = new Mass(scale * Main.rand.NextFloat(0.25f, 0.37f) * 1.3f, position, false);
                for (int i = 1; i < count - 1; i++)
                {
                    mass[i] = new Mass(scale * Main.rand.NextFloat(0.25f, 0.37f), position, false);
                }

                for (int i = 1; i < count; i++)
                {
                    spring[i] = new Spring(0.3f, 20, 0.05f, mass[i - 1], mass[i]);
                }
            }
            public void Update(float deltaTime)
            {
                foreach (var s in spring)
                {
                    s.ApplyForce(deltaTime);
                }
                foreach (var m in mass)
                {
                    m.Update(deltaTime);
                }
            }
            public void Draw()
            {
                List<Vector2> smooth = new List<Vector2>(mass.Length);
            }

        }
        private string ropeImagePath;
        private float gravity;
        private List<Rope> ropes = new List<Rope>(100);
        private void Initialize()
        {
            var imageData = ImageReader.Read<SixLabors.ImageSharp.PixelFormats.Rgb24>("Everglow/Sources/Modules/MythModule/TheFirefly/" + ropeImagePath);
            imageData.ProcessPixelRows(accessor =>
            {
                for (int y = 0; y < accessor.Height; y++)
                {
                    var pixelRow = accessor.GetRowSpan(y);
                    for (int x = 0; x < pixelRow.Length; x++)
                    {
                        ref var pixel = ref pixelRow[x];
                        if (pixel.R == 255)
                        {
                            ropes.Add(new Rope(new Vector2(x * 5, y * 5), (pixel.B + 140) / 300f, pixel.G));
                        }
                    }
                }
            });
        }
        public void Update(float deltaTime)
        {
            for (int i = 0; i < ropes.Count; i++)
            {
                ropes[i].Update(deltaTime);
            }
        }
        public void Draw()
        {
            var gd = Main.instance.GraphicsDevice;
            gd.RasterizerState = RasterizerState.CullNone;

            

        }
    }
}
