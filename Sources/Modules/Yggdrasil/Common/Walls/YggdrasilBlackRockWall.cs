using Everglow.Yggdrasil.Common.Dusts;

namespace Everglow.Yggdrasil.Common.Walls;

public class YggdrasilBlackRockWall : ModWall
{
	public override void SetStaticDefaults()
	{
		Main.wallHouse[Type] = true;
		DustType = ModContent.DustType<YggdrasilBlackRock_Dust>();
		AddMapEntry(new Color(6, 6, 7));
	}
}