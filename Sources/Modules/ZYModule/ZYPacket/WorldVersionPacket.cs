using Everglow.Sources.Commons.Core.Network.PacketHandle;
using Everglow.Sources.Modules.ZYModule.WorldModule;

namespace Everglow.Sources.Modules.ZYModule.ZYPacket;

internal class WorldVersionPacket : IZYPacket
{
    public ulong version;
    public void Receive(BinaryReader reader, int whoAmI)
    {
        if (Main.netMode == NetmodeID.MultiplayerClient)
        {
            version = reader.ReadUInt64();
        }
    }

    public void Send(BinaryWriter writer)
    {
        //如果是服务器，就发送version
        //如果是客户端，只利用whoAmI
        if (Main.netMode == NetmodeID.Server)
        {
            writer.Write(Main.ActiveWorldFileData.WorldGeneratorVersion);
        }
    }
}

[HandlePacket(typeof(WorldVersionPacket))]
internal class WorldVersionPacketHandler : IPacketHandler
{
    public void Handle(IPacket packet, int whoAmI)
    {
        //客户端收到Packet后根据version生成World
        if (Main.netMode == NetmodeID.MultiplayerClient)
        {
            WorldVersionPacket pack = packet as WorldVersionPacket;
            WorldSystem.CurrentWorld = World.CreateInstance(World.GetWorldName(pack.version));
        }
        //服务端收到Packet后会返回一个同样的Packet
        else if (Main.netMode == NetmodeID.Server)
        {
            //直接复用当前的Packet
            Everglow.PacketResolver.Send<WorldVersionPacket>(whoAmI);
        }
    }
}
