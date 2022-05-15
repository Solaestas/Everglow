using Everglow.Sources.Commons.ModuleSystem;
using Everglow.Sources.Commons.Network.PacketHandle;

namespace Everglow.Sources.Commons.Core.Network.Packets
{
    //public class TestSystem : ModPlayer
    //{
    //    public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
    //    {
    //        if(newPlayer)
    //        {
    //            Everglow.PacketResolver.Send(new RequestWorldVersion());
    //        }
    //    }
    //}
    //public class RequestWorldVersion : IPacket
    //{
    //    public void Receive(BinaryReader reader, int whoAmI)
    //    {
    //    }

    //    public void Send(BinaryWriter writer)
    //    {
    //    }
    //}
    //public class SendWorldVersion : IPacket
    //{
    //    public ulong version;
    //    public void Receive(BinaryReader reader, int whoAmI)
    //    {
    //        version = reader.ReadUInt64();
    //    }
    //    public void Send(BinaryWriter writer)
    //    {
    //        writer.Write(Main.ActiveWorldFileData.WorldGeneratorVersion);
    //    }
    //}
    //[HandlePacket(typeof(RequestWorldVersion))]
    //public class RequestWorldVersionHandler : IPacketHandler
    //{
    //    public void Handle(IPacket packet, int whoAmI)
    //    {
    //        Everglow.PacketResolver.Send(new SendWorldVersion(), whoAmI);
    //    }
    //}
    //[HandlePacket(typeof(SendWorldVersion))]
    //public class SendWorldVersionHandler : IPacketHandler
    //{
    //    public void Handle(IPacket packet, int whoAmI)
    //    {
    //        Main.NewText((packet as SendWorldVersion).version);
    //        Main.NewText(1);
    //    }
    //}
    public class TestPacket : IPacket
    {
        public int TestValue { get; set; }

        public TestPacket() { }

        public TestPacket(int x) { TestValue = x; }

        public void Receive(BinaryReader reader, int whoAmI)
        {
            TestValue = reader.ReadInt32();
        }

        public void Send(BinaryWriter writer)
        {
            writer.Write(TestValue);
        }
    }

    [HandlePacket(typeof(TestPacket))]
    public class TestPacketHandler : IPacketHandler
    {
        public void Handle(IPacket packet, int whoAmI)
        {
            // 这里注意最好不要直接改 TestPacket 的内容，因为这样可能会影响后续的handler
            // 如果只有一个handler那没问题
            var testPacket = packet as TestPacket;
            if (Main.netMode == NetmodeID.Server)
            {
                Everglow.PacketResolver.Send(new TestPacket(114514 * testPacket.TestValue));
            }
            else
            {
                Main.NewText($"From Server: {testPacket.TestValue}");
            }
        }
    }
}
