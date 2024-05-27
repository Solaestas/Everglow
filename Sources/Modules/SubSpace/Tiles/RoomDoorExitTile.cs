using Terraria.IO;
using Terraria.ObjectData;

namespace Everglow.SubSpace.Tiles;
public abstract class RoomDoorExitTile : ModTile
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
		RoomManager.ExitALevelOfRoom();
		return base.RightClick(i, j);
	}
}
