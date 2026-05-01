using Everglow.Commons.Mechanics.Mission.WorldMission.Base;
using Everglow.Commons.Netcode.Abstracts;
using Everglow.Commons.Utilities;

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
		_mission = WorldMissionManager.Instance.GetMission(name);
		_mission.NetReceive(reader);
	}

	public void Send(BinaryWriter writer)
	{
		writer.Write(_mission.Name);
		_mission.NetSend(writer);
	}

	[HandlePacket(typeof(MissionSyncPacket))]
	public class MissionSyncPacketHandler : IPacketHandler
	{
		public void Handle(IPacket packet, int whoAmI)
		{
			// Forward sync packet to all subworld client.
			if (NetUtils.IsSubServer)
			{
				ModIns.PacketResolver.Send(packet);
			}
		}
	}
}