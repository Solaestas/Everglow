namespace Everglow.Commons.Mechanics.Mission.Presentation;

public readonly record struct MissionIdentity(
	MissionSide Side,
	string DefinitionId,
	string InstanceId);
