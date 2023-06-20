using Terraria.Graphics.Light;

namespace Everglow.Yggdrasil.YggdrasilTown;

public class YggdrasilTownBiome : ModBiome
{
	public override int Music => !Main.dayTime ? Common.YggdrasilContent.QuickMusic("NewYggdrasilTownBGM") : Common.YggdrasilContent.QuickMusic("YggdrasilTownBGM");
	public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
	public override string BestiaryIcon => "Everglow/Yggdrasil/YggdrasilTown/YggdrasilTownIcon";
	public override string BackgroundPath => base.BackgroundPath;
	public override string MapBackground => "Everglow/Yggdrasil/YggdrasilTown/Backgrounds/YggdrasilTown_MapBackground";
	public override ModWaterStyle WaterStyle => ModContent.GetInstance<Water.YggdrasilTownWaterStyle>();
	public override Color? BackgroundColor => base.BackgroundColor;

	public override void Load()
	{
		base.Load();
	}
	public override bool IsBiomeActive(Player player)
	{
		return Background.YggdrasilTownBackground.BiomeActive();
	}

	public override void OnInBiome(Player player)
	{
		base.OnInBiome(player);
	}
}