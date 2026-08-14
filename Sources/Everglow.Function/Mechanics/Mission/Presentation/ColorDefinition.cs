using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.PlayerSide;

namespace Everglow.Commons.Mechanics.Mission.Presentation;

public static class ColorDefinition
{
	public static readonly Color InitialLightColor = new Color(1f, 1f, 1f, 0f) * 0.8f;

	public static Color GetMissionStateColor(PlayerMissionState? poolType) => poolType switch
	{
		PlayerMissionState.Accepted => new Color(0f, 1f, 0f, 0f),
		PlayerMissionState.Available => new Color(0.9f, 0.88f, 0.06f, 0f),
		PlayerMissionState.Failed => new Color(1f, 0f, 0f, 0.3f),
		PlayerMissionState.Overdue => new Color(0.5f, 0f, 0.2f, 0.7f),
		PlayerMissionState.Completed => new Color(0.2f, 0.6f, 1f, 0.2f),
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

	public static Rectangle GetMissionStateFrame(PlayerMissionState? poolType) => poolType switch
	{
		PlayerMissionState.Accepted => new Rectangle(139, 36, 17, 67),
		PlayerMissionState.Available => new Rectangle(121, 36, 17, 67),
		PlayerMissionState.Failed => new Rectangle(103, 36, 17, 67),
		PlayerMissionState.Overdue => new Rectangle(85, 36, 17, 67),
		PlayerMissionState.Completed => new Rectangle(67, 36, 17, 67),
		null => new Rectangle(157, 36, 17, 67),
		_ => new Rectangle(157, 36, 17, 67),
	};
}
