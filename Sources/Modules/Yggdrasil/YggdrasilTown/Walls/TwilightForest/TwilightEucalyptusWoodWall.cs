using Everglow.Yggdrasil.YggdrasilTown.Dusts.TwilightForest;

namespace Everglow.Yggdrasil.YggdrasilTown.Walls.TwilightForest;

public class TwilightEucalyptusWoodWall : ModWall
{
	public override void SetStaticDefaults()
	{
		Main.wallHouse[Type] = true;
		DustType = ModContent.DustType<TwilightEucalyptusWoodWallDust>();
		AddMapEntry(new Color(111, 106, 96));
	}
}