namespace Everglow.Myth.TheTusk.Tiles;

public class BloodMossStone : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileMergeDirt[Type] = false;
		Main.tileBlockLight[Type] = true;
		MinPick = 200;
		DustType = DustID.Blood;
		HitSound = SoundID.Pixie;
		AddMapEntry(new Color(62, 61, 84));
	}
}