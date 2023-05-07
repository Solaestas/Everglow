using Terraria.ObjectData;

namespace Everglow.Minortopography.GiantPinetree.TilesAndWalls;

public class PineCone : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileLavaDeath[Type] = true;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
		TileObjectData.newTile.CoordinateWidth = 16;
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.addTile(Type);
		AddMapEntry(new Color(119, 77, 63));
	}
}