using Everglow.Core.Network.PacketHandle;

namespace Everglow.Core.PlayerUtils;

internal class InputPacketToServer : IPacket
{
	public BitsByte bits;
	public Vector2 mouseWorld;
	public void Receive(BinaryReader reader, int whoAmI)
	{
		bits = reader.ReadByte();
		mouseWorld = reader.ReadVector2();
	}

	public void Send(BinaryWriter writer)
	{
		BitsByte bits = new(Main.mouseLeft, Main.mouseRight, Main.LocalPlayer.controlUseTile);
		writer.Write(bits);
		writer.WriteVector2(Main.MouseWorld);
	}
}
internal class InputPacketToClient : IPacket
{
	public int whoAmI;
	public BitsByte bits;
	public Vector2 mouseWorld;
	public void Receive(BinaryReader reader, int whoAmI)
	{
		this.whoAmI = reader.ReadInt32();
		bits = reader.ReadByte();
		mouseWorld = reader.ReadVector2();
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
		var p = packet as InputPacketToServer;
		var toClient = new InputPacketToClient()
		{
			whoAmI = whoAmI,
			bits = p.bits,
			mouseWorld = p.mouseWorld,
		};
		var player = Main.player[whoAmI].GetModPlayer<PlayerManager>();
		player.MouseLeft.NetUpdate(p.bits[0]);
		player.MouseRight.NetUpdate(p.bits[1]);
		player.ControlUseTile.NetUpdate(p.bits[2]);
		player.MouseWorld.NetUpdate(p.mouseWorld);
		Everglow.PacketResolver.Send(toClient, -1, whoAmI);
	}
}

[HandlePacket(typeof(InputPacketToClient))]
internal class MousePacketToClientHandler : IPacketHandler
{
	public void Handle(IPacket packet, int whoAmI)
	{
		var p = packet as InputPacketToClient;
		var player = Main.player[p.whoAmI].GetModPlayer<PlayerManager>();
		player.MouseLeft.NetUpdate(p.bits[0]);
		player.MouseRight.NetUpdate(p.bits[1]);
		player.ControlUseTile.NetUpdate(p.bits[2]);
		player.MouseWorld.NetUpdate(p.mouseWorld);
	}
}