namespace Everglow.Commons.Mechanics.Mission.Core;

public readonly record struct MissionIdentity(
	MissionSide Side,
	string DefinitionId,
	string InstanceId);
