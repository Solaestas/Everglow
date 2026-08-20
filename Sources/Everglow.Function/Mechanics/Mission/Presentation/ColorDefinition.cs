using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.Presentation.Views;

namespace Everglow.Commons.Mechanics.Mission.Presentation;

public static class ColorDefinition
{
	public static Color GetMissionNotificationColor(MissionNotificationType type) => type switch
	{
		MissionNotificationType.Unlocked or MissionNotificationType.Restored => new Color(150, 150, 250),
		MissionNotificationType.Failed => new Color(250, 150, 150),
		MissionNotificationType.Completed or MissionNotificationType.Restarted => new Color(150, 250, 150),
		MissionNotificationType.ObjectiveCompleted => new Color(250, 250, 150),
		_ => throw new ArgumentOutOfRangeException(nameof(type)),
	};

	public static Rectangle GetGemFrame(MissionType? missionType) => missionType switch
	{
		MissionType.None => new Rectangle(231, 0, 33, 33),
		MissionType.MainStory => new Rectangle(198, 0, 33, 33),
		MissionType.SideStory => new Rectangle(165, 0, 33, 33),
		MissionType.Achievement => new Rectangle(99, 0, 33, 33),
		MissionType.Challenge => new Rectangle(33, 0, 33, 33),
		MissionType.Daily => new Rectangle(66, 0, 33, 33),
		MissionType.Legendary => new Rectangle(132, 0, 33, 33),
		_ => new Rectangle(0, 0, 33, 33),
	};

	public static Rectangle GetMissionStateFrame(MissionViewState? missionState) => missionState switch
	{
		MissionViewState.Active => new Rectangle(139, 36, 17, 67),
		MissionViewState.Available => new Rectangle(121, 36, 17, 67),
		MissionViewState.Failed => new Rectangle(103, 36, 17, 67),
		MissionViewState.Overdue => new Rectangle(85, 36, 17, 67),
		MissionViewState.Completed => new Rectangle(67, 36, 17, 67),
		null => new Rectangle(157, 36, 17, 67),
		_ => new Rectangle(157, 36, 17, 67),
	};
}
