using Everglow.Sources.Commons.Core.Profiler.Fody;
using Everglow.Sources.Modules.ExampleModule.Packets;

namespace Everglow.Sources.Modules.ExampleModule
{
    [ProfilerMeasure]
    internal class ExampleSystem : ModSystem
    {
        public override void PostUpdateEverything()
        {
            //if (Main.time % 600 < 1)
            //{
            //    Everglow.ProfilerManager.PrintSummary();
            //}
        }
    }
}
