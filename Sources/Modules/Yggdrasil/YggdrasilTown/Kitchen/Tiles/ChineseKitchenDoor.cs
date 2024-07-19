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
		PlaceFrameImportantTiles(148, 156, 5, 4, ModContent.TileType<WoodenRoomDoor_exit>(), 0, 0);
	}

	public override bool RightClick(int i, int j)
	{
		Tile tile = Main.tile[i, j];
		Point point = new Point(i - tile.TileFrameX / 18, j - tile.TileFrameY / 18);
		RoomManager.EnterNextLevelRoom(point, BuildChineseStyleKitchenGen);
		return false;
	}
}