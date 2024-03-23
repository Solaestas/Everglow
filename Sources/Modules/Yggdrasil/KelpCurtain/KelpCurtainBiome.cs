using Everglow.Yggdrasil.Common;
using Everglow.Yggdrasil.KelpCurtain.Background;
using Terraria;
using Terraria.Graphics.Light;

namespace Everglow.Yggdrasil.KelpCurtain;

public class KelpCurtainBiome : ModBiome
{
	public override int Music => !Main.dayTime ? YggdrasilContent.QuickMusic("KelpCurtainBGM") : YggdrasilContent.QuickMusic("OldKelpCurtainBGM");
	public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
	public override string BestiaryIcon => "Everglow/Yggdrasil/KelpCurtain/KelpCurtainIcon";
	public override string BackgroundPath => base.BackgroundPath;
	public override string MapBackground => "Everglow/Yggdrasil/KelpCurtain/Backgrounds/KelpCurtain_MapBackground";
	public override ModWaterStyle WaterStyle => ModContent.GetInstance<Water.KelpCurtainWaterStyle>();
	public override Color? BackgroundColor => base.BackgroundColor;

	public override void Load()
	{
		base.Load();
	}
	public override bool IsBiomeActive(Player player)
	{
		return KelpCurtainBackground.BiomeActive();
	}

	public override void OnInBiome(Player player)
	{
		base.OnInBiome(player);
	}
}