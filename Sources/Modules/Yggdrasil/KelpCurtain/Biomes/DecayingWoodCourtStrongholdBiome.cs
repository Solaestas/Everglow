using Everglow.Yggdrasil.Common;

namespace Everglow.Yggdrasil.YggdrasilTown.Biomes;

public class DecayingWoodCourtStrongholdBiome : ModBiome
{
	/// <summary>
	/// TODO: BGM
	/// </summary>
	public override int Music => YggdrasilContent.QuickMusic(ModAsset.JellyBallHotbed_BGM_Path);

	public override SceneEffectPriority Priority => SceneEffectPriority.Environment;

	/// <summary>
	/// TODO:Icon
	/// </summary>
	public override string BestiaryIcon => ModAsset.JellyBallHotbedIcon_Mod;

	/// <summary>
	/// TODO:Background
	/// </summary>
	public override string BackgroundPath => ModAsset.YggdrasilTown_MapBackground_Mod;

	/// <summary>
	/// TODO:MapBackground
	/// </summary>
	public override string MapBackground => ModAsset.YggdrasilTown_MapBackground_Mod;

	/// <summary>
	/// TODO:WaterStyle
	/// </summary>
	public override ModWaterStyle WaterStyle => ModContent.GetInstance<Water.YggdrasilTownWaterStyle>();

	public override Color? BackgroundColor => base.BackgroundColor;

	public override void Load()
	{
		base.Load();
	}

	public override bool IsBiomeActive(Player player)
	{
		return ModContent.GetInstance<YggdrasilBiomeTileCounter>().JellyBallSecretionCount > 150;
	}

	public override void OnInBiome(Player player)
	{
		base.OnInBiome(player);
	}
}