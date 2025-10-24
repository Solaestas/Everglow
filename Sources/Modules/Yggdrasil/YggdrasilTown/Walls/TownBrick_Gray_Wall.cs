using Everglow.Yggdrasil.YggdrasilTown.Dusts;

namespace Everglow.Yggdrasil.YggdrasilTown.Walls;

public class TownBrick_Gray_Wall : ModWall
{
	public override void SetStaticDefaults()
	{
		Main.wallHouse[Type] = true;
		DustType = ModContent.DustType<TownBrick_Khaki_Dust>();
		AddMapEntry(new Color(70, 76, 74));
	}
}