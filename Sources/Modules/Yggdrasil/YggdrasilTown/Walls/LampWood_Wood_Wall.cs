namespace Everglow.Yggdrasil.YggdrasilTown.Walls;

public class LampWood_Wood_Wall : ModWall
{
	public override void SetStaticDefaults()
	{
		Main.wallHouse[Type] = true;
		DustType = DustID.WoodFurniture;
		AddMapEntry(new Color(27, 38, 31));
	}

	public override bool CanExplode(int i, int j)
	{
		return false;
	}
}