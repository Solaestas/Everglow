using Everglow.Yggdrasil.Common;

namespace Everglow.Yggdrasil.YggdrasilTown;

public class LampWoodForest : ModBiome
{
	public override int Music => !Main.dayTime ? Common.YggdrasilContent.QuickMusic("NewYggdrasilTownBGM") : Common.YggdrasilContent.QuickMusic("YggdrasilTownBGM");

	public override SceneEffectPriority Priority => SceneEffectPriority.BossMedium;

	public override string BestiaryIcon => "Everglow/Yggdrasil/YggdrasilTown/YggdrasilTownIcon";

	public override string BackgroundPath => "Everglow/" + ModAsset.LampWood_MapBackground_Path;

	public override string MapBackground => "Everglow/" + ModAsset.LampWood_MapBackground_Path;

	public override ModWaterStyle WaterStyle => ModContent.GetInstance<Water.YggdrasilTownWaterStyle>();

	public override ModUndergroundBackgroundStyle UndergroundBackgroundStyle => base.UndergroundBackgroundStyle;

	public override Color? BackgroundColor => base.BackgroundColor;

	public override void Load()
	{
		base.Load();
	}

	public override bool IsBiomeActive(Player player)
	{
		return ModContent.GetInstance<YggdrasilBiomeTileCounter>().DarkForestGrassCount > 150;
	}

	public override void OnInBiome(Player player)
	{
		Main.raining = false;
		Main.bloodMoon = false;
		base.OnInBiome(player);
	}
}