using Everglow.Yggdrasil.Common;

namespace Everglow.Yggdrasil.YggdrasilTown;

public class FurnaceAreaBiome : ModBiome
{
	public override int Music => YggdrasilContent.QuickMusic(ModAsset.FurnaceArea_BGM_Path);

	public override SceneEffectPriority Priority => SceneEffectPriority.Environment;

	public override string BestiaryIcon => ModAsset.FurnaceAreaIcon_Mod;

	public override string BackgroundPath => ModAsset.YggdrasilTown_MapBackground_Mod;

	public override string MapBackground => ModAsset.YggdrasilTown_MapBackground_Mod;

	public override ModWaterStyle WaterStyle => ModContent.GetInstance<Water.YggdrasilTownWaterStyle>();

	public override Color? BackgroundColor => base.BackgroundColor;

	public override void Load()
	{
		base.Load();
	}

	public override bool IsBiomeActive(Player player)
	{
		return YggdrasilTownCentralSystem.InFurnace_YggdrasilTown();
	}

	public override void OnInBiome(Player player)
	{
		base.OnInBiome(player);
	}
}