namespace Everglow.Commons.Mechanics.Mission.Core;

public enum MissionActionType
{
	Accept,
	Cancel,
	Retry,
	ClaimReward,
}

public readonly record struct MissionAction(
	MissionIdentity Mission,
	MissionActionType Type);
