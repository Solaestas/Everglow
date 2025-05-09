using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.Localization;
using Terraria.ObjectData;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles;

public class LargeCrystalShowTable_Blue : ModTile
{
	public override void SetStaticDefaults()
	{
		// Properties
		Main.tileFrameImportant[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileLavaDeath[Type] = true;

		DustType = DustID.Gold; // You should set a kind of dust manually.
		AdjTiles = new int[] { TileID.Chairs };

		// Names
		AddMapEntry(new Color(186, 98, 9));

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
		TileObjectData.newTile.Height = 3;
		TileObjectData.newTile.Width = 2;
		TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, };
		TileObjectData.newTile.CoordinatePaddingFix = new Point16(0, 2);
		TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;

		// The following 3 lines are needed if you decide to add more styles and stack them vertically
		TileObjectData.newTile.StyleWrapLimit = 2;
		TileObjectData.newTile.StyleMultiplier = 2;
		TileObjectData.newTile.StyleHorizontal = true;

		TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
		TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
		TileObjectData.addAlternate(1); // Facing right will use the second texture style
		TileObjectData.addTile(Type);
	}

	public override void NumDust(int i, int j, bool fail, ref int num)
	{
		num = fail ? 1 : 3;
	}

	public override bool CreateDust(int i, int j, ref int type) => base.CreateDust(i, j, ref type);

	public override bool RightClick(int i, int j) => base.RightClick(i, j);
}