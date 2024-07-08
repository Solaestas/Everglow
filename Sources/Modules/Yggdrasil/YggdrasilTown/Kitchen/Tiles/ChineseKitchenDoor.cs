using Everglow.SubSpace;
using Everglow.SubSpace.Tiles;

namespace Everglow.Yggdrasil.YggdrasilTown.Kitchen.Tiles;

public class ChineseKitchenDoor : RoomDoorTile
{
	public override bool RightClick(int i, int j)
	{
		Tile tile = Main.tile[i, j];
		Point point = new Point(i - tile.TileFrameX / 18, j - tile.TileFrameY / 18);
		RoomManager.EnterNextLevelRoom(point, "SubSpace/RoomMapIO/WoodenRoomMapIO_0.mapio");
		return base.RightClick(i, j);
	}
}