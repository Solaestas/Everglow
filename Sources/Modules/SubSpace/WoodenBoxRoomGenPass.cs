using Everglow.Commons.TileHelper;
using Terraria.IO;
using Terraria.WorldBuilding;

namespace Everglow.SubSpace;

public class WoodenBoxRoomGenPass : GenPass
{
	public WoodenBoxRoomGenPass() : base("Wooden Box", 500)
	{
	}

	public override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
	{
		Main.statusText = "Test";
		BuildWoodenRoom();
	}
	/// <summary>
	/// 建造一个木制世界房间
	/// </summary>
	public static void BuildWoodenRoom()
	{
		//获取对应层级的世界材料
		GetLayerMaterial();
		//用木头填满世界
		for (int x = 20; x < Main.maxTilesX - 20; x++)
		{
			for (int y = 20; y < Main.maxTilesY - 20; y++)
			{
				Tile tile = Main.tile[x, y];
				tile.wall = (ushort)RoomWallType;
				if (!(Math.Abs(x - Main.maxTilesX / 2) < 10 && Math.Abs(y - Main.maxTilesY / 2) < 10))
				{
					tile.TileType = (ushort)RoomTileType;
					tile.HasTile = true;
				}
				else
				{
					tile.HasTile = false;
				}
			}
		}
		//读取保存记录
		ReadWorldSave();
	}
	public static bool ReadWorldSave()
	{
		string path = Path.Combine(Main.SavePath, "Mods", "ModDatas", "Everglow_RoomWorlds", Main.worldID.ToString());
		if (RoomWorld.OriginalWorld != null)
		{
			path = Path.Combine(Main.SavePath, "Mods", "ModDatas", "Everglow_RoomWorlds", Main.worldID.ToString(), RoomWorld.OriginalWorld.Name.ToString());
		}
		string readPath = path + "\\RoomMapIO" + RoomWorld.AnchorWorldCoordinate.X.ToString() + "_" + RoomWorld.AnchorWorldCoordinate.Y.ToString() + "_" + RoomWorld.LayerDepth + ".mapio";
		return QuickBuild(5, 5, readPath);
	}
	public static bool QuickBuild(int x, int y, string pathName)
	{
		var mapIO = new MapIO(x, y);

		if (!File.Exists(pathName))
		{
			return false;
		}

		mapIO.Read(pathName);

		var it = mapIO.GetEnumerator();
		while (it.MoveNext())
		{
			WorldGen.SquareTileFrame(it.CurrentCoord.X, it.CurrentCoord.Y);
			WorldGen.SquareWallFrame(it.CurrentCoord.X, it.CurrentCoord.Y);
		}
		return true;
	}
	public static void GetLayerMaterial()
	{
		switch (RoomWorld.LayerDepth)
		{
			case 1:
				RoomTileType = TileID.WoodBlock;
				RoomWallType = WallID.Wood;
				break;
			case 2:
				RoomTileType = TileID.RichMahogany;
				RoomWallType = WallID.RichMaogany;
				break;
			case 3:
				RoomTileType = TileID.PalmWood;
				RoomWallType = WallID.PalmWood;
				break;
			case 4:
				RoomTileType = TileID.Shadewood;
				RoomWallType = WallID.Shadewood;
				break;
			default:
				RoomTileType = TileID.BorealWood;
				RoomWallType = WallID.BorealWood;
				break;
		}
	}
	public static int RoomTileType;
	public static int RoomWallType;
}