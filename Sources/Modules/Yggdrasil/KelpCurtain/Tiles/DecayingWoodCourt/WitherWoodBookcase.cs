using Terraria.DataStructures;
using Terraria.ObjectData;
using Everglow.Yggdrasil.KelpCurtain.Dusts;

namespace Everglow.Yggdrasil.KelpCurtain.Tiles.DecayingWoodCourt;

public class WitherWoodBookcase : ModTile
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

		DustType = ModContent.DustType<WitherWoodDust>(); // You should set a kind of dust manually.
		AdjTiles = new int[] { TileID.Bookcases };

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
		TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16 };
		TileObjectData.newTile.CoordinatePaddingFix = new Point16(0, -2);
		TileObjectData.addTile(Type);

		AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
	}
}