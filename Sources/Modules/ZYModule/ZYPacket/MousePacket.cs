using Everglow.Sources.Commons.Core.Network.PacketHandle;
using Everglow.Sources.Modules.ZYModule.Commons.Function;

namespace Everglow.Sources.Modules.ZYModule.ZYPacket;

internal class MousePacketToServer : IZYPacket
{
    public void Receive(BinaryReader reader, int whoAmI)
    {
        var packet = new MousePacketToClient()
        {
            mouseLeft = reader.ReadBoolean(),
            mouseRight = reader.ReadBoolean(),
            mouseWorld = reader.ReadVector2(),
            whoAmI = whoAmI
        };
        var player = Main.player[whoAmI].GetModPlayer<PlayerManager>();
        player.MouseLeft.Update(packet.mouseLeft);
        player.MouseRight.Update(packet.mouseRight);
        player.MouseWorld = packet.mouseWorld;
        Everglow.PacketResolver.Send(packet, -1, whoAmI);
    }

    public void Send(BinaryWriter writer)
    {
        writer.Write(Main.mouseLeft);
        writer.Write(Main.mouseRight);
        writer.WriteVector2(Main.MouseWorld);
    }
}
internal class MousePacketToClient : IZYPacket
{
    public int whoAmI;
    public bool mouseLeft;
    public bool mouseRight;
    public Vector2 mouseWorld;
    public void Receive(BinaryReader reader, int whoAmI)
    {
        var player = Main.player[reader.ReadInt32()].GetModPlayer<PlayerManager>();
        player.MouseLeft.Update(reader.ReadBoolean());
        player.MouseRight.Update(reader.ReadBoolean());
        player.MouseWorld = reader.ReadVector2();
    }

    public void Send(BinaryWriter writer)
    {
        writer.Write(whoAmI);
        writer.Write(mouseLeft);
        writer.Write(mouseRight);
        writer.WriteVector2(mouseWorld);
    }
}

[HandlePacket(typeof(MousePacketToServer))]
internal class MousePacketToServerHandler : IPacketHandler
{
    public void Handle(IPacket packet, int whoAmI)
    {
        
    }
}

[HandlePacket(typeof(MousePacketToClient))]
internal class MousePacketToClientHandler : IPacketHandler
{
    public void Handle(IPacket packet, int whoAmI)
    {

    }
}