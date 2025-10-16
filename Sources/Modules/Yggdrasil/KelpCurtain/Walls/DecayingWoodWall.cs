using Everglow.Yggdrasil.KelpCurtain.Dusts;

namespace Everglow.Yggdrasil.KelpCurtain.Walls;

public class DecayingWoodWall : ModWall
{
	public override void SetStaticDefaults()
	{
		Main.wallHouse[Type] = true;
		DustType = ModContent.DustType<DecayingWoodDust>();
		AddMapEntry(new Color(64, 51, 33));
	}
}