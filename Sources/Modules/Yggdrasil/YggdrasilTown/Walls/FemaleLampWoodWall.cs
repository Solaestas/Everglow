namespace Everglow.Yggdrasil.YggdrasilTown.Walls;

public class FemaleLampWoodWall : ModWall
{
	public override void SetStaticDefaults()
	{
		Main.wallHouse[Type] = true;
		DustType = DustID.WoodFurniture;
		AddMapEntry(new Color(68, 56, 50));
	}
	public override bool CanExplode(int i, int j)
	{
		return false;
	}
}
