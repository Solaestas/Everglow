namespace Everglow.Minortopography.GiantPinetree.TilesAndWalls;

public class PineLeaves : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileBlockLight[Type] = true;
		Main.tileMerge[Type][TileID.PineTree] = true;
		Main.tileMerge[TileID.PineTree][Type] = true;
		DustType = DustID.GreenMoss;
		HitSound = SoundID.Grass;
		AddMapEntry(new Color(36, 64, 50));
	}
}
