namespace Everglow.Commons.Mechanics.Mission.WorldMission.Base;

public abstract partial class MissionBase_New : IMissionBehavior
{
	public const string RewardItemsSourceContext = "Everglow.MissionSystem";

	public int WhoAmI { get; internal set; }

	public WorldMissionState State { get; protected set; } = WorldMissionState.Locked;

	public virtual float Progress => 1;

	public MissionObjectiveContainer_New Objectives { get; } = new();

	public ObjectiveBase CurrentObjective { get; protected set; }

	public int Time { get; protected set; }

	public bool Retriable => true;

	public bool RewardClaimed { get; protected set; }

	public HashSet<string> RewardClaimedPlayers { get; protected set; } = [];

	public void Unlock()
	{
		// Unlcok mission == net needed
		State = WorldMissionState.Active;

		Activate();
		OnUnlock();

		Main.NewText($"[{DisplayName}]任务已解锁", 150, 150, 250);
	}

	public void Activate()
	{
		CurrentObjective = Objectives.First;

		CurrentObjective?.Activate(this);
	}

	public void Deactivate()
	{
		CurrentObjective?.Deactivate();
	}

	public virtual bool CheckComplete() => true;

	public void Update()
	{
		// Handle time limit == net needed
		int timeLimit = (this as IMissionMetadata).TimeLimit;
		if (timeLimit > 0)
		{
			Time += MissionManager_New.UpdateInterval;
			if (Time >= TimeLimit)
			{
				Time = TimeLimit;
				OnExpire();
				Deactivate();
				State = WorldMissionState.Failed;

				Main.NewText($"[{DisplayName}]任务已过期", 250, 150, 150);

				return;
			}
		}

		// Handle objectives. If the mission is finished but not completed, skip this step.
		if (CurrentObjective is not null)
		{
			CurrentObjective.Update();

			// Switch to next objective == net needed
			// This operation should be stopped on local client in multiplayer here.
			if (!CurrentObjective.Completed
				&& CurrentObjective.CheckCompletion())
			{
				CurrentObjective.Complete();
				CurrentObjective.Deactivate();

				CurrentObjective = CurrentObjective.Next;
				CurrentObjective?.Activate(this);

				Main.NewText($"[{DisplayName}]任务当前目标已完成", 250, 250, 150);
			}
		}
		else
		{
			// Run mission completion == net needed
			State = WorldMissionState.Completed;
			OnComplete();
			Deactivate(); // Maybe not necessary, because the current objective is null.
			Main.NewText($"[{DisplayName}]任务已完成", 150, 250, 150);
		}
	}

	/// <summary>
	/// Requests the mission retry. Called by clicking the 'retry' button in the mission panel.
	/// </summary>
	public void Retry()
	{
		if (!Retriable || State != WorldMissionState.Failed)
		{
			return;
		}

		// Retry the mission == net needed
		State = WorldMissionState.Active;
		Time = 0;
		ResetProgress();
		Activate();

		Main.NewText($"[{DisplayName}]任务已重启", 150, 250, 150);
	}

	/// <summary>
	/// Requests the mission reward. Called by clicking the 'reward' button in the mission panel.
	/// </summary>
	public void GiveRewards()
	{
		if (RewardClaimed || State != WorldMissionState.Completed)
		{
			return;
		}

		RewardClaimed = true; // Sync this state to all sides == net needed

		// TODO: Sync items to all sides.
		var rewardPlayer = Main.LocalPlayer;
		foreach (var item in RewardItems)
		{
			rewardPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_Misc(RewardItemsSourceContext), item, item.stack);
		}
	}

	/// <summary>
	/// Resets the mission to its initial state, clearing progress, time, and objectives.
	/// <br/> This is called when loading a world.
	/// <para/> Override <see cref="OnReset"/> to add custom reset behavior.
	/// </summary>
	public void Reset()
	{
		State = WorldMissionState.Locked;
		Time = 0;
		RewardClaimed = false;
		RewardClaimedPlayers = [];
		ResetProgress();
		OnReset();
	}

	public void ResetProgress()
	{
		foreach (var objective in Objectives.AllObjectives)
		{
			objective.ResetProgress();
		}
	}

	public virtual void OnUnlock()
	{
	}

	public virtual void OnComplete()
	{
	}

	public virtual void OnExpire()
	{
	}

	public virtual void OnReset()
	{
	}
}