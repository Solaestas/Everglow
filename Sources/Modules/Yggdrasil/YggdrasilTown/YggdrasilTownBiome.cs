using Everglow.Yggdrasil.WorldGeneration;
using Everglow.Yggdrasil.YggdrasilTown.Tiles;
using SubworldLibrary;

namespace Everglow.Yggdrasil.YggdrasilTown;

public class YggdrasilTownBiome : ModBiome
{
	public override int Music => !Main.dayTime ? Common.YggdrasilContent.QuickMusic("NewYggdrasilTownBGM") : Common.YggdrasilContent.QuickMusic("YggdrasilTownBGM");

	public override SceneEffectPriority Priority => SceneEffectPriority.Environment;

	public override string BestiaryIcon => "Everglow/Yggdrasil/YggdrasilTown/YggdrasilTownIcon";

	public override string BackgroundPath => "Everglow/Yggdrasil/YggdrasilTown/Backgrounds/YggdrasilTown_MapBackground";

	public override string MapBackground => "Everglow/Yggdrasil/YggdrasilTown/Backgrounds/YggdrasilTown_MapBackground";

	public override ModWaterStyle WaterStyle => ModContent.GetInstance<Water.YggdrasilTownWaterStyle>();

	public override Color? BackgroundColor => base.BackgroundColor;

	public override void Load()
	{
		base.Load();
	}

	public override bool IsBiomeActive(Player player)
	{
		return BiomeActive();
	}

	public static bool CheckedBiomeCenter = false;

	public static Vector2 BiomeCenter = Vector2.zeroVector;

	/// <summary>
	/// 地形中心
	/// </summary>
	public static Vector2 GetBiomeCenter()
	{
		if(!SubworldSystem.IsActive<YggdrasilWorld>())
		{
			CheckedBiomeCenter = false;
			return Vector2.zeroVector;
		}
		if (!CheckedBiomeCenter)
		{
			for (int x = 20; x < Main.maxTilesX - 20; x++)
			{
				for (int y = Main.maxTilesY - 2000; y < Main.maxTilesY - 20; y++)
				{
					Tile tile = YggdrasilWorldGeneration.SafeGetTile(x, y);
					if (tile.TileType == ModContent.TileType<YggdrasilCommonBlock>())
					{
						if (tile.TileFrameX == 0)
						{
							if (tile.TileFrameY == 0)
							{
								CheckedBiomeCenter = true;
								return new Point(x, y).ToWorldCoordinates();
							}
						}
					}
				}
			}
			CheckedBiomeCenter = true;
		}
		else if (BiomeCenter == Vector2.zeroVector)
		{
			return new Vector2(Main.maxTilesX / 2f * 16, (Main.maxTilesY - 1000) * 16);
		}
		return new Vector2(Main.maxTilesX / 2f * 16, (Main.maxTilesY - 1000) * 16);
	}

	/// <summary>
	/// 判定是否开启地形
	/// </summary>
	/// <returns></returns>
	public static bool BiomeActive()
	{
		if (BiomeCenter == Vector2.zeroVector)
		{
			BiomeCenter = GetBiomeCenter();
		}
		if (Main.screenPosition.Y > (BiomeCenter.Y - 18000))
		{
			if (SubworldSystem.IsActive<YggdrasilWorld>())
			{
				return true;
			}
		}
		return false;
	}

	public override void OnInBiome(Player player)
	{
		Main.raining = false;
		base.OnInBiome(player);
	}
}