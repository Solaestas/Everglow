using Everglow.Sources.Commons.Core.Profiler.Fody;
using Everglow.Sources.Modules.MythModule.TheFirefly.Backgrounds;
using MonoMod.Cil;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.GameContent.Shaders;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;

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
        }

        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            base.PostDrawInterface(spriteBatch);
        }
    }
}
