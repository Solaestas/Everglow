using Terraria.IO;
using Terraria.ObjectData;

namespace Everglow.SubSpace.Tiles;
public abstract class RoomDoorTile : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileLavaDeath[Type] = false;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
		TileObjectData.newTile.Height = 4;
		TileObjectData.newTile.Width = 5;
		TileObjectData.newTile.CoordinateHeights = new int[]
		{
			16,
			16,
			16,
			16
		};
		TileObjectData.addTile(Type);
		AddMapEntry(new Color(86, 62, 44));
		base.SetStaticDefaults();
	}
	public override bool RightClick(int i, int j)
	{
		Tile tile = Main.tile[i, j];
		Point point = new Point(i - tile.TileFrameX / 18, j - tile.TileFrameY / 18);
		RoomManager.EnterNextLevelRoom(point, "SubSpace/RoomMapIO/WoodenRoomMapIO_0.mapio");
		return base.RightClick(i, j);
	}
	public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
	{
		base.KillTile(i, j, ref fail, ref effectOnly, ref noItem);
	}
}
