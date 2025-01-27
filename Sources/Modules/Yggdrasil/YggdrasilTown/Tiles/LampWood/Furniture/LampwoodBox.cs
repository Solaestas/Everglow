using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using Terraria.Localization;
using Terraria.ObjectData;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles.LampWood.Furniture;

public class LampwoodBox : ModTile
{
	public override void SetStaticDefaults()
	{
		DustType = ModContent.DustType<LampWood_Dust>();
		// Properties
		Main.tileFrameImportant[Type] = true;
		Main.tileSolidTop[Type] = true;
		Main.tileTable[Type] = true;

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
		TileObjectData.newTile.CoordinateHeights = new int[2] { 16, 18 };
		TileObjectData.newTile.StyleHorizontal = true; // Optional, if you add more placeStyles for the item 
		TileObjectData.addTile(Type);

		// Etc
		LocalizedText name = CreateMapEntryName();
		AdjTiles = new int[] { TileID.Tables };		AddMapEntry(new Color(191, 142, 111), name);
	}

	public override void NumDust(int i, int j, bool fail, ref int num)
	{
		num = fail ? 1 : 3;
	}
}