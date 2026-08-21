using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.PlayerSide;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Abstractions;
using Everglow.Commons.Mechanics.Mission.PlayerSide.MissionStructure.Nodes;
using Everglow.Commons.Mechanics.Mission.Presentation.Icons;
using Everglow.Commons.Mechanics.Mission.Presentation.Views;

namespace Everglow.Commons.Mechanics.Mission.Presentation.Adapters;

public static class PlayerMissionViewAdapter
{
	public static MissionView Create(PlayerMissionBase mission)
	{
		ArgumentNullException.ThrowIfNull(mission);

		string hint = mission.Hint ?? string.Empty;
		float progress = ClampProgress(mission.Progress);
		int elapsedTime = mission.Time;
		int missionTimeLimit = mission.TimeLimit;
		int? timeLimit = missionTimeLimit <= 0 ? null : missionTimeLimit;
		ObjectiveNodeView[] objectiveNodes = CreateObjectiveNodes(mission);
		RewardView[] rewards = CreateRewards(mission);

		MissionIconBase[] icons = CreateIcons(mission);

		return new MissionView
		{
			Identity = new MissionIdentity(MissionSide.Player, mission.Name, mission.InstanceId),
			Source = mission.Source ?? MissionSourceBase.Default,
			SubSource = mission.SubSource,
			Type = mission.Type,
			DisplayName = mission.DisplayName ?? string.Empty,
			Description = mission.Description ?? string.Empty,
			Hint = hint,
			Visible = mission.IsVisible,
			Icons = icons,
			State = MapState(mission.State),
			Progress = progress,
			ElapsedTime = elapsedTime,
			TimeLimit = timeLimit,
			ObjectiveNodes = objectiveNodes,
			Rewards = rewards,
		};
	}

	private static MissionIconBase[] CreateIcons(PlayerMissionBase mission)
	{
		var iconGroup = new MissionIconGroup();
		iconGroup.Add(MissionSourceIcon.Create(mission.Source ?? MissionSourceBase.Default, mission.SubSource));
		mission.Objectives.GetObjectivesIcon(iconGroup);
		return iconGroup.Icons.ToArray();
	}

	private static ObjectiveNodeView[] CreateObjectiveNodes(PlayerMissionBase mission)
	{
		var activeObjectives = new HashSet<PlayerObjectiveBase>(ReferenceEqualityComparer.Instance);
		if (mission.State == PlayerMissionState.Accepted)
		{
			activeObjectives.UnionWith(mission.Objectives.FindCurrentObjectives());
		}

		return mission.Objectives.AllNodes
			.Select(node => CreateObjectiveNode(node, activeObjectives))
			.ToArray();
	}

	private static ObjectiveNodeView CreateObjectiveNode(
		PlayerObjectiveNodeBase node,
		IReadOnlySet<PlayerObjectiveBase> activeObjectives)
	{
		return node switch
		{
			PlayerLeafNode leaf => new LeafObjectiveNodeView(
				CreateObjective(leaf.Objective, activeObjectives, skipped: false)),
			PlayerParallelNode parallel => new ParallelObjectiveNodeView(
				parallel.Objectives
					.Select(objective => CreateObjective(objective, activeObjectives, skipped: false))
					.ToArray()),
			PlayerOptionalNode optional => new AnyOfObjectiveNodeView(
				optional.Objectives
					.Select(objective => CreateObjective(objective, activeObjectives, skipped: false))
					.ToArray()),
			PlayerBranchNode branch => CreateBranchNode(branch, activeObjectives),
			_ => throw new InvalidDataException($"Unknown player objective node type {node.GetType().FullName}."),
		};
	}

	private static BranchObjectiveNodeView CreateBranchNode(
		PlayerBranchNode node,
		IReadOnlySet<PlayerObjectiveBase> activeObjectives)
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
		PlayerObjectiveBase objective,
		IReadOnlySet<PlayerObjectiveBase> activeObjectives,
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
			Description = objective.Description,
			ObjectiveText = objective.GetObjectiveText(),
			Progress = progress,
			State = state,
		};
	}

	private static RewardView[] CreateRewards(PlayerMissionBase mission)
	{
		List<Item> rewardItems = mission.RewardItems;
		return rewardItems is null
			? []
			: rewardItems.Select(item => new RewardView { Item = item }).ToArray();
	}

	private static MissionViewState MapState(PlayerMissionState state)
	{
		return state switch
		{
			PlayerMissionState.Available => MissionViewState.Available,
			PlayerMissionState.Accepted => MissionViewState.Active,
			PlayerMissionState.Completed => MissionViewState.Completed,
			PlayerMissionState.Failed => MissionViewState.Failed,
			PlayerMissionState.Overdue => MissionViewState.Overdue,
			_ => throw new InvalidDataException($"Unknown player mission state {state}."),
		};
	}

	private static float ClampProgress(float progress) => float.IsNaN(progress)
		? 0f
		: Math.Clamp(progress, 0f, 1f);
}
