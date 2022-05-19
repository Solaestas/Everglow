using Everglow.Sources.Commons.Core.Profiler.Fody;
using Everglow.Sources.Modules.ExampleModule.Packets;

namespace Everglow.Sources.Modules.ExampleModule
{
    internal class ExampleSystem : ModSystem
    {
        [ProfilerMeasure]
        public override void PostUpdateEverything()
        {
        }
    }
}
