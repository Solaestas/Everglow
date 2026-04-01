using Everglow.Commons.Mechanics.Mission.WorldMission.Packets;
using Everglow.Commons.Utilities;

namespace Everglow.Commons.Mechanics.Mission.WorldMission.Base;

public abstract partial class WorldMissionBase : IMissionBehavior
{
	public const string RewardItemsSourceContext = "Everglow.MissionSystem";

	public int WhoAmI { get; internal set; }

	public WorldMissionState State { get; protected set; } = WorldMissionState.Locked;

	public virtual float Progress => 1;

	public WorldObjectiveContainer Objectives { get; } = new();

	/// <summary>
	/// Set by the mission on activation. It is possibly null.
	/// </summary>
	public WorldObjectiveBase CurrentObjective { get; protected set; }

	public int Time { get; protected set; }

	public bool Retriable => true;

	public bool RewardClaimed { get; protected set; }

	public HashSet<string> RewardClaimedPlayers { get; protected set; } = [];

	public void Unlock()
	{
		if (UnlockCore())
		{
			var unlockText = $"[{DisplayName}]任务已解锁";
			var unlockTextColor = new Color(150, 150, 250);
			Main.NewText(unlockText, unlockTextColor);

			if (NetUtils.IsServer)
			{
				Console.WriteLine(unlockText);
				ModIns.PacketResolver.Send(new MissionSyncPacket(this));
			}
		}
	}

