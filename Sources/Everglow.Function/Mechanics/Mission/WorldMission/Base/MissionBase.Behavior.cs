namespace Everglow.Commons.Mechanics.Mission.WorldMission.Base;

public abstract partial class MissionBase_New : IMissionBehavior
{
	public const string RewardItemsSourceContext = "Everglow.MissionSystem";

	public int WhoAmI { get; internal set; }

	public WorldMissionState MissionState { get; protected set; }

	public float Progress => throw new NotImplementedException();

	public MissionObjectiveContainer_New Objectives { get; } = new();

	public ObjectiveBase CurrentObjective { get; protected set; }

	public int Time { get; protected set; }

	public bool Resettable => MissionState == WorldMissionState.Failed;

	public bool RewardClaimed { get; protected set; }

	public HashSet<string> RewardClaimedPlayers { get; } = [];

	public void Activate()
	{
		CurrentObjective = Objectives.First;

		CurrentObjective?.Activate(this);
	}

	public void Deactivate()
	{
		CurrentObjective?.Deactivate();
	}

	public bool CheckComplete() => throw new NotImplementedException();

	public void Update()
	{
		// Handle time limit
		int timeLimit = (this as IMissionMetadata).TimeLimit;
		if (timeLimit > 0)
		{
			Time++;
			if (Time >= TimeLimit)
			{
				OnExpire();
				Deactivate();
				MissionState = WorldMissionState.Failed;
				return;
			}
		}

		// Handle objectives. If the mission is finished but not completed, skip this step.
		if (CurrentObjective is not null)
		{
			CurrentObjective.Update();

			if (!CurrentObjective.Completed
			&& CurrentObjective.CheckCompletion())
			{
				CurrentObjective.Complete();
				CurrentObjective.Deactivate();

				CurrentObjective = CurrentObjective.Next;
				CurrentObjective?.Activate(this);

				Main.NewText($"[{Name}]任务当前目标已完成", 250, 250, 150);
			}
		}
		else
		{
			// Run mission completion
			OnComplete();
			Deactivate(); // TODO: Sync to clients
		}
	}

	public void OnComplete()
	{
		GiveRewards();
		Main.NewText($"[{Name}]任务已完成", 150, 250, 150);
	}

	public virtual void OnExpire()
	{
	}

	public void Reset()
	{
		// TODO: reset all states, including progress, time, objectives, etc.
		MissionState = WorldMissionState.Active;
		Time = 0;
		RewardClaimed = false;
		ResetProgress();
	}

	public void ResetProgress()
	{
		foreach (var objective in Objectives.AllObjectives)
		{
			objective.ResetProgress();
		}
	}

	public void GiveRewards()
	{
		RewardClaimed = true;

		if (!Main.dedServ)
		{
			// TODO: Sync items to all sides.
			var rewardPlayer = Main.LocalPlayer;
			foreach (var item in RewardItems)
			{
				rewardPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_Misc(RewardItemsSourceContext), item, item.stack);
			}
		}
	}
}