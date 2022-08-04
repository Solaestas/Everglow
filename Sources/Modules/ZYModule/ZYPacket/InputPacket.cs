using Everglow.Sources.Commons.Core.Network.PacketHandle;
using Everglow.Sources.Modules.ZYModule.Commons.Function;

namespace Everglow.Sources.Modules.ZYModule.ZYPacket;

internal class InputPacketToServer : IZYPacket
{
    public void Receive(BinaryReader reader, int whoAmI)
    {
        BitsByte bits = reader.ReadByte();
        var packet = new InputPacketToClient()
        {
            whoAmI = whoAmI,
            bits = bits,
            mouseWorld = reader.ReadVector2(),
        };
        var player = Main.player[whoAmI].GetModPlayer<PlayerManager>();
        player.MouseLeft.Update(bits[0]);
        player.MouseRight.Update(bits[1]);
        player.ControlUseTile.Update(bits[2]);
        player.MouseWorld = packet.mouseWorld;
        Everglow.PacketResolver.Send(packet, -1, whoAmI);
    }

    public void Send(BinaryWriter writer)
    {
        BitsByte bits = new BitsByte(Main.mouseLeft, Main.mouseRight, Main.LocalPlayer.controlUseTile);
        writer.Write(bits);
        writer.WriteVector2(Main.MouseWorld);
    }
}
internal class InputPacketToClient : IZYPacket
{
    public int whoAmI;
    public BitsByte bits;
    public Vector2 mouseWorld;
    public void Receive(BinaryReader reader, int whoAmI)
    {
        var player = Main.player[reader.ReadInt32()].GetModPlayer<PlayerManager>();
        BitsByte bits = reader.ReadByte();
        player.MouseLeft.Update(bits[0]);
        player.MouseRight.Update(bits[1]);
        player.ControlUseTile.Update(bits[2]);
        player.MouseWorld = reader.ReadVector2();
    }

    public void Send(BinaryWriter writer)
    {
        writer.Write(whoAmI);
        writer.Write(bits);
        writer.WriteVector2(mouseWorld);
    }
}

[HandlePacket(typeof(InputPacketToServer))]
internal class MousePacketToServerHandler : IPacketHandler
{
    public void Handle(IPacket packet, int whoAmI)
    {
        
    }
}

[HandlePacket(typeof(InputPacketToClient))]
internal class MousePacketToClientHandler : IPacketHandler
{
    public void Handle(IPacket packet, int whoAmI)
    {

    }
}