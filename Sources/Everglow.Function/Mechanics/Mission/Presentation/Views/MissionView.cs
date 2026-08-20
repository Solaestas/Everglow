using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.Presentation.Icons;

namespace Everglow.Commons.Mechanics.Mission.Presentation.Views;

public sealed class MissionView
{
	public MissionIdentity Identity { get; init; }

	public MissionSourceBase Source { get; init; } = MissionSourceBase.Default;

	public MissionSourceBase SubSource { get; init; }

	public MissionType Type { get; init; }

	public string DisplayName { get; init; } = string.Empty;

	public string Description { get; init; } = string.Empty;

	public string Hint { get; init; } = string.Empty;

	public bool Visible { get; init; }

	public IReadOnlyList<MissionIconBase> Icons { get; init; } = [];

	public MissionViewState State { get; init; }

	public float Progress { get; init; }

	public int ElapsedTime { get; init; }

	public int? TimeLimit { get; init; }

	public int? RemainingTime => TimeLimit is int limit
		? Math.Max(0, limit - ElapsedTime)
		: null;

	public IReadOnlyList<ObjectiveNodeView> ObjectiveNodes { get; init; } = [];

	public IReadOnlyList<RewardView> Rewards { get; init; } = [];
}
