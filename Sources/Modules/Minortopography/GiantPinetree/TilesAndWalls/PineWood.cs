namespace Everglow.Minortopography.GiantPinetree.TilesAndWalls;

public class PineWood : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileBlockLight[Type] = true;
		Main.tileMerge[Type][TileID.PineTree] = true;
		Main.tileMerge[TileID.PineTree][Type] = true;
		DustType = DustID.BorealWood;
		HitSound = SoundID.Dig;
		ItemDrop = ItemID.BorealWood;
		AddMapEntry(new Color(60, 45, 39));
	}
}
