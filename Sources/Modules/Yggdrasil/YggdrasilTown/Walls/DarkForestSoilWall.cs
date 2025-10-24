namespace Everglow.Yggdrasil.YggdrasilTown.Walls;

public class DarkForestSoilWall : ModWall
{
	public override void SetStaticDefaults()
	{
		Main.wallHouse[Type] = true;
		DustType = DustID.WoodFurniture;
		AddMapEntry(new Color(34, 24, 32));
	}

	public override bool CanExplode(int i, int j)
	{
		return false;
	}
}