using Everglow.Yggdrasil.KelpCurtain.Dusts;

namespace Everglow.Yggdrasil.KelpCurtain.Walls;

public class WitherWoodShutterWall : ModWall
{
	public override void SetStaticDefaults()
	{
		Main.wallHouse[Type] = true;
		DustType = DustType = ModContent.DustType<WitherWoodDust>();

		AddMapEntry(new Color(63, 44, 38));
	}
}