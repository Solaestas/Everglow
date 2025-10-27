using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using Terraria.Localization;
using Terraria.ObjectData;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles.LampWood.Furniture;

public class LampWoodCrate : ModTile
{
	public override void SetStaticDefaults()
	{
		DustType = ModContent.DustType<LampWood_Dust>();

		Main.tileFrameImportant[Type] = true;
		Main.tileSolidTop[Type] = true;
		Main.tileTable[Type] = true;

		TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
		TileObjectData.newTile.CoordinateHeights = [16, 18];
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.addTile(Type);

		LocalizedText name = CreateMapEntryName();
		AdjTiles = [TileID.Tables];
		AddMapEntry(new Color(191, 142, 111), name);
	}

	public override void NumDust(int i, int j, bool fail, ref int num)
	{
		num = fail ? 1 : 3;
	}
}