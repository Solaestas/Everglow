using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Core;
using Everglow.Commons.Mechanics.Mission.WorldSide;
using Everglow.Commons.Mechanics.Mission.WorldSide.Abstractions;

namespace Everglow.Commons.Mechanics.Mission.Presentation;

public class MissionView
{
	public MissionView(MissionBase mission)
	{

	}

	public MissionView(WorldMissionBase mission)
	{
		DisplayName = mission.DisplayName;
		State = MapState(mission.State);
		Type = mission.MissionType;
		TimeLimit = mission.TimeLimit;
		TimeRemaining = mission.Time;
		Source = mission.MissionSource;
		Progress = mission.Progress;
		Visible = mission.Visible;
		Retriable = mission.Retriable;

		CompletedSteps = 0;
		TotalSteps = 0;

		//Description = mission.Description;
		//CompletedObjectives = mission.Objectives.AllObjectives.Where(o => o.Completed).Select(o => new ObjectiveView(o));

		//CurrentNode = new NodeView(mission.Objectives.FindCurrentNode());

		//CanClaimReward = !mission.RewardClaimed || !mission.RewardClaimedPlayers.Contains(Main.LocalPlayer.name);
		//Rewards = mission.RewardItems;
		//ExtraRewards = mission.ExtraRewardsText;
	}

	// Overview
	// ========
	public string DisplayName;
	public UIMissionState State;
	public MissionType Type;
	public int TimeLimit;
	public int TimeRemaining;
	public MissionSource Source;
	public float Progress;
	public bool Visible;
	public bool Retriable;

	// Detail
	// ======

	// Progress
	public int CompletedSteps;
	public int TotalSteps;

	public string Description;

	public IEnumerable<ObjectiveView> CompletedObjectives;

	// Current Objectives && Track Info
	public NodeView CurrentNode;

	// Reward
	public bool CanClaimReward;
	public IEnumerable<Item> Rewards;
	public IEnumerable<object> ExtraRewards;

	public static UIMissionState MapState(WorldMissionState state)
	{
		return state switch
		{
			WorldMissionState.Locked => UIMissionState.World_Locked,
			WorldMissionState.Active => UIMissionState.World_Active,
			WorldMissionState.Completed => UIMissionState.World_Completed,
			WorldMissionState.Failed => UIMissionState.World_Failed,
			_ => throw new InvalidDataException("Unknown world mission state."),
		};
	}
}