using Everglow.Yggdrasil.WorldGeneration;
using Everglow.Yggdrasil.YggdrasilTown.Tiles;
using SubworldLibrary;

namespace Everglow.Yggdrasil.YggdrasilTown.Biomes;

public class YggdrasilTownBiome : ModBiome
{
	public override int Music => GetMusic();

	public int GetMusic()
	{
		if (Mod != null)
		{
			if (YggdrasilTownCentralSystem.InArena_YggdrasilTown())
			{
				return MusicLoader.GetMusicSlot(Mod, ModAsset.Arena_BGM_Path);
			}
			if (YggdrasilTownCentralSystem.InFurnace_YggdrasilTown())
			{
				return MusicLoader.GetMusicSlot(Mod, ModAsset.FurnaceArea_BGM_Path);
			}
			return !Main.dayTime ? MusicLoader.GetMusicSlot(Mod, ModAsset.NewYggdrasilTownBGM_Path) : MusicLoader.GetMusicSlot(Mod, ModAsset.YggdrasilTownBGM_Path);
		}
		else
		{
			return 0;
		}
	}

	public override SceneEffectPriority Priority => SceneEffectPriority.Environment;

	public override string BestiaryIcon => ModAsset.YggdrasilTownIcon_Mod;

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
					Tile tile = TileUtils.SafeGetTile(x, y);
					if (tile.TileType == ModContent.TileType<YggdrasilCommandBlock>())
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
			return new Vector2(487, 20711).ToWorldCoordinates();
		}
		return new Vector2(487, 20711).ToWorldCoordinates();
	}

	/// <summary>
	/// 判定是否开启地形
	/// </summary>
	/// <returns></returns>
	public static bool BiomeActive()
	{
		if(YggdrasilTownCentralSystem.InCanteen_YggdrasilTown())
		{
			return true;
		}
		if (YggdrasilTownCentralSystem.InUnion_YggdrasilTown())
		{
			return true;
		}
		if (YggdrasilTownCentralSystem.InPlayerRoom_YggdrasilTown())
		{
			return true;
		}
		if (YggdrasilTownCentralSystem.InArena_YggdrasilTown())
		{
			return true;
		}
		if (BiomeCenter == Vector2.zeroVector)
		{
			BiomeCenter = GetBiomeCenter();
		}
		if (Main.screenPosition.Y > BiomeCenter.Y - 18000)
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