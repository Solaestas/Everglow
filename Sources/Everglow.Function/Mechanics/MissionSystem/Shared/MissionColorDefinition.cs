using Everglow.Commons.Mechanics.MissionSystem.Enums;

namespace Everglow.Commons.Mechanics.MissionSystem.Shared;

public static class MissionColorDefinition
{
	public static readonly Color InitialLightColor = new Color(1f, 1f, 1f, 0f) * 0.8f;

	public static Color GetPoolTypeColor(PoolType? poolType) => poolType switch
	{
		PoolType.Accepted => new Color(0f, 1f, 0f, 0f),
		PoolType.Available => new Color(0.9f, 0.88f, 0.06f, 0f),
		PoolType.Failed => new Color(1f, 0f, 0f, 0.3f),
		PoolType.Overdue => new Color(0.5f, 0f, 0.2f, 0.7f),
		PoolType.Completed => new Color(0.2f, 0.6f, 1f, 0.2f),
		null => InitialLightColor,
		_ => InitialLightColor,
	};

	public static Color GetMissionTypeColor(MissionType? missionType) => missionType switch
	{
		MissionType.None => new Color(0f, 0f, 0f, 1f),
		MissionType.MainStory => new Color(1f, 0.9f, 0.1f, 0f),
		MissionType.SideStory => new Color(0.4f, 0.1f, 1f, 0.4f),
		MissionType.Legendary => Color.Lerp(
			new Color(
			MathF.Sin((float)Main.timeForVisualEffects * 0.04f),
			MathF.Sin((float)Main.timeForVisualEffects * 0.04f + MathHelper.TwoPi / 3f),
			MathF.Sin((float)Main.timeForVisualEffects * 0.04f + MathHelper.TwoPi / 3f * 2),
			0f), new Color(1f, 1f, 1f, 0), 0.35f),
		MissionType.Achievement => new Color(0.15f, 0.7f, 0.3f, 0.3f),
		MissionType.Daily => new Color(0f, 0.2f, 1f, 0.4f),
		MissionType.Challenge => new Color(1f, 0f, 0f, 0.3f),
		null => InitialLightColor,
		_ => InitialLightColor,
	};
}