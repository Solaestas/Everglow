using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using Terraria.DataStructures;
using Terraria.ObjectData;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles.FurnaceTiles;

public class HeatproofBookcase : ModTile
{
	public override void SetStaticDefaults()
	{
		// Properties
		Main.tileTable[Type] = true;
		Main.tileSolidTop[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileLavaDeath[Type] = true;
		Main.tileFrameImportant[Type] = true;
		TileID.Sets.DisableSmartCursor[Type] = true;
		TileID.Sets.IgnoredByNpcStepUp[Type] = true; // This line makes NPCs not try to step up this tile during their movement. Only use this for furniture with solid tops.

		DustType = ModContent.DustType<Heatproof_Furniture_Dust>(); // You should set a kind of dust manually.
		AdjTiles = new int[] { TileID.Bookcases };

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
		TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 18 };
		TileObjectData.newTile.CoordinatePaddingFix = new Point16(0, -2);
		TileObjectData.addTile(Type);

		AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
	}
	public override void NumDust(int i, int j, bool fail, ref int num)
	{
		num = 0;
	}
}
