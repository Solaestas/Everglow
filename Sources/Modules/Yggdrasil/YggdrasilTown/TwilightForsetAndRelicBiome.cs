using Everglow.Yggdrasil.Common;
using SubworldLibrary;

namespace Everglow.Yggdrasil.YggdrasilTown;

public class TwilightForsetAndRelic : ModBiome
{
	public override int Music => YggdrasilContent.QuickMusic(ModAsset.NewYggdrasilTownBGM_Path);

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
		if (!SubworldSystem.IsActive<YggdrasilWorld>())
		{
			return false;
		}
		return (new Point(Main.maxTilesX / 2, 20000).ToWorldCoordinates() - (Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f)).Length() < 5000;
	}

	public override void OnInBiome(Player player)
	{
		Main.raining = false;
		Main.bloodMoon = false;
		base.OnInBiome(player);
	}
}