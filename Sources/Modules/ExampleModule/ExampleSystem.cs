using Everglow.Sources.Commons.Core.Profiler.Fody;
using Everglow.Sources.Modules.ExampleModule.Packets;

namespace Everglow.Sources.Modules.ExampleModule
{
    internal class ExampleSystem : ModSystem
    {
        [ProfilerMeasure]
        public override void PostUpdateEverything()
        {
            if (Main.netMode != 3)
            {
                if (Main.time % 60 < 1)
                {
                    Everglow.PacketResolver.Send(new ExamplePacket(1));
                }
                int x = 0;
                for (int i = 0; i < 1090000; i++)
                {
                    x++;
                }
            }

            Everglow.ProfilerManager.PrintSummary();
        }
    }
}
