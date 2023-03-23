using Terraria.ObjectData;

namespace Everglow.TwilightForest.Tiles;

public class TwilightCrystal_Flat : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileLavaDeath[Type] = false;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
		TileObjectData.newTile.Height = 1;
		TileObjectData.newTile.Width = 3;
		TileObjectData.newTile.CoordinateHeights = new int[]
		{
			18
		};
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.addTile(Type);

		AddMapEntry(new Color(45, 59, 168));
	}
}