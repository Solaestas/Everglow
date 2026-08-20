namespace Everglow.Commons.Mechanics.Mission.Core;

public enum MissionNotificationType
{
	Unlocked,
	Restored,
	Failed,
	Completed,
	Restarted,
	ObjectiveCompleted,
}

public readonly record struct MissionNotification(
	MissionIdentity Mission,
	MissionNotificationType Type,
	string Detail = null);
