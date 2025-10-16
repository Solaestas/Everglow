using Everglow.Yggdrasil.KelpCurtain.Dusts;

namespace Everglow.Yggdrasil.KelpCurtain.Walls;

public class MossProneSandSoilWall : ModWall
{
	public override void SetStaticDefaults()
	{
		Main.wallHouse[Type] = true;
		DustType = ModContent.DustType<MossProneSandSoilDust>();
		AddMapEntry(new Color(25, 14, 12));
	}
}