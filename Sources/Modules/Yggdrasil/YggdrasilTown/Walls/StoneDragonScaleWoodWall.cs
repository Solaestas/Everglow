namespace Everglow.Yggdrasil.YggdrasilTown.Walls;

public class StoneDragonScaleWoodWall : ModWall
{
	public override void SetStaticDefaults()
	{
		Main.wallHouse[Type] = true;
		DustType = DustID.WoodFurniture;
		AddMapEntry(new Color(24, 0, 0));
	}
	public override bool CanExplode(int i, int j)
	{
		return false;
	}
}
