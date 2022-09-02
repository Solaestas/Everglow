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

namespace Everglow.Sources.Modules.ExampleModule
{
    [ProfilerMeasure]
    internal class ExampleSystem : ModSystem
    {
        class Entity
        {
            public Vector2 Position;
            public Vector2 Velocity;
            public float Mass;
        }
        private List<Entity> entities = new List<Entity>();

        private Vector<float> G_prime(Vector<float> x, float dt, Entity A, Entity B, float elasticity, float restLength)
        {
            var pos = B.Position.ToMathNetVector();
            var vel = B.Velocity.ToMathNetVector();

            Vector<float> fixedPos = Vector<float>.Build.DenseOfArray(new float[] { A.Position.X, A.Position.Y });
            var offset = (x - fixedPos);
            var length = (float)offset.L2Norm();

            var unit = offset / length;
            Vector<float> force = -elasticity * (length - restLength) * unit + Vector<float>.Build.DenseOfArray(new float[] {0, 9.8f});
            Vector<float> term2 = Matrix<float>.Build.DenseIdentity(2) * B.Mass / (dt * dt) * (x - pos - dt * vel);
            return term2 - force;
        }

        private Matrix<float> G_Hessian(Vector<float> x, float dt, Entity A, Entity B, float elasticity, float restLength)
        {
            Vector<float> fixedPos = A.Position.ToMathNetVector();
            var offset = (x - fixedPos);
            var length = (float)offset.L2Norm();
            var length2 = offset.DotProduct(offset);

            var span = offset.OuterProduct(offset);
            var term1 = span * elasticity / length2;
            var term2 = (Matrix<float>.Build.DenseIdentity(2) - span / length2) * elasticity * (1 - restLength / length);
            return Matrix<float>.Build.DenseIdentity(2) * B.Mass / (dt * dt) + term1 + term2;
        }

        private Vector<float> NewtonsMethod(float dt, Entity A, Entity B, float elasticity, float restLength)
        {
            Vector<float> x = B.Position.ToMathNetVector();
            for (int i = 0; i < 10; i++)
            {
                var gPrime = G_prime(x, dt, A, B, elasticity, restLength);
                var H = G_Hessian(x, dt, A, B, elasticity, restLength);
                var deltaX = H.Solve(-gPrime);
                x += deltaX;
                if (deltaX.DotProduct(deltaX) < 1e-3)
                {
                    break;
                }
            }
            return x;
        }

        public override void OnModLoad()
        {
            base.OnModLoad();
        }

        public override void PostUpdateEverything()
        {
            //if (Main.time % 600 < 1)
            //{
            //    Everglow.ProfilerManager.PrintSummary();
            //}
            if (Main.mouseRight && Main.mouseRightRelease)
            {
                entities.Add(new Entity()
                {
                    Position = Main.LocalPlayer.Center + new Vector2(-300, 0),
                    Velocity = Vector2.Zero,
                    Mass = 1f
                });
                entities.Add(new Entity()
                {
                    Position = Main.LocalPlayer.Center + new Vector2(300, 0),
                    Velocity = Vector2.Zero,
                    Mass = 1f
                });
            }

            float deltaTime = 1f;
            for (int i = 0; i < entities.Count; i += 2)
            {
                var pos = NewtonsMethod(deltaTime, entities[i], entities[i + 1], 0.3f, 100f).ToVector2();
                var offset = pos - entities[i + 1].Position;
                entities[i + 1].Position = pos;
                entities[i + 1].Velocity = offset / deltaTime;
            }
        }

        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            foreach (var entity in entities)
            {
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, entity.Position - Main.screenPosition, new Rectangle(0, 0, 8, 8),
                    Color.White, 0, Vector2.One * 0.5f, 1f, SpriteEffects.None, 0f);
            }
            base.PostDrawInterface(spriteBatch);
        }
    }
}
