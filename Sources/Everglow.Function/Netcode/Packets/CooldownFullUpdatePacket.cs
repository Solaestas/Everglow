using Everglow.Commons.Mechanics.Cooldown;
using Everglow.Commons.Netcode.Abstracts;
using Everglow.Commons.Netcode.PacketHandle;

namespace Everglow.Commons.Netcode.Packets;

public class CooldownFullUpdatePacket : IPacket
{
	public Dictionary<string, CooldownInstance> cooldowns;

	public Dictionary<ushort, CooldownInstance> syncedCooldowns;

	public CooldownFullUpdatePacket()
	{
	}

	public CooldownFullUpdatePacket(Dictionary<string, CooldownInstance> cooldowns)
	{
		this.cooldowns = cooldowns;
	}

	public void Receive(BinaryReader reader, int whoAmI)
	{
		var count = reader.ReadInt32();
		if (count <= 0)
		{
			syncedCooldowns = [];
			return;
		}

		syncedCooldowns = new Dictionary<ushort, CooldownInstance>(count);
		for (int i = 0; i < count; i++)
		{
			CooldownInstance instance = new CooldownInstance(reader);
			syncedCooldowns[instance.netID] = instance;
		}
	}

	public void Send(BinaryWriter writer)
	{
		writer.Write(cooldowns.Count);
		foreach (var cd in cooldowns.Values)
		{
			cd.Write(writer);
		}
	}

	[HandlePacket(typeof(CooldownFullUpdatePacket))]
	public class CooldownFullUpdatePacketHandler : IPacketHandler
	{
		public void Handle(IPacket packet, int whoAmI)
		{
			var mp = Main.player[whoAmI].GetModPlayer<EverglowPlayer>();
			var syncedCooldowns = (packet as CooldownFullUpdatePacket).syncedCooldowns;

			HashSet<ushort> localIDs = [];
			foreach (var cd in mp.cooldowns.Values)
			{
				localIDs.Add(cd.netID);
			}

			HashSet<ushort> syncedIDs = [];
			foreach (var cd in syncedCooldowns.Values)
			{
				syncedIDs.Add(cd.netID);
			}

			HashSet<ushort> combinedIDSet = [];
			combinedIDSet.UnionWith(localIDs);
			combinedIDSet.UnionWith(syncedIDs);

			foreach (var netID in combinedIDSet)
			{
				bool existsInLocal = localIDs.Contains(netID);
				bool existsInSynced = syncedIDs.Contains(netID);
				string id = CooldownRegistry.Registry[netID].ID;

				if (existsInLocal && !existsInSynced)
				{
					syncedCooldowns.Remove(netID);
				}
				else if (!existsInLocal && existsInSynced)
				{
					mp.cooldowns[id] = syncedCooldowns[netID];
				}
				else if (existsInLocal && existsInSynced)
				{
					CooldownInstance localInstance = mp.cooldowns[id];
					localInstance.timeMax = syncedCooldowns[netID].timeMax;
					localInstance.timeLeft = syncedCooldowns[netID].timeLeft;
				}
			}
		}
	}
}