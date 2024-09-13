using Everglow.Yggdrasil.Common;
using Everglow.Yggdrasil.HurricaneMaze.Background;

namespace Everglow.Yggdrasil.HurricaneMaze;

public class HurricaneMazeBiome : ModBiome
{
	public override int Music => !Main.dayTime ? YggdrasilContent.QuickMusic("HurricaneMazeBGM") : YggdrasilContent.QuickMusic("OldHurricaneMazeBGM");
	public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
	public override string BestiaryIcon => "Everglow/Yggdrasil/HurricaneMaze/HurricaneMazeIcon";
	public override string BackgroundPath => base.BackgroundPath;
	public override string MapBackground => "Everglow/Yggdrasil/HurricaneMaze/Backgrounds/HurricaneMaze_MapBackground";
	public override ModWaterStyle WaterStyle => ModContent.GetInstance<Water.HurricaneMazeWaterStyle>();
	public override Color? BackgroundColor => base.BackgroundColor;

	public override void Load()
	{
		base.Load();
	}
	public override bool IsBiomeActive(Player player)
	{
		return HurricaneMazeBackground.BiomeActive();
	}

	public override void OnInBiome(Player player)
	{
		base.OnInBiome(player);
	}
}