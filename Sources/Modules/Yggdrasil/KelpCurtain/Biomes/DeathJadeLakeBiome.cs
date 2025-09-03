using Everglow.Yggdrasil.Common;
using Everglow.Yggdrasil.KelpCurtain.Water;
using Everglow.Yggdrasil.WorldGeneration;

namespace Everglow.Yggdrasil.KelpCurtain.Biomes;

public class DeathJadeLakeBiome : ModBiome
{
	public float LiquidSurfaceY = 0;

	public void GetLiquidSurfaceY()
	{
		int checkY = (int)(Main.maxTilesY * 0.88f);
		int checkX = Main.maxTilesX / 2;
		for (int j = 0; j < 1000; j++)
		{
			var tile = YggdrasilWorldGeneration.SafeGetTile(checkX, checkY + j);
			if (tile.LiquidAmount > 0)
			{
				LiquidSurfaceY = (checkY + j) * 16f + 16 - tile.LiquidAmount / 16f; // LiquidAmount 0 ~255
				break;
			}
		}
	}

	/// <summary>
	/// TODO: BGM
	/// </summary>
	public override int Music => YggdrasilContent.QuickMusic(ModAsset.JellyBallHotbed_BGM_Path);

	public override SceneEffectPriority Priority => SceneEffectPriority.Environment;

	public override string BestiaryIcon => ModAsset.DeathJadeLakeIcon_Mod;

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
	public override ModWaterStyle WaterStyle => ModContent.GetInstance<KelpCurtainWaterStyle>();

	public override Color? BackgroundColor => base.BackgroundColor;

	public override void Load()
	{
		base.Load();
	}

	public override bool IsBiomeActive(Player player)
	{
		bool flag1 = player.Center.X / 16 > Main.maxTilesX * 0.05f && player.Center.X / 16 < Main.maxTilesX * 0.75f;
		bool flag2 = player.Center.Y / 16 > Main.maxTilesY * 0.87f && player.Center.Y / 16 < Main.maxTilesY * 0.9f;
		bool flag3 = player.InModBiome<KelpCurtainBiome>();
		return flag1 && flag2 && flag3;
	}

	public override void OnInBiome(Player player)
	{
		base.OnInBiome(player);
	}
}