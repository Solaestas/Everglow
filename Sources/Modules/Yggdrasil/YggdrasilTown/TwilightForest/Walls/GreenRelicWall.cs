using Everglow.Yggdrasil.YggdrasilTown.TwilightForest.Dusts;

namespace Everglow.Yggdrasil.YggdrasilTown.TwilightForest.Walls;

public class GreenRelicWall : ModWall
{
	public override void SetStaticDefaults()
	{
		Main.wallHouse[Type] = true;
		DustType = ModContent.DustType<GreenRelicBrick_dust>();
		AddMapEntry(new Color(10, 31, 31));
	}

	public override bool CanExplode(int i, int j)
	{
		return false;
	}
}