using Everglow.Sources.Commons.Core.Network.PacketHandle;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.WorldGeneration
{
    internal class MothPositionPacket : IPacket
    {
        public int fireflyCenterX;
        public int fireflyCenterY;

        public MothPositionPacket()
        {
        }

        public MothPositionPacket(int fireflyCenterX, int fireflyCenterY)
        {
            this.fireflyCenterX = fireflyCenterX;
            this.fireflyCenterY = fireflyCenterY;
        }

        public void Receive(BinaryReader reader, int whoAmI)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                //服务器收到请求后发送指定客户端坐标
            }
            else
            {
                //客户端收到后读取坐标
                fireflyCenterX = reader.ReadInt32();
                fireflyCenterY = reader.ReadInt32();
            }
        }

        public void Send(BinaryWriter writer)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                //客户端发送给服务器请求获取坐标
            }
            else
            {
                //服务器收到请求后写入坐标
                writer.Write(fireflyCenterX);
                writer.Write(fireflyCenterY);
            }
        }
    }
    [HandlePacket(typeof(MothPositionPacket))]
    internal class MothPositionPacketHandler : IPacketHandler
    {
        public void Handle(IPacket packet, int whoAmI)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                //服务器收到请求后发送指定客户端坐标
                var moth = ModContent.GetInstance<MothLand>();
                Everglow.PacketResolver.Send(new MothPositionPacket(moth.fireflyCenterX, moth.fireflyCenterY), whoAmI, -1);
            }
            else
            {
                //客户端收到后读取坐标
                var moth = ModContent.GetInstance<MothLand>();
                var p = packet as MothPositionPacket;
                moth.fireflyCenterX = p.fireflyCenterX;
                moth.fireflyCenterY = p.fireflyCenterY;
            }
        }
    }
}
