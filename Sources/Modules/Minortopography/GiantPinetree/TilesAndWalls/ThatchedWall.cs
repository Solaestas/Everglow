namespace Everglow.Minortopography.GiantPinetree.TilesAndWalls;

public class ThatchedWall : ModWall
{
	public override void SetStaticDefaults()
	{
		Main.wallHouse[Type] = true;
		DustType = ModContent.DustType<Dusts.ThatchedDust>();
		HitSound = SoundID.Grass;
		AddMapEntry(new Color(60, 33, 16));
	}
}
