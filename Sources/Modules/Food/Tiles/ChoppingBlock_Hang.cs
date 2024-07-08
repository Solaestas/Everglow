using Terraria.ObjectData;

namespace Everglow.Food.Tiles;

public class ChoppingBlock_Hang : ModTile
{
	public override void SetStaticDefaults()
	{
		// Properties
		Main.tileFrameImportant[Type] = true;
		Main.tileLavaDeath[Type] = true;
		TileID.Sets.DisableSmartCursor[Type] = true;

		DustType = DustID.Pearlwood;

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
		TileObjectData.newTile.Height = 3;
		TileObjectData.newTile.Width = 2;
		TileObjectData.newTile.CoordinateHeights = new int[]
		{
			16,
			16,
			16,
		};
		TileObjectData.addTile(Type);
		AddMapEntry(new Color(226, 196, 158));
	}
}