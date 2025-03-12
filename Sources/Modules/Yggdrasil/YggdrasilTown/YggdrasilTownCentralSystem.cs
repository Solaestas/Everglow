using Everglow.Yggdrasil.YggdrasilTown.NPCs.TownNPCs;
using SubworldLibrary;

namespace Everglow.Yggdrasil.YggdrasilTown;

public class YggdrasilTownCentralSystem : ModSystem
{
	public static Rectangle TownArea => new Rectangle(TownTopLeftWorldCoord.ToTileCoordinates().X, TownTopLeftWorldCoord.ToTileCoordinates().Y, 706, 275);

	public static Vector2 TownTopLeftWorldCoord => YggdrasilTownBiome.BiomeCenter - new Vector2(257, 191) * 16;

	public float TownSurfaceWorldCoordY => TownTopLeftWorldCoord.Y + 1600;

	public static Vector2 TownPos(Vector2 position_Orig)
	{
		return position_Orig - TownTopLeftWorldCoord;
	}

	public override void OnWorldLoad()
	{
		if(SubworldSystem.Current is YggdrasilWorld)
		{
			if (NPC.CountNPCS(ModContent.NPCType<Guard_of_YggdrasilTown>()) <= 0)
			{
				Point spawnPos = YggdrasilTownBiome.BiomeCenter.ToTileCoordinates();
				NPC.NewNPC(WorldGen.GetNPCSource_TileBreak(spawnPos.X, spawnPos.Y), spawnPos.X, spawnPos.Y, ModContent.NPCType<Guard_of_YggdrasilTown>());
			}
		}
		base.OnWorldLoad();
	}

	public static bool InYggdrasilTown(Vector2 worldCoordiante)
	{
		return InYggdrasilTown(worldCoordiante.ToTileCoordinates());
	}

	public static bool InYggdrasilTown(Point tileCoordiante)
	{
		if (SubworldSystem.Current is YggdrasilWorld)
		{
			return tileCoordiante.X >= TownArea.X && tileCoordiante.X <= TownArea.X + TownArea.Width && tileCoordiante.Y >= TownArea.Y && tileCoordiante.Y <= TownArea.Y + TownArea.Height;
		}
		return false;
	}

	public static Vector2 GetTownCoord(Vector2 worldCoordiante)
	{
		return worldCoordiante - new Point(430, Main.maxTilesY - 400).ToWorldCoordinates();
	}
}