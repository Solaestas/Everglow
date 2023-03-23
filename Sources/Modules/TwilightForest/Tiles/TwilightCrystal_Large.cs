using Terraria.ObjectData;

namespace Everglow.TwilightForest.Tiles;

public class TwilightCrystal_Large : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileLavaDeath[Type] = false;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
		TileObjectData.newTile.Height = 3;
		TileObjectData.newTile.Width = 3;
		TileObjectData.newTile.CoordinateHeights = new int[]
		{
			16,
			16,
			18
		};
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.addTile(Type);

		AddMapEntry(new Color(45, 59, 168));
	}
}