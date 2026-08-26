using Everglow.Commons.Mechanics.Quest.Core;
using Everglow.Commons.Mechanics.Quest.Presentation.Icons;

namespace Everglow.Commons.Mechanics.Quest.Presentation.Views;

public sealed class QuestView
{
	public QuestIdentity Identity { get; init; }

	public QuestSourceBase Source { get; init; } = QuestSourceBase.Default;

	public QuestSourceBase SubSource { get; init; }

	public QuestType Type { get; init; }

	public string DisplayName { get; init; } = string.Empty;

	public string Description { get; init; } = string.Empty;

	public string Hint { get; init; } = string.Empty;

	public bool Visible { get; init; }

	public IReadOnlyList<QuestIconBase> Icons { get; init; } = [];

	public QuestViewState State { get; init; }

	public float Progress { get; init; }

	public int ElapsedTime { get; init; }

	public int? TimeLimit { get; init; }

	public int? RemainingTime => TimeLimit is int limit
		? Math.Max(0, limit - ElapsedTime)
		: null;

	public IReadOnlyList<ObjectiveNodeView> ObjectiveNodes { get; init; } = [];

	public TimerView PrimaryObjectiveTimer => EnumerateObjectives()
		.Where(objective => objective.Timer is not null
			&& objective.State is ObjectiveViewState.Active or ObjectiveViewState.TimedOut)
		.OrderBy(objective => objective.Timer.RemainingTime)
		.Select(objective => objective.Timer)
		.FirstOrDefault();

	public IReadOnlyList<RewardView> Rewards { get; init; } = [];

	private IEnumerable<ObjectiveView> EnumerateObjectives()
	{
		foreach (ObjectiveNodeView node in ObjectiveNodes)
		{
			switch (node)
			{
				case LeafObjectiveNodeView leaf:
					yield return leaf.Objective;
					break;
				case ParallelObjectiveNodeView parallel:
					foreach (ObjectiveView objective in parallel.Objectives)
					{
						yield return objective;
					}
					break;
				case AnyOfObjectiveNodeView anyOf:
					foreach (ObjectiveView objective in anyOf.Objectives)
					{
						yield return objective;
					}
					break;
				case BranchObjectiveNodeView branch:
					foreach (ObjectiveView objective in branch.Branches.SelectMany(branchView => branchView.Objectives))
					{
						yield return objective;
					}
					break;
			}
		}
	}
}
