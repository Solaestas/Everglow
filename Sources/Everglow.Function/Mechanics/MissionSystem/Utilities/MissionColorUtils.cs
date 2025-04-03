using Everglow.Commons.Mechanics.MissionSystem.Enums;

namespace Everglow.Commons.Mechanics.MissionSystem.Utilities;

public static class MissionColorUtils
{
	public static readonly Color InitialLightColor = new Color(1f, 1f, 1f, 0f) * 0.8f;

	public static Color GetPoolTypeColor(PoolType? poolType) => poolType switch
	{
		PoolType.Accepted => new Color(0f, 1f, 0f, 0f),
		PoolType.Available => new Color(1f, 1f, 0f, 0f),
		PoolType.Failed => new Color(1f, 0f, 0f, 0f),
		PoolType.Overdue => new Color(0.5f, 0.2f, 0.2f, 0.1f),
		PoolType.Completed => new Color(0f, 0f, 1f, 0f),
		null => InitialLightColor,
		_ => InitialLightColor,
	};

	public static Color GetMissionTypeColor(MissionType? missionType) => missionType switch
	{
		MissionType.None => new Color(0f, 0f, 0f, 0.1f),
		MissionType.MainStory => new Color(1f, 1f, 0f, 0f),
		MissionType.SideStory => new Color(1f, 0f, 1f, 0f),
		MissionType.Legendary => new Color(
			MathF.Sin((float)Main.timeForVisualEffects * 0.04f),
			MathF.Cos((float)Main.timeForVisualEffects * 0.03f),
			MathF.Sin((float)Main.timeForVisualEffects * 0.03f),
			0f),
		MissionType.Achievement => new Color(0f, 1f, 0f, 0f),
		MissionType.Daily => new Color(0f, 0f, 1f, 0f),
		MissionType.Challenge => new Color(1f, 0f, 0f, 0f),
		null => InitialLightColor,
		_ => InitialLightColor,
	};
}