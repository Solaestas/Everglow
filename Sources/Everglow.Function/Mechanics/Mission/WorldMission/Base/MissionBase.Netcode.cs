using Everglow.Commons.Mechanics.Mission.WorldMission.Packets;

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
		Console.WriteLine("Full sync msg sent!");
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

		if (oldState != State)
		{
			if (State == WorldMissionState.Active)
			{
				if (oldState == WorldMissionState.Locked)
				{
					var unlockText = $"[{DisplayName}]任务已解锁";
					var unlockTextColor = new Color(150, 150, 250);
					Main.NewText(unlockText, unlockTextColor);
				}
				else
				{
					var unlockText = $"[{DisplayName}]任务已恢复";
					var unlockTextColor = new Color(150, 150, 250);
					Main.NewText(unlockText, unlockTextColor);
				}
			}
			else if (State == WorldMissionState.Completed)
			{
				var completeText = $"[{DisplayName}]任务已完成";
				var completeTextColor = new Color(150, 250, 150);
				Main.NewText(completeText, completeTextColor);
			}
			else if (State == WorldMissionState.Failed)
			{
				var failText = $"[{DisplayName}]任务已失败";
				var failTextColor = new Color(250, 150, 150);
				Main.NewText(failText, failTextColor);
			}
		}
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