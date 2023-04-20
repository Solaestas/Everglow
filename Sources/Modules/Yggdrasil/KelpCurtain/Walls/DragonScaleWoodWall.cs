namespace Everglow.Yggdrasil.KelpCurtain.Walls;

public class DragonScaleWoodWall : ModWall
{
	public override void SetStaticDefaults()
	{
		Main.wallHouse[Type] = true;
		DustType = DustID.WoodFurniture;
		ItemDrop = ModContent.ItemType<Items.DragonScaleWoodWall>();
		AddMapEntry(new Color(32, 8, 0));
	}
	public override bool CanExplode(int i, int j)
	{
		return false;
	}
}
