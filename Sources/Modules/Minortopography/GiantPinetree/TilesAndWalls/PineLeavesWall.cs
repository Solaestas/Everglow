namespace Everglow.Minortopography.GiantPinetree.TilesAndWalls;

public class PineLeavesWall : ModWall
{
	public override void SetStaticDefaults()
	{
		Main.wallHouse[Type] = true;
		DustType = DustID.GreenMoss;
		ModTranslation modTranslation = base.CreateMapEntryName(null);
		modTranslation.SetDefault("");
		AddMapEntry(new Color(0, 32, 22));
	}
	public override bool CanExplode(int i, int j)
	{
		return false;
	}
}
