using Everglow.Commons.Mechanics.Mission.WorldMission.Base;
using Everglow.Commons.Netcode.Abstracts;

namespace Everglow.Commons.Mechanics.Mission.WorldMission.Packets;

public class MissionSyncPacket : IPacket
{
	private WorldMissionBase _mission;

	public MissionSyncPacket()
	{
	}

	public MissionSyncPacket(WorldMissionBase mission)
	{
		_mission = mission;
	}

	public void Receive(BinaryReader reader, int whoAmI)
	{
		var name = reader.ReadString();
		var mission = WorldMissionManager.Instance.GetMission(name);
		mission.NetReceive(reader);
	}

	public void Send(BinaryWriter writer)
	{
		writer.Write(_mission.Name);
		_mission.NetSend(writer);
	}

	[HandlePacket(typeof(MissionSyncPacket))]
	public class SyncMissionPacketHandler : IPacketHandler
	{
		public void Handle(IPacket packet, int whoAmI)
		{
			// All logic already executed in Receive()
			// Keep empty to satisfy the interface
		}
	}
}