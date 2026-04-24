using Everglow.Commons.Mechanics.Mission.WorldMission.Base;
using Everglow.Commons.Netcode.Abstracts;

namespace Everglow.Commons.Mechanics.Mission.WorldMission.Packets;

public class ObjectiveDeltaSyncPacket_MainProgress : IPacket
{
	private int _missionWhoAmI;

	private IDeltaSyncObjective syncObjective;

	public ObjectiveDeltaSyncPacket_MainProgress()
	{
	}

	public ObjectiveDeltaSyncPacket_MainProgress(int missionWhoAmI, IDeltaSyncObjective objective)
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
			deltaSyncObjective.ReceiveMain(reader);
		}
		else
		{
			Ins.Logger.Error($"{missionId} {objectiveId} {objective.GetType().Name} is not {nameof(IDeltaSyncObjective)}.");
		}
	}

	public void Send(BinaryWriter writer)
	{
		writer.Write(_missionWhoAmI);
		writer.Write((syncObjective as WorldObjectiveBase).ObjectiveID);
		syncObjective.SendMain(writer);
	}

	[HandlePacket(typeof(ObjectiveDeltaSyncPacket_MainProgress))]
	public class MissionDeltaSyncPacketHandler : IPacketHandler
	{
		public void Handle(IPacket packet, int whoAmI)
		{
		}
	}
}