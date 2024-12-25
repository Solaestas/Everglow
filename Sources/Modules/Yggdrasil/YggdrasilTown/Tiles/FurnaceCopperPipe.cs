namespace Everglow.Yggdrasil.YggdrasilTown.Tiles;

public class FurnaceCopperPipe : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileSolid[Type] = false;
		Main.tilePile[Type] = true;
		;
		Main.tileMergeDirt[Type] = false;
		Main.tileNoAttach[Type] = true;
		Main.tileBlendAll[Type] = false;
		Main.tileBlockLight[Type] = false;
		DustType = DustID.Copper;
		HitSound = default;

		AddMapEntry(new Color(132, 84, 58));
	}

	public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak) => base.TileFrame(i, j, ref resetFrame, ref noBreak);
}