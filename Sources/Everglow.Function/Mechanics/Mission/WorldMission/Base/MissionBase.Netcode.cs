using Everglow.Commons.Mechanics.Mission.WorldMission.Objectives;
using Everglow.Commons.Netcode.Packets.Mission;

namespace Everglow.Commons.Mechanics.Mission.WorldMission.Base;

public abstract partial class WorldMissionBase : IMissionNetcode
{
	public virtual void NetSend(BinaryWriter writer)
	{
		writer.Write((int)State);
		writer.Write(Time);
		writer.Write(RewardClaimed);
		writer.Write(RewardClaimedPlayers.Count);
		foreach (var player in RewardClaimedPlayers)
		{
			writer.Write(player);
		}
		foreach (var objective in Objectives.AllObjectives)
		{
			objective.NetSend(writer);
		}
	}

	public virtual void NetReceive(BinaryReader reader)
	{
		var oldState = State;
		State = (WorldMissionState)reader.ReadInt32();
		Time = reader.ReadInt32();
		RewardClaimed = reader.ReadBoolean();
		int rewardPlayerCount = reader.ReadInt32();
		for (int i = 0; i < rewardPlayerCount; i++)
		{
			RewardClaimedPlayers.Add(reader.ReadString());
		}
		foreach (var objective in Objectives.AllObjectives)
		{
			objective.NetReceive(reader);
		}

		ApplySnapshot(State, oldState);
	}

	public void OnMPSync()
	{
		if (CurrentObjective is not null
			and IDeltaSyncObjective deltaSync)
		{
			if (deltaSync.NeedDeltaSync)
			{
				ModIns.PacketResolver.Send(new ObjectiveDeltaSyncPacket_SubProgress(WhoAmI, deltaSync));
			}
		}
	}

	public void SyncRetry()
	{
		RetryCore();
	}

	public void SyncObjectiveCompletion()
	{
		CompleteObjectiveCore();
	}
}