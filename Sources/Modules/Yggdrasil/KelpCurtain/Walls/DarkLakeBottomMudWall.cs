using Everglow.Yggdrasil.KelpCurtain.Dusts;

namespace Everglow.Yggdrasil.KelpCurtain.Walls;

public class DarkLakeBottomMudWall : ModWall
{
	public override void SetStaticDefaults()
	{
		Main.wallHouse[Type] = true;
		DustType = DustType = ModContent.DustType<DarkLakeBottomMudDust>();

		AddMapEntry(new Color(18, 29, 26));
	}
}