namespace Everglow.Commons.Mechanics.Mission.Core;

public enum MissionActionKind
{
	Accept,
	Cancel,
	Retry,
	ClaimReward,
}

public readonly record struct MissionAction(
	MissionIdentity Mission,
	MissionActionKind Kind);
