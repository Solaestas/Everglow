using Everglow.Commons.Mechanics.ElementalDebuff;
using Everglow.Commons.Netcode.Abstracts;
using Everglow.Commons.Netcode.PacketHandle;
using Everglow.Commons.Utilities;

namespace Everglow.Commons.Netcode.Packets;

public class ElementalBuildUpPacket : IPacket
{
	public ElementalBuildUpPacket()
	{
	}

	public ElementalBuildUpPacket(int target, ushort netID, int buildUp)
	{
		this.target = target;
		this.netID = netID;
		this.buildUp = buildUp;
	}

	private int target;
	private ushort netID;
	private int buildUp;

	public void Receive(BinaryReader reader, int whoAmI)
	{
		target = reader.ReadInt32();
		netID = reader.ReadUInt16();
		buildUp = reader.ReadInt32();
	}

	public void Send(BinaryWriter writer)
	{
		writer.Write(target);
		writer.Write(netID);
		writer.Write(buildUp);
	}

	[HandlePacket(typeof(ElementalBuildUpPacket))]
	public class ElementalBuildUpPacketHandler : IPacketHandler
	{
		public void Handle(IPacket packet, int whoAmI)
		{
			var elementalBuildUpPacket = (ElementalBuildUpPacket)packet;
			var player = Main.player[whoAmI];
			var type = ElementalDebuffRegistry.Registry[elementalBuildUpPacket.netID].ID;
			if (whoAmI == -1)
			{
				Main.npc[elementalBuildUpPacket.target].AddElementalDebuffBuildUp_World(type, elementalBuildUpPacket.buildUp);
			}
			else
			{
				Main.npc[elementalBuildUpPacket.target].AddElementalDebuffBuildUp(player, type, elementalBuildUpPacket.buildUp);
				if (NetUtils.IsServer)
				{
					ModIns.PacketResolver.Send(packet, true, player);
				}
			}
		}
	}
}