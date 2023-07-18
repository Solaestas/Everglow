using Terraria.ObjectData;

namespace Everglow.Example.TileLayers;

public class WoodenRoomDoorItem : LayerDeeperTriggerTile
{
	public override void SSD()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileLavaDeath[Type] = false;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
		TileObjectData.newTile.Height = 4;
		TileObjectData.newTile.Width = 4;
		TileObjectData.newTile.CoordinateHeights = new int[]
		{
			16,
			16,
			16,
			16
		};
		TileObjectData.addTile(Type);
		AddMapEntry(new Color(15, 11, 9));
	}
}