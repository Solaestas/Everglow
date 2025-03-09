using Everglow.Commons.TileHelper;
using Everglow.SubSpace;
using Everglow.SubSpace.Tiles;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles;

public class YggdrasilPlayerRoomDoor : RoomDoorTile
{
	public void BuildCanteenGen()
	{
		var mapIO = new MapIO(100, 110);

		mapIO.Read(ModIns.Mod.GetFileStream(ModAsset.MapIOs_107x60PlayerHouse_Path));

		var it = mapIO.GetEnumerator();
		while (it.MoveNext())
		{
			WorldGen.SquareTileFrame(it.CurrentCoord.X, it.CurrentCoord.Y);
			WorldGen.SquareWallFrame(it.CurrentCoord.X, it.CurrentCoord.Y);
		}
	}

	public override bool RightClick(int i, int j)
	{
		Tile tile = Main.tile[i, j];
		var point = new Point(i - tile.TileFrameX / 18, j - tile.TileFrameY / 18);
		RoomManager.EnterNextLevelRoom(point, default, default, default, new Point(150, 150), BuildCanteenGen);
		return false;
	}
}