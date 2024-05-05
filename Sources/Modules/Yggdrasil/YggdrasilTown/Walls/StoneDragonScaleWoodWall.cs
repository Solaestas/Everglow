namespace Everglow.Yggdrasil.YggdrasilTown.Walls;

public class StoneDragonScaleWoodWall : ModWall
{
	public override void SetStaticDefaults()
	{
		Main.wallHouse[Type] = true;
		DustType = DustID.WoodFurniture;
		AddMapEntry(new Color(10, 8, 8));
	}
	public override bool CanExplode(int i, int j)
	{
		return false;
	}
}
