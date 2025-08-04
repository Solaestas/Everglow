using Everglow.Commons.Mechanics.Cooldown;
using Everglow.Commons.Netcode.Abstracts;
using Everglow.Commons.Netcode.PacketHandle;

namespace Everglow.Commons.Netcode.Packets;

public class CooldownRemovalPacket : IPacket
{
	public IList<string> cooldownIDs;

	public CooldownRemovalPacket()
	{
	}

	public CooldownRemovalPacket(IList<string> cooldownIDs)
	{
		this.cooldownIDs = cooldownIDs;
	}

	public void Receive(BinaryReader reader, int whoAmI)
	{
		cooldownIDs = [];
		int count = reader.ReadInt32();
		for (int i = 0; i < count; i++)
		{
			var netID = reader.ReadUInt16();
			cooldownIDs.Add(CooldownRegistry.registry[netID].ID);
		}
	}

	public void Send(BinaryWriter writer)
	{
		writer.Write(cooldownIDs.Count);
		foreach (var id in cooldownIDs)
		{
			writer.Write(CooldownRegistry.GetNet(id).NetID);
		}
	}

	[HandlePacket(typeof(CooldownRemovalPacket))]
	public class CooldownRemovalPacketHandler : IPacketHandler
	{
		public void Handle(IPacket packet, int whoAmI)
		{
			foreach(var id in ((CooldownRemovalPacket)packet).cooldownIDs)
			{
				Main.player[whoAmI].GetModPlayer<EverglowPlayer>().cooldowns.Remove(id);

				// Console.WriteLine(id);
			}
		}
	}
}