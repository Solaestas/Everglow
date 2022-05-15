using Everglow.Sources.Commons.Network.PacketHandle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Commons.Core.Network.Packets
{
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
