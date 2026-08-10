namespace Everglow.Commons.Mechanics.Mission.Presentation.Views;

public readonly record struct MissionIdentity(
	MissionSide Side,
	string DefinitionId,
	string InstanceId);