	private bool UnlockCore()
	{
		if (State != WorldMissionState.Locked)
		{
			return false;
		}

		State = WorldMissionState.Active;
		Activate();
		OnUnlock();

		return true;
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

	public void ForceAdvanceObjective()
	{
		CurrentObjective.Deactivate(); // Make sure there's no registered hooks or other side effects from the current objective.
		CurrentObjective = CurrentObjective.Next;
		CurrentObjective.Activate(this);
	}

	public void Update()
	{
		if (!UpdateTime())
		{
			return;
		}

		if (CurrentObjective is null)
		{
			CompleteMission();
			return;
		}

		// Pass the completed objective.
		// This block often happens when the mission just got loaded, or synced from server.
		if (CurrentObjective.Completed)
		{
			ForceAdvanceObjective();
			return;
		}

		CurrentObjective.Update();

		// Check objective completion.
		if (CurrentObjective.CheckCompletion())
		{
			CompleteObjective();
		}
	}

	public bool UpdateTime()
	{
		if (TimeLimit <= 0)
		{
			return true;
		}

		Time += WorldMissionManager.UpdateInterval;
		if (Time >= TimeLimit)
		{
			Time = TimeLimit;

			if (ExpireCore())
			{
				var failText = $"[{DisplayName}]任务已失败";
				var failTextColor = new Color(250, 150, 150);
				Main.NewText(failText, failTextColor);

				if (NetUtils.IsServer)
				{
					Console.WriteLine(failText);
					// ChatHelper.BroadcastChatMessage(new Terraria.Localization.NetworkText(failText, Terraria.Localization.NetworkText.Mode.Literal), failTextColor);
					ModIns.PacketResolver.Send(new MissionSyncPacket(this));
				}
			}

			return false;
		}

		return true;
	}

	private bool ExpireCore()
	{
		if (State != WorldMissionState.Active)
		{
			return false;
		}

		State = WorldMissionState.Failed;
		OnExpire();
		Deactivate();

		return true;
	}

	public void CompleteMission()
	{
		if (CompleteMissionCore())
		{
			var completeText = $"[{DisplayName}]任务已完成";
			var completeTextColor = new Color(150, 250, 150);
			Main.NewText(completeText, completeTextColor);

			if (NetUtils.IsServer)
			{
				Console.WriteLine(completeText);
				// ChatHelper.BroadcastChatMessage(new Terraria.Localization.NetworkText(completeText, Terraria.Localization.NetworkText.Mode.Literal), completeTextColor);
				ModIns.PacketResolver.Send(new MissionSyncPacket(this));
			}
		}
	}

	private bool CompleteMissionCore()
	{
		if (State != WorldMissionState.Active)
		{
			return false;
		}

		State = WorldMissionState.Completed;
		OnComplete();
		Deactivate(); // Maybe not necessary, because the current objective is null here.

		return true;
	}

	public void CompleteObjective()
	{
		if (CompleteObjectiveCore())
		{
			var objectiveCompleteText = $"[{DisplayName}]任务当前目标已完成";
			var objectiveCompleteTextColor = new Color(250, 250, 150);
			Main.NewText(objectiveCompleteText, objectiveCompleteTextColor);

			if (NetUtils.IsServer)
			{
				Console.WriteLine(objectiveCompleteText);
				// ChatHelper.BroadcastChatMessage(new Terraria.Localization.NetworkText(objectiveCompleteText, Terraria.Localization.NetworkText.Mode.Literal), objectiveCompleteTextColor);
				if (CurrentObjective.Next != null) // Skip the last objective sync because a packet for completion will be sent.
				{
					ModIns.PacketResolver.Send(new MissionSyncPacket(this));
				}
			}
		}
	}

	private bool CompleteObjectiveCore()
	{
		if (State != WorldMissionState.Active || CurrentObjective is null)
		{
			return false;
		}

		CurrentObjective.Complete();
		CurrentObjective.Deactivate();
		CurrentObjective = CurrentObjective.Next;
		CurrentObjective?.Activate(this);

		return true;
	}

	/// <summary>
	/// Requests the mission retry. Called by clicking the 'retry' button in the mission panel.
	/// </summary>
	public void Retry()
	{
		if (NetUtils.IsServer)
		{
			Console.WriteLine("Waiting for retry packet.");
			return;
		}

		if (NetUtils.IsSingle)
		{
			if (!RetryCore())
			{
				return;
			}

			Main.NewText($"[{DisplayName}]任务已重启", 150, 250, 150);
		}
		else if (NetUtils.IsClient)
		{
			// TODO: Send retry request packet to server
		}
	}

	public bool RetryCore()
	{
		if (!Retriable || State != WorldMissionState.Failed)
		{
			return false;
		}

		State = WorldMissionState.Active;
		Time = 0;
		ResetProgress();
		Activate();

		return true;
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
			if (GiveRewardsCore(Main.LocalPlayer.name))
			{
				foreach (var item in RewardItems)
				{
					Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_Misc(RewardItemsSourceContext), item, item.stack);
				}
			}
		}
		else if (NetUtils.IsClient)
		{
			// TODO: Send reward claim request packet to server
		}
	}

	private bool GiveRewardsCore(string player)
	{
		if (State != WorldMissionState.Completed)
		{
			return false;
		}

		if (!RewardClaimed)
		{
			RewardClaimed = true;
		}

		// TODO: We haven't decided how to handle rewards for multiplayers yet.
		RewardClaimedPlayers.Add(player);

		return true;
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
		CurrentObjective = Objectives.First;
		foreach (var objective in Objectives.AllObjectives)
		{
			objective.ResetProgress();
		}
	}

	/// <summary>
	/// Use this method to adapt to a fully new snapshot.
	/// <br/> This is called when loading a world, or syncing from server.
	/// </summary>
	/// <param name="newState"></param>
	/// <param name="oldState"></param>
	private void ApplySnapshot(WorldMissionState newState, WorldMissionState oldState)
	{
		if (CurrentObjective == Objectives.FirstIncomplete)
		{
			if (oldState == newState)
			{
				return;
			}

			if (oldState is WorldMissionState.Locked or WorldMissionState.Failed
				&& newState is WorldMissionState.Active)
			{
				CurrentObjective.Activate(this);
			}
			else if (oldState is WorldMissionState.Active
				&& newState is WorldMissionState.Failed or WorldMissionState.Completed)
			{
				CurrentObjective.Deactivate();
			}
		}
		else
		{
			var oldObjective = CurrentObjective;
			CurrentObjective = Objectives.FirstIncomplete;
			if (oldState == WorldMissionState.Active && newState == WorldMissionState.Active)
			{
				oldObjective?.Deactivate();
				CurrentObjective?.Activate(this);
			}
			else if (oldState is WorldMissionState.Locked or WorldMissionState.Failed
				&& newState is WorldMissionState.Active)
			{
				CurrentObjective.Activate(this);
			}
			else if (oldState is WorldMissionState.Active
				&& newState is WorldMissionState.Failed or WorldMissionState.Completed)
			{
				oldObjective.Deactivate();
			}
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