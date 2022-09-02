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

        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            //foreach (var entity in entities)
            //{
            //    spriteBatch.Draw(TextureAssets.MagicPixel.Value, entity.Position - Main.screenPosition, new Rectangle(0, 0, 8, 8),
            //        Color.White, 0, Vector2.One * 0.5f, 1f, SpriteEffects.None, 0f);
            //}
            base.PostDrawInterface(spriteBatch);
        }
    }
}
