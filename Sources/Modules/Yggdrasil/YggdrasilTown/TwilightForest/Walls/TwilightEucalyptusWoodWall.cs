using Everglow.Yggdrasil.YggdrasilTown.TwilightForest.Dusts;

namespace Everglow.Yggdrasil.YggdrasilTown.TwilightForest.Walls;

public class TwilightEucalyptusWoodWall : ModWall
{
	public override void SetStaticDefaults()
	{
		Main.wallHouse[Type] = true;
		DustType = ModContent.DustType<TwilightEucalyptusWoodWallDust>();
		AddMapEntry(new Color(111, 106, 96));
	}
}