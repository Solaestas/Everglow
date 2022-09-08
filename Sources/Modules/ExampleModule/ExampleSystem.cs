using Everglow.Sources.Commons.Core.Profiler.Fody;
using Everglow.Sources.Modules.MythModule.TheFirefly.Backgrounds;
using MonoMod.Cil;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.GameContent.Shaders;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;

using MathNet.Numerics.LinearAlgebra.Single;
using MathNet.Numerics.LinearAlgebra;
using Everglow.Sources.Commons.Function.Numerics;
using Everglow.Sources.Modules.MythModule.TheFirefly;
using Everglow.Sources.Modules.MythModule.TheFirefly.Physics;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MythModule.Common;
using Matrix = Microsoft.Xna.Framework.Matrix;

namespace Everglow.Sources.Modules.ExampleModule
{
    [ProfilerMeasure]
    internal class ExampleSystem : ModSystem
    {
        private RopeManager m_ropeManager;
        private Effect m_defualtDrawEffect;
        public override void OnModLoad()
        {
            m_defualtDrawEffect = MythContent.QuickEffect("Effects/DefaultDraw");
            base.OnModLoad();
        }
        public override void PreUpdatePlayers()
        {
            if (Main.mouseRight && Main.mouseRightRelease)
            {
                var tile = Main.tile[Player.tileTargetX, Player.tileTargetY];
                Main.NewText(tile.HasTile);
            }

            var sdf = SDFUtils.CalculateTileSDF(Main.LocalPlayer.position + Main.LocalPlayer.Size);

            // m_ropeManager = null;
            if (m_ropeManager == null)
            {
                m_ropeManager = new RopeManager();

                List<Vector2> posList = new List<Vector2>();
                Vector2 start = new Vector2(Main.spawnTileX * 16 - 200, Main.spawnTileY * 16 - 200);
                int slices = 100;
                float totalMass = 10f;
                for (int i = 0; i < slices; i++)
                {
                    posList.Add(start + new Vector2(400 / (float)slices, 0) * i);
                }
                Rope rope = new Rope(posList, 5, totalMass / slices, (x) => x, true);
                m_ropeManager.LoadRope(rope);
            }
            m_ropeManager.Ropes[0].GetMassList[^1].Position = Main.LocalPlayer.Center;
            m_ropeManager.Update(0.5f);

            var last = m_ropeManager.Ropes[0].GetMassList[^1].Position;
            var last2 = m_ropeManager.Ropes[0].GetMassList[^2].Position;
            var len = (last - last2).Length();
            var unit = Vector2.Normalize(last - last2);
            var force = -3 * (len - 400 / 60f) * unit;
            if (Main.LocalPlayer.velocity.Y == 0)
            {
                force.Y = 0;
            }
            //Main.LocalPlayer.velocity += force * 0.01f;
        }
        public override void PostUpdateEverything()
        {
            


            //if (Main.time % 600 < 1)
            //{
            //    Everglow.ProfilerManager.PrintSummary();
            //}
            //if (Main.mouseRight && Main.mouseRightRelease)
            //{
            //    entities.Add(new Entity()
            //    {
            //        Position = Main.LocalPlayer.Center + new Vector2(-300, 0),
            //        Velocity = Vector2.Zero,
            //        Mass = 1f
            //    });
            //    entities.Add(new Entity()
            //    {
            //        Position = Main.LocalPlayer.Center + new Vector2(300, 0),
            //        Velocity = Vector2.Zero,
            //        Mass = 1f
            //    });
            //}

            //float deltaTime = 1f;
            //float elasticity = 0.3f;

            //for (int i = 0; i < entities.Count; i++)
            //{
            //    var E = entities[i];
            //    E.X = (E.Position + E.Velocity * deltaTime).ToMathNetVector();
            //}

            //int iters = 0;
            //for (int j = 0; j < 100; j++)
            //{
            //    bool canEnd = true;
            //    for (int i = 0; i < entities.Count; i++)
            //    {
            //        entities[i].G = G_1(entities[i], deltaTime);
            //    }
            //    for (int i = 0; i < entities.Count; i += 2)
            //    {
            //        entities[i].G -= Force(deltaTime, entities[i], entities[i + 1], elasticity, 100f);
            //        entities[i + 1].G -= Force(deltaTime, entities[i + 1], entities[i], elasticity, 100f);
            //    }
            //    for (int i = 0; i < entities.Count; i++)
            //    {
            //        float alpha = 1f / (entities[i].Mass / (deltaTime * deltaTime) + 4 * elasticity);
            //        var dx = alpha * entities[i].G;
            //        entities[i].X -= dx;

            //        if (i % 2 == 0 && dx.L2Norm() > 1e-3)
            //        {
            //            canEnd = false;
            //        }
            //    }
            //    iters++;
            //    if (canEnd)
            //    {
            //        break;
            //    }
            //}

            //Main.NewText(iters);

            //for (int i = 0; i < entities.Count; i += 2)
            //{
            //    var E = entities[i];
            //    var oldPos = E.Position;
            //    E.Position = E.X.ToVector2();

            //    var offset = E.Position - oldPos;
            //    E.Velocity = offset / deltaTime;
            //}
        }

        private void DrawRope(List<Vector2> path, List<Vertex2D> vertices, List<int> indices)
        {
            const float baseWidth = 0.5f;
            int count = path.Count;
            int baseIndex = vertices.Count;
            for (int i = 0; i < count; i++)
            {
                Vector2 normal = Vector2.Zero;
                if (i == 0)
                {
                    normal = Vector2.Normalize(path[i + 1] - path[i]);
                }
                else
                {
                    normal = Vector2.Normalize(path[i] - path[i - 1]);
                }

                (normal.X, normal.Y) = (-normal.Y, normal.X);
                float width = baseWidth * (1 - (float)i / (count - 1));
                float factor = (i - 1f) / (count - 2);

                vertices.Add(new Vertex2D(path[i] - normal * baseWidth, Color.White, new Vector3(0, factor, 0)));
                vertices.Add(new Vertex2D(path[i] + normal * baseWidth, Color.White, new Vector3(1, factor, 0)));
            }
            for (int i = 0; i < count - 1; i++)
            {
                indices.Add(baseIndex + i * 2);
                indices.Add(baseIndex + i * 2 + 1);
                indices.Add(baseIndex + i * 2 + 2);

                indices.Add(baseIndex + i * 2 + 1);
                indices.Add(baseIndex + i * 2 + 3);
                indices.Add(baseIndex + i * 2 + 2);
            }
        }

        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            if (m_ropeManager == null)
                return;

            m_ropeManager.Ropes[0].GetMassList[^1].Position = Main.LocalPlayer.Center;
            List<Vertex2D> vertices = new List<Vertex2D>(100);
            List<int> indices = new List<int>(100);
            foreach (var rope in m_ropeManager.Ropes)
            {
                List<Vector2> massPositionsSmooth = Commons.Function.Curves.CatmullRom.SmoothPath(rope.GetMassList.Select(m => rope.RenderingTransform(m.Position)), null);
                DrawRope(massPositionsSmooth, vertices, indices);
            }

            var model = Matrix.CreateTranslation(-Main.screenPosition.X, -Main.screenPosition.Y, 0);
            m_defualtDrawEffect.Parameters["uTransform"].SetValue(model * Main.Transform * Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1));
            var graphicsDevice = Main.graphics.GraphicsDevice;

            m_defualtDrawEffect.CurrentTechnique.Passes["DefaultDraw"].Apply();
            graphicsDevice.RasterizerState.CullMode = CullMode.None;
            graphicsDevice.Textures[0] = TextureAssets.MagicPixel.Value;
            graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices.ToArray(),
                0, vertices.Count, indices.ToArray(), 0, indices.Count / 3);
            //foreach (var entity in entities)
            //{
            //    spriteBatch.Draw(TextureAssets.MagicPixel.Value, entity.Position - Main.screenPosition, new Rectangle(0, 0, 8, 8),
            //        Color.White, 0, Vector2.One * 0.5f, 1f, SpriteEffects.None, 0f);
            //}

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.Transform);
            foreach (var rope in m_ropeManager.Ropes)
            {
                int index = 0;
                List<Vector2> massPositionsSmooth = Commons.Function.Curves.CatmullRom.SmoothPath(rope.GetMassList.Select(m => rope.RenderingTransform(m.Position)), 4);
                foreach (var m in rope.GetMassList)
                {
                    spriteBatch.Draw(TextureAssets.MagicPixel.Value, m.Position - Main.screenPosition, new Rectangle(0, 0, 8, 8),
                        index == 2 ? Color.Red : Color.Green, 0, Vector2.One * 4f, 0.5f, SpriteEffects.None, 0f);
                    index++;
                }
                //foreach (var p in massPositionsSmooth)
                //{
                //    spriteBatch.Draw(TextureAssets.MagicPixel.Value, p - Main.screenPosition, new Rectangle(0, 0, 8, 8),
                //        Color.Red, 0, Vector2.One * 4f, 0.5f, SpriteEffects.None, 0f);
                //}
            }
            graphicsDevice.RasterizerState.FillMode = FillMode.Solid;
            base.PostDrawInterface(spriteBatch);
        }
    }
}
