using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.Localization;
using Terraria.ObjectData;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles.FurnaceTiles;

public class HeatproofClock : ModTile
{
	public override void SetStaticDefaults() {
		// Properties
		Main.tileFrameImportant[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileLavaDeath[Type] = true;
		TileID.Sets.Clock[Type] = true;

		DustType = ModContent.DustType<Heatproof_Furniture_Dust>(); // You should set a kind of dust manually.
		AdjTiles = new int[] { TileID.GrandfatherClocks };

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
		TileObjectData.newTile.Height = 5;
		TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16, 16 };
		TileObjectData.addTile(Type);

		// Etc
		AddMapEntry(new Color(200, 200, 200), Language.GetText("ItemName.GrandfatherClock"));
	}

	public override bool RightClick(int x, int y) {
		return FurnitureUtils.ClockRightClick();
	}

	public override void NumDust(int i, int j, bool fail, ref int num) {
		num = fail ? 1 : 3;
	}
}
