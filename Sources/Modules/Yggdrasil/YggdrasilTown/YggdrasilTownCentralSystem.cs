using Everglow.SubSpace;
using Everglow.Yggdrasil.WorldGeneration;
using Everglow.Yggdrasil.YggdrasilTown.Kitchen.Tiles;
using Everglow.Yggdrasil.YggdrasilTown.NPCs.TownNPCs;
using Everglow.Yggdrasil.YggdrasilTown.Tiles;
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
		if (SubworldSystem.Current is YggdrasilWorld)
		{
			CheckNPC(ModContent.NPCType<Guard_of_YggdrasilTown>());
			CheckNPC(ModContent.NPCType<TeahouseLady>());
		}
		if (InCanteen_YggdrasilTown())
		{
			CheckNPC(ModContent.NPCType<CanteenMaid>());
		}
		base.OnWorldLoad();
	}

	public override void PostUpdateNPCs()
	{
		if(Main.time == 0)
		{
			if (SubworldSystem.Current is YggdrasilWorld)
			{
				CheckNPC(ModContent.NPCType<Guard_of_YggdrasilTown>());
				CheckNPC(ModContent.NPCType<TeahouseLady>());
				CheckNPC(ModContent.NPCType<InnKeeper>());
			}
			if (InCanteen_YggdrasilTown())
			{
				CheckNPC(ModContent.NPCType<CanteenMaid>());
				CheckNPC(ModContent.NPCType<Resturateur>());
			}
		}
		base.PostUpdateNPCs();
	}

	public static void CheckNPC(int type)
	{
		if (type > 0)
		{
			if (NPC.CountNPCS(type) <= 0)
			{
				Point spawnPos = YggdrasilTownBiome.BiomeCenter.ToTileCoordinates();
				NPC.NewNPC(WorldGen.GetNPCSource_TileBreak(spawnPos.X, spawnPos.Y), spawnPos.X, spawnPos.Y, type);
			}
			else if (NPC.CountNPCS(type) >= 2)
			{
				foreach (NPC npc in Main.npc)
				{
					if(npc != null && npc.type == type && npc.active)
					{
						npc.active = false;
					}
					if(NPC.CountNPCS(type) <= 1)
					{
						break;
					}
				}
			}
		}
	}

	public static bool InYggdrasilTown(Vector2 worldCoordiante)
	{
		return InYggdrasilTown(worldCoordiante.ToTileCoordinates());
	}

	public static bool InYggdrasilTown(Point tileCoordiante)
	{
		if(InCanteen_YggdrasilTown() || InUnion_YggdrasilTown() || InPlayerRoom_YggdrasilTown())
		{
			return true;
		}
		if (SubworldSystem.Current is YggdrasilWorld)
		{
			return tileCoordiante.X >= TownArea.X && tileCoordiante.X <= TownArea.X + TownArea.Width && tileCoordiante.Y >= TownArea.Y && tileCoordiante.Y <= TownArea.Y + TownArea.Height;
		}
		return false;
	}

	public static bool InCanteen_YggdrasilTown()
	{
		if (SubworldSystem.Current is RoomWorld)
		{
			return YggdrasilWorldGeneration.SafeGetTile(20, 20).TileType == ModContent.TileType<CanteenCommandBlock>();
		}
		return false;
	}

	public static bool InUnion_YggdrasilTown()
	{
		if (SubworldSystem.Current is RoomWorld)
		{
			return YggdrasilWorldGeneration.SafeGetTile(20, 20).TileType == ModContent.TileType<UnionCommandBlock>();
		}
		return false;
	}

	public static bool InPlayerRoom_YggdrasilTown()
	{
		if (SubworldSystem.Current is RoomWorld)
		{
			return YggdrasilWorldGeneration.SafeGetTile(20, 20).TileType == ModContent.TileType<PlayerRoomCommandBlock>();
		}
		return false;
	}

	public static Vector2 GetTownCoord(Vector2 worldCoordiante)
	{
		return worldCoordiante - new Point(430, Main.maxTilesY - 400).ToWorldCoordinates();
	}
}