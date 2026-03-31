using Everglow.Commons.Mechanics.Mission.WorldMission;
using Everglow.Commons.Mechanics.Mission.WorldMission.Base;
using Everglow.Commons.Mechanics.Mission.WorldMission.Objectives;
using Everglow.Commons.Netcode.Abstracts;

namespace Everglow.Commons.Netcode.Packets.Mission;

public class ObjectiveDeltaSyncPacket_SubProgress : IPacket
{
	private int _missionWhoAmI;

	private IDeltaSyncObjective syncObjective;

	public ObjectiveDeltaSyncPacket_SubProgress()
	{
	}

	public ObjectiveDeltaSyncPacket_SubProgress(int missionWhoAmI, IDeltaSyncObjective objective)
	{
		_missionWhoAmI = missionWhoAmI;
		syncObjective = objective;
	}

	public void Receive(BinaryReader reader, int whoAmI)
	{
		var missionId = reader.ReadInt32();
		var objectiveId = reader.ReadInt32();
		var mission = WorldMissionManager.Instance.GetMission(missionId);
		var objective = mission.Objectives[objectiveId];
		if (objective is IDeltaSyncObjective deltaSyncObjective)
		{
			deltaSyncObjective.ReceiveDelta(reader);
			ModIns.PacketResolver.Send(new ObjectiveDeltaSyncPacket_MainProgress(missionId, deltaSyncObjective), -1, -1);
		}
		else
		{
			Ins.Logger.Error($"{missionId} {objectiveId} {objective.GetType().Name} is not {nameof(IDeltaSyncObjective)}.");
		}
	}

	public void Send(BinaryWriter writer)
	{
		writer.Write(_missionWhoAmI); // Mission id
		writer.Write((syncObjective as WorldObjectiveBase).ObjectiveID); // Objective id
		syncObjective.SendDelta(writer);
	}

	[HandlePacket(typeof(ObjectiveDeltaSyncPacket_SubProgress))]
	public class MissionDeltaSyncPacketHandler : IPacketHandler
	{
		public void Handle(IPacket packet, int whoAmI)
		{
			// All logic already executed in Receive()
			// Keep empty to satisfy the interface
		}
	}
}