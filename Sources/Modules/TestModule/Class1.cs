using Everglow.Sources.Modules.TestModule.Packets;

namespace Everglow.Sources.Modules.TestModule
{
    internal class Class1 : ModSystem
    {
        public override void PostUpdateEverything()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                if (Main.time % 60 < 1)
                {
                    Everglow.PacketResolver.Send(new TestPacket(1));
                }
            }
        }
    }
}
