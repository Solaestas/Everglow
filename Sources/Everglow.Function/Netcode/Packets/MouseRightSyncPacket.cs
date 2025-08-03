using Everglow.Commons.Netcode.Abstracts;
using Everglow.Commons.Netcode.PacketHandle;
using Everglow.Commons.Utilities;

namespace Everglow.Commons.Netcode.Packets;

public class MouseRightSyncPacket : IPacket
{
	public MouseRightSyncPacket()
	{
	}

	public MouseRightSyncPacket(bool mouseRight)
	{
		this.mouseRight = mouseRight;
	}

	public bool mouseRight;

	public void Receive(BinaryReader reader, int whoAmI)
	{
		mouseRight = reader.ReadBoolean();
	}

	public void Send(BinaryWriter writer)
	{
		writer.Write(mouseRight);
	}

	[HandlePacket(typeof(MouseRightSyncPacket))]
	public class MouseRightSyncPacketHandler : IPacketHandler
	{
		public void Handle(IPacket packet, int whoAmI)
		{
			var mouseRight = (packet as MouseRightSyncPacket).mouseRight;
			var player = Main.player[whoAmI];
			var mp = player.GetModPlayer<EverglowPlayer>();
			mp.mouseRight = mouseRight;

			if (NetUtils.IsServer)
			{
				mp.SyncMouseRight(true);
			}

			// Console.WriteLine($"Player: {player.name} MouseRight: {mp.mouseRight}");
		}
	}
}