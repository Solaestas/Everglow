using Everglow.Yggdrasil.KelpCurtain.Dusts;

namespace Everglow.Yggdrasil.KelpCurtain.Walls;

public class OldMossWall : ModWall
{
	public override void SetStaticDefaults()
	{
		Main.wallHouse[Type] = true;
		DustType = DustType = ModContent.DustType<OldMossDust>();

		AddMapEntry(new Color(36, 56, 23));
	}
}