using Everglow.Yggdrasil.YggdrasilTown.Dusts.TwilightForest;
using Terraria.Localization;
using Terraria.ObjectData;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles.TwilightForest.Furnitures;

public class TwilightEucalyptusClock : ModTile
{
	public override void SetStaticDefaults()
	{
		// Properties
		Main.tileFrameImportant[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileLavaDeath[Type] = true;
		TileID.Sets.Clock[Type] = true;

		DustType = ModContent.DustType<TwilightEucalyptusWoodDust>(); // You should set a kind of dust manually.
		AdjTiles = new int[] { TileID.GrandfatherClocks };

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
		TileObjectData.newTile.Height = 5;
		TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16, 16 };
		TileObjectData.addTile(Type);

		// Etc
		AddMapEntry(new Color(200, 200, 200), Language.GetText("ItemName.GrandfatherClock"));
	}

	public override bool RightClick(int x, int y)
	{
		return FurnitureUtils.ClockRightClick();
	}

	public override void NumDust(int i, int j, bool fail, ref int num)
	{
		num = fail ? 1 : 3;
	}
}