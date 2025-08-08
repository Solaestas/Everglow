using Everglow.Yggdrasil.Common;
using SubworldLibrary;

namespace Everglow.Yggdrasil.YggdrasilTown.Biomes;

public class MidnightBayouBiome : ModBiome
{
	public override int Music => YggdrasilContent.QuickMusic(ModAsset.NewYggdrasilTownBGM_Path);

	public override SceneEffectPriority Priority => SceneEffectPriority.BossMedium;

	public override string BestiaryIcon => ModAsset.YggdrasilTownIcon_Mod;

	public override string BackgroundPath => ModAsset.MidnightBayou_MapBackground_Mod;

	public override string MapBackground => ModAsset.MidnightBayou_MapBackground_Mod;

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
		if (Main.maxRaining > 0)
		{
			Main.maxRaining = 0;
			Main.StopRain();
			Main.raining = false;
		}
		if (Main.slimeRain)
		{
			Main.StopSlimeRain();
		}
		Main.bloodMoon = false;
		base.OnInBiome(player);
	}
}