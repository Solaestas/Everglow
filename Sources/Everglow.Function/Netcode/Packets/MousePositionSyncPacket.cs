using Everglow.Commons.Netcode.Abstracts;
using Everglow.Commons.Netcode.PacketHandle;
using Everglow.Commons.Utilities;

namespace Everglow.Commons.Netcode.Packets;

public class MousePositionSyncPacket : IPacket
{
	public MousePositionSyncPacket()
	{
	}

	public MousePositionSyncPacket(Vector2 mouseWorld)
	{
		this.mouseWorld = mouseWorld;
	}

	public Vector2 mouseWorld;

	public void Receive(BinaryReader reader, int whoAmI)
	{
		mouseWorld = reader.ReadVector2();
	}

	public void Send(BinaryWriter writer)
	{
		writer.WriteVector2(mouseWorld);
	}

	[HandlePacket(typeof(MousePositionSyncPacket))]
	public class MousePositionSyncPacketHandler : IPacketHandler
	{
		public void Handle(IPacket packet, int whoAmI)
		{
			var player = Main.player[whoAmI];
			var mp = player.GetModPlayer<EverglowPlayer>();
			mp.mouseWorld = ((MousePositionSyncPacket)packet).mouseWorld;

			if (NetUtils.IsServer)
			{
				mp.SyncMousePosition(true);
			}

			// Console.WriteLine($"Player: {player.name} MousePos: {mp.mouseWorld}");
		}
	}
}