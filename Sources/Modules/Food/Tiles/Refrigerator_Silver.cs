using Terraria.ObjectData;

namespace Everglow.Food.Tiles
{
	public class Refrigerator_Silver : ModTile
	{
		public override void SetStaticDefaults()
		{
			// Properties
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = false;
			Main.tileLavaDeath[Type] = true;
			Main.tileSolidTop[Type] = true;
			DustType = DustID.Silver;

			// Placement
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 4;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16 };
			TileObjectData.addTile(Type);

			// Etc
			AddMapEntry(new Color(200, 200, 200));
		}
	}
}