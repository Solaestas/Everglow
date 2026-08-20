using Everglow.Commons.Mechanics.Mission.Core;

namespace Everglow.Commons.Mechanics.Mission.WorldSide.Abstractions;

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
		Objectives.NetSend(writer);
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
		Objectives.NetReceive(reader);
		if (!RecoverInvalidObjectiveState())
		{
			ApplyObjectiveSnapshot(oldState, State);
		}

		if (oldState != State)
		{
			if (State == WorldMissionState.Active)
			{
				if (oldState == WorldMissionState.Locked)
				{
					WorldMissionManager.Notify(this, MissionNotificationType.Unlocked);
				}
				else
				{
					WorldMissionManager.Notify(this, MissionNotificationType.Restored);
				}
			}
			else if (State == WorldMissionState.Completed)
			{
				WorldMissionManager.Notify(this, MissionNotificationType.Completed);
			}
			else if (State == WorldMissionState.Failed)
			{
				WorldMissionManager.Notify(this, MissionNotificationType.Failed);
			}
		}
	}

	public void OnMPSync()
	{
		Objectives.OnMPSync();
	}
}
