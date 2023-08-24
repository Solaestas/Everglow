namespace Everglow.Minortopography.GiantPinetree.TilesAndWalls;

public class PineLeavesWall : ModWall
{
	public override void SetStaticDefaults()
	{
		Main.wallHouse[Type] = true;
		DustType = DustID.GreenMoss;
		HitSound = SoundID.Grass;
		AddMapEntry(new Color(0, 32, 22));
	}
}
