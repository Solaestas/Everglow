using Everglow.Commons.Utilities;
using Terraria.Chat;

namespace Everglow.Commons.Mechanics.Mission.WorldMission.Base;

public abstract partial class WorldMissionBase : IMissionBehavior
{
	public const string RewardItemsSourceContext = "Everglow.MissionSystem";

	public int WhoAmI { get; internal set; }

	public WorldMissionState State { get; protected set; } = WorldMissionState.Locked;

	public virtual float Progress => 1;

	public WorldMissionObjectiveContainer Objectives { get; } = new();

	public WorldObjectiveBase CurrentObjective { get; protected set; }

	public int Time { get; protected set; }

	public bool Retriable => true;

	public bool RewardClaimed { get; protected set; }

	public HashSet<string> RewardClaimedPlayers { get; protected set; } = [];

	public void Unlock()
	{
		if (NetUtils.IsClient)
		{
			Console.WriteLine("Waiting for unlock packet.");
			return;
		}

		var unlockText = $"[{DisplayName}]任务已解锁";
		var unlockTextColor = new Color(150, 150, 250);

		if (NetUtils.IsSingle)
		{
			State = WorldMissionState.Active;

			Activate();
			OnUnlock();

			Main.NewText(unlockText, unlockTextColor);
		}
		else if (NetUtils.IsServer)
		{
			State = WorldMissionState.Active;
			Activate();
			OnUnlock();
			ChatHelper.BroadcastChatMessage(new Terraria.Localization.NetworkText(unlockText, Terraria.Localization.NetworkText.Mode.Literal), unlockTextColor);
			Console.WriteLine(unlockText);
			// TODO: Sync unlock state to all clients
		}
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

	public void Update()
	{
		if (TimeLimit > 0)
		{
			Time += WorldMissionManager.UpdateInterval;
			if (Time >= TimeLimit)
			{
				Time = TimeLimit;

				if (NetUtils.IsClient)
				{
					Console.WriteLine("Waiting for fail packet.");
					return;
				}

				var failText = $"[{DisplayName}]任务已失败";
				var failTextColor = new Color(250, 150, 150);

				if (NetUtils.IsSingle)
				{
					State = WorldMissionState.Failed;
					OnExpire();
					Deactivate();
					Main.NewText(failText, failTextColor);
				}
				else if (NetUtils.IsServer)
				{
					State = WorldMissionState.Failed;
					OnExpire();
					Deactivate();
					ChatHelper.BroadcastChatMessage(new Terraria.Localization.NetworkText(failText, Terraria.Localization.NetworkText.Mode.Literal), failTextColor);
					Console.WriteLine(failText);
					// TODO: Sync failure state to all clients
				}

				return;
			}
		}

		if (CurrentObjective is null)
		{
			Complete();
			return;
		}

		CurrentObjective.Update();

		// Check objective completion
		if (!CurrentObjective.Completed
			&& CurrentObjective.CheckCompletion())
		{
			if (NetUtils.IsClient)
			{
				Console.WriteLine("Waiting for objective complete packet.");
				return;
			}

			var objectiveCompleteText = $"[{DisplayName}]任务当前目标已完成";
			var objectiveCompleteTextColor = new Color(250, 250, 150);

			if (NetUtils.IsSingle)
			{
				CurrentObjective.Complete();
				CurrentObjective.Deactivate();

				CurrentObjective = CurrentObjective.Next;
				CurrentObjective?.Activate(this);

				Main.NewText(objectiveCompleteText, objectiveCompleteTextColor);
			}
			else if (NetUtils.IsServer)
			{
				CurrentObjective.Complete();
				CurrentObjective.Deactivate();
				CurrentObjective = CurrentObjective.Next;
				CurrentObjective?.Activate(this);
				ChatHelper.BroadcastChatMessage(new Terraria.Localization.NetworkText(objectiveCompleteText, Terraria.Localization.NetworkText.Mode.Literal), objectiveCompleteTextColor);
				Console.WriteLine(objectiveCompleteText);
				// TODO: Sync objective completion to all clients
			}
		}
	}

	public void Complete()
	{
		if (State != WorldMissionState.Active)
		{
			return;
		}

		if (NetUtils.IsClient)
		{
			Console.WriteLine("Waiting for complete packet.");
			return;
		}

		var completeText = $"[{DisplayName}]任务已完成";
		var completeTextColor = new Color(150, 250, 150);

		if (NetUtils.IsSingle)
		{
			State = WorldMissionState.Completed;
			OnComplete();
			Deactivate(); // Maybe not necessary, because the current objective is null here.
			Main.NewText(completeText, completeTextColor);
		}
		else if (NetUtils.IsServer)
		{
			State = WorldMissionState.Completed;
			OnComplete();
			Deactivate(); // Maybe not necessary, because the current objective is null.
			ChatHelper.BroadcastChatMessage(new Terraria.Localization.NetworkText(completeText, Terraria.Localization.NetworkText.Mode.Literal), completeTextColor);
			Console.WriteLine(completeText);
			// TODO: Sync completion to all clients
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

		if (NetUtils.IsServer)
		{
			Console.WriteLine("Waiting for retry packet.");
			return;
		}
		else if (NetUtils.IsSingle)
		{
			State = WorldMissionState.Active;
			Time = 0;
			ResetProgress();
			Activate();

			Main.NewText($"[{DisplayName}]任务已重启", 150, 250, 150);
		}
		else if (NetUtils.IsClient)
		{
			// TODO: Send retry request packet to server
		}
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

		if (NetUtils.IsSingle)
		{
			RewardClaimed = true;

			var rewardPlayer = Main.LocalPlayer;
			foreach (var item in RewardItems)
			{
				rewardPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_Misc(RewardItemsSourceContext), item, item.stack);
			}
		}
		else if (NetUtils.IsClient)
		{
			// TODO: Send reward claim request packet to server
		}
		else if (NetUtils.IsServer)
		{
			RewardClaimed = true;
			// TODO: Generate reward items and send them to the player who claimed the reward.
			// And record player name in RewardClaimedPlayers to prevent multiple claims from the same player.
			// TODO: Add extra hooks for world state changes and other types of rewards.
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