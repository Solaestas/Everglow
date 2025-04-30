using Everglow.Yggdrasil.Common;
using SubworldLibrary;

namespace Everglow.Yggdrasil.YggdrasilTown;

public class MidnightBayouBiome : ModBiome
{
	public override int Music => !Main.dayTime ? Common.YggdrasilContent.QuickMusic("NewYggdrasilTownBGM") : Common.YggdrasilContent.QuickMusic("YggdrasilTownBGM");

	public override SceneEffectPriority Priority => SceneEffectPriority.BossMedium;

	public override string BestiaryIcon => "Everglow/Yggdrasil/YggdrasilTown/YggdrasilTownIcon";

	public override string BackgroundPath => "Everglow/" + ModAsset.MidnightBayou_MapBackground_Path;

	public override string MapBackground => "Everglow/" + ModAsset.MidnightBayou_MapBackground_Path;

	public override ModWaterStyle WaterStyle => ModContent.GetInstance<Water.YggdrasilTownWaterStyle>();

	public override ModUndergroundBackgroundStyle UndergroundBackgroundStyle => base.UndergroundBackgroundStyle;

	public override Color? BackgroundColor => base.BackgroundColor;

	public override void Load()
	{
		base.Load();
	}

	public override bool IsBiomeActive(Player player)
	{
		if (!SubworldSystem.IsActive<YggdrasilWorld>())
		{
			return false;
		}
		return (new Point(1395, Main.maxTilesY - 405).ToWorldCoordinates() - (Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f)).Length() < 5000;
	}

	public override void OnInBiome(Player player)
	{
		Main.raining = false;
		Main.bloodMoon = false;
		base.OnInBiome(player);
	}
}