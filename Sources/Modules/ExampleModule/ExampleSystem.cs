namespace Everglow.Sources.Modules.ExampleModule
{
    internal class ExampleSystem : ModSystem
    {
        public override void PostUpdateEverything()
        {
            //if (Main.netMode == NetmodeID.MultiplayerClient)
            //{
            //    if (Main.time % 60 < 1)
            //    {
            //        Everglow.PacketResolver.Send(new ExamplePacket(1));
            //    }
            //}
        }
    }
}
