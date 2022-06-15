using Everglow.Sources.Commons.Core.Network.PacketHandle;
using Everglow.Sources.Commons.Core.Profiler.Fody;

namespace Everglow.Sources.Modules.ExampleModule.Packets
{
    [ProfilerMeasure]
    public class ExamplePacket : IPacket
    {
        public int TestValue
        {
            get; set;
        }

        public ExamplePacket( )
        {
        }

        public ExamplePacket( int x )
        {
            TestValue=x;
        }


        public void Receive( BinaryReader reader,int whoAmI )
        {
            TestValue=reader.ReadInt32( );
        }

        public void Send( BinaryWriter writer )
        {
            writer.Write(TestValue);
        }
    }

    [ProfilerMeasure]
    [HandlePacket(typeof(ExamplePacket))]
    public class TestPacketHandler : IPacketHandler
    {

        public void Handle( IPacket packet,int whoAmI )
        {
            // 这里注意最好不要直接改 TestPacket 的内容，因为这样可能会影响后续的handler
            // 如果只有一个handler那没问题
            var testPacket = packet as ExamplePacket;
            if( Main.netMode==NetmodeID.Server )
            {
                Everglow.PacketResolver.Send(new ExamplePacket(114514*testPacket.TestValue));
            }
            else
            {
                Main.NewText($"From Server: {testPacket.TestValue}");
            }
        }
    }
}
