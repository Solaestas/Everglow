using Everglow.Commons.TileHelper;
using Everglow.SubSpace;
using Everglow.SubSpace.Tiles;
using Everglow.Yggdrasil.YggdrasilTown.Tiles;
using static Everglow.Yggdrasil.WorldGeneration.YggdrasilWorldGeneration;

namespace Everglow.Yggdrasil.YggdrasilTown.Kitchen.Tiles;

public class ChineseKitchenDoor : RoomDoorTile
{
	public void BuildChineseStyleKitchenGen()
	{
		WoodenBoxRoomGenPass.BuildWoodenSquareRoom();
		var mapIO = new MapIO(136, 120);

		mapIO.Read(ModIns.Mod.GetFileStream("Yggdrasil/YggdrasilTown/Kitchen/MapIOs/ChineseStyleKitchen.mapio"));

		var it = mapIO.GetEnumerator();
		while (it.MoveNext())
		{
			WorldGen.SquareTileFrame(it.CurrentCoord.X, it.CurrentCoord.Y);
			WorldGen.SquareWallFrame(it.CurrentCoord.X, it.CurrentCoord.Y);
		}
		PlaceFrameImportantTiles(156, 156, 5, 4, ModContent.TileType<WoodenRoomDoor_exit>(), 0, 0);
		PlaceFrameImportantTiles(146, 158, 2, 2, ModContent.TileType<FoodChest>(), 0, 0);
		PlaceFrameImportantTiles(144, 158, 2, 2, ModContent.TileType<FoodChest>(), 36, 0);
		PlaceFrameImportantTiles(142, 158, 2, 2, ModContent.TileType<FoodChest>(), 72, 0);
		PlaceFrameImportantTiles(146, 156, 2, 2, ModContent.TileType<FoodChest>(), 108, 0);
		PlaceFrameImportantTiles(144, 156, 2, 2, ModContent.TileType<FoodChest>(), 144, 0);
		PlaceFrameImportantTiles(142, 156, 2, 2, ModContent.TileType<FoodChest>(), 180, 0);
		PlaceFrameImportantTiles(146, 154, 2, 2, ModContent.TileType<FoodChest>(), 216, 0);
		PlaceFrameImportantTiles(144, 154, 2, 2, ModContent.TileType<FoodChest>(), 252, 0);
		PlaceFrameImportantTiles(142, 154, 2, 2, ModContent.TileType<FoodChest>(), 288, 0);
		PlaceFrameImportantTiles(146, 152, 2, 2, ModContent.TileType<FoodChest>(), 324, 0);
		PlaceFrameImportantTiles(144, 152, 2, 2, ModContent.TileType<FoodChest>(), 360, 0);
	}

	public override bool RightClick(int i, int j)
	{
		Tile tile = Main.tile[i, j];
		Point point = new Point(i - tile.TileFrameX / 18, j - tile.TileFrameY / 18);
		RoomManager.EnterNextLevelRoom(point, BuildChineseStyleKitchenGen);
		return false;
	}
}