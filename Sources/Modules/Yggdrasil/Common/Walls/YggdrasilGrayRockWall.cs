using Everglow.Yggdrasil.Common.Dusts;

namespace Everglow.Yggdrasil.Common.Walls;

public class YggdrasilGrayRockWall : ModWall
{
	public override void SetStaticDefaults()
	{
		Main.wallHouse[Type] = true;
		DustType = ModContent.DustType<YggdrasilGrayRock_Dust>();
		AddMapEntry(new Color(34, 34, 34));
	}
}