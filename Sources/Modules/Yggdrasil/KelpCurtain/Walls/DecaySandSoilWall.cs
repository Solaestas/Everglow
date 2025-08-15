using Everglow.Yggdrasil.KelpCurtain.Dusts;

namespace Everglow.Yggdrasil.KelpCurtain.Walls;

public class DecaySandSoilWall : ModWall
{
	public override void SetStaticDefaults()
	{
		Main.wallHouse[Type] = true;
		DustType = DustType = ModContent.DustType<DecaySandSoil_Dust>();

		AddMapEntry(new Color(42, 64, 49));
	}
}