namespace Everglow.Minortopography.GiantPinetree.TilesAndWalls;

public class PineWoodWall : ModWall
{
	public override void SetStaticDefaults()
	{
		Main.wallHouse[Type] = true;
		DustType = DustID.WoodFurniture;
		AddMapEntry(new Color(24, 10, 10));
	}
}
