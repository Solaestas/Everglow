namespace Everglow.Myth.TheTusk.Walls;

public class BloodyStoneWall : ModWall
{
	public override void SetStaticDefaults()
	{
		Main.wallHouse[Type] = true;
		DustType = DustID.Stone;
		AddMapEntry(new Color(61, 13, 11));
	}
}