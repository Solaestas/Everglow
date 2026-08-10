using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.Presentation.Views;
using Everglow.Commons.Mechanics.Mission.WorldSide;
using Everglow.Commons.Mechanics.Mission.WorldSide.Abstractions;
using Everglow.Commons.Mechanics.Mission.WorldSide.MissionStructure.Nodes;

namespace Everglow.Commons.Mechanics.Mission.Presentation.Adapters;

public static class WorldMissionViewAdapter
{
	public static MissionView Create(WorldMissionBase mission)
	{
		ArgumentNullException.ThrowIfNull(mission);

		string definitionId = mission.Name;
		string hint = mission.Hint ?? string.Empty;
		bool hidesDetails = hint.Length > 0;
		ObjectiveNodeView[] objectiveNodes = hidesDetails ? [] : CreateObjectiveNodes(mission);
		RewardView[] rewards = hidesDetails ? [] : CreateRewards(mission);
		float progress = 0f;
		long elapsedTime = 0;
		long? timeLimit = null;
		if (!hidesDetails)
		{
			progress = ClampProgress(mission.Progress);
			elapsedTime = mission.Time;
			int missionTimeLimit = mission.TimeLimit;
			timeLimit = missionTimeLimit <= 0 ? null : missionTimeLimit;
		}

		return new MissionView
		{
			Identity = new MissionIdentity(MissionSide.World, definitionId, definitionId),
			Source = mission.Source ?? MissionSourceBase.Default,
			SubSource = null,
			Type = mission.Type,
			DisplayName = mission.DisplayName ?? string.Empty,
			Description = hidesDetails ? string.Empty : mission.Description ?? string.Empty,
			Hint = hint,
			Visible = mission.Visible,
			Icons = [],
			State = MapState(mission.State),
			Progress = progress,
			ElapsedTime = elapsedTime,
			TimeLimit = timeLimit,
			ObjectiveNodes = objectiveNodes,
			Rewards = rewards,
		};
	}

	private static ObjectiveNodeView[] CreateObjectiveNodes(WorldMissionBase mission)
	{
		var activeObjectives = new HashSet<WorldObjectiveBase>(ReferenceEqualityComparer.Instance);
		if (mission.State == WorldMissionState.Active)
		{
			activeObjectives.UnionWith(mission.Objectives.FindCurrentObjectives());
		}

		return mission.Objectives.AllNodes
			.Select(node => CreateObjectiveNode(node, activeObjectives))
			.ToArray();
	}

	private static ObjectiveNodeView CreateObjectiveNode(
		WorldObjectiveNodeBase node,
		IReadOnlySet<WorldObjectiveBase> activeObjectives)
	{
		return node switch
		{
			WorldLeafNode leaf => new LeafObjectiveNodeView(
				CreateObjective(leaf.Objective, activeObjectives, skipped: false)),
			WorldParallelNode parallel => new ParallelObjectiveNodeView(
				parallel.Objectives
					.Select(objective => CreateObjective(objective, activeObjectives, skipped: false))
					.ToArray()),
			WorldOptionalNode optional => new AnyOfObjectiveNodeView(
				optional.Objectives
					.Select(objective => CreateObjective(objective, activeObjectives, skipped: false))
					.ToArray()),
			WorldBranchNode branch => CreateBranchNode(branch, activeObjectives),
			_ => throw new InvalidDataException($"Unknown world objective node type {node.GetType().FullName}."),
		};
	}

	private static BranchObjectiveNodeView CreateBranchNode(
		WorldBranchNode node,
		IReadOnlySet<WorldObjectiveBase> activeObjectives)
	{
		int? selectedBranchIndex = node.SelectedBranchIndex;
		var branches = new ObjectiveBranchView[node.Branches.Count];
		for (int branchIndex = 0; branchIndex < node.Branches.Count; branchIndex++)
		{
			ObjectiveBranchState state = selectedBranchIndex switch
			{
				null => ObjectiveBranchState.Candidate,
				int selected when selected == branchIndex => ObjectiveBranchState.Selected,
				_ => ObjectiveBranchState.Skipped,
			};
			bool skipped = state == ObjectiveBranchState.Skipped;
			ObjectiveView[] objectives = node.Branches[branchIndex]
				.Select(objective => CreateObjective(objective, activeObjectives, skipped))
				.ToArray();
			branches[branchIndex] = new ObjectiveBranchView(state, objectives);
		}

		return new BranchObjectiveNodeView(branches);
	}

	private static ObjectiveView CreateObjective(
		WorldObjectiveBase objective,
		IReadOnlySet<WorldObjectiveBase> activeObjectives,
		bool skipped)
	{
		ObjectiveViewState state;
		float progress;
		if (skipped)
		{
			state = ObjectiveViewState.Skipped;
			progress = 0f;
		}
		else if (objective.Completed)
		{
			state = ObjectiveViewState.Completed;
			progress = 1f;
		}
		else
		{
			state = activeObjectives.Contains(objective)
				? ObjectiveViewState.Active
				: ObjectiveViewState.Pending;
			progress = ClampProgress(objective.Progress);
		}

		return new ObjectiveView
		{
			Id = objective.ObjectiveID,
			Description = string.Empty,
			Progress = progress,
			State = state,
		};
	}

	private static RewardView[] CreateRewards(WorldMissionBase mission)
	{
		List<Item> rewardItems = mission.RewardItems;
		return rewardItems is null
			? []
			: rewardItems.Select(item => new RewardView { Item = item }).ToArray();
	}

	private static MissionViewState MapState(WorldMissionState state)
	{
		return state switch
		{
			WorldMissionState.Locked => MissionViewState.Locked,
			WorldMissionState.Active => MissionViewState.Active,
			WorldMissionState.Completed => MissionViewState.Completed,
			WorldMissionState.Failed => MissionViewState.Failed,
			_ => throw new InvalidDataException($"Unknown world mission state {state}."),
		};
	}

	private static float ClampProgress(float progress) => float.IsNaN(progress)
		? 0f
		: Math.Clamp(progress, 0f, 1f);
}
