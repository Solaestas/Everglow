namespace Everglow.Yggdrasil.KelpCurtain.Tiles;

public class DragonScaleWood : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileBlockLight[Type] = true;
		Main.tileMerge[Type][ModContent.TileType<OldMoss>()] = true;
		Main.tileMerge[Type][ModContent.TileType<MossProneSandSoil>()] = true;
		Main.tileMerge[Type][TileID.Stone] = true;
		Main.tileMerge[TileID.Stone][Type] = true;
		Main.ugBackTransition = 1000;
		DustType = DustID.BorealWood;
		MinPick = 300;
		HitSound = SoundID.Dig;
		AddMapEntry(new Color(86, 62, 44));
	}
	public override bool CanExplode(int i, int j)
	{
		return false;
	}

}
