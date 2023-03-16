namespace Everglow.Minortopography.GiantPinetree.TilesAndWalls;

public class PineWoodWall : ModWall
{
	public override void SetStaticDefaults()
	{
		Main.wallHouse[Type] = true;
		DustType = DustID.WoodFurniture;
		ModTranslation modTranslation = base.CreateMapEntryName(null);
		modTranslation.SetDefault("");
		AddMapEntry(new Color(24, 10, 10));
	}
	public override bool CanExplode(int i, int j)
	{
		return false;
	}
}
