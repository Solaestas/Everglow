namespace Everglow.Commons.Mechanics.Mission.Core;

public enum MissionActionType
{
	Accept,
	Cancel,
	Retry,
	ClaimReward,
	Submit,
}

public readonly record struct MissionAction(
	MissionIdentity Mission,
	MissionActionType Type);
