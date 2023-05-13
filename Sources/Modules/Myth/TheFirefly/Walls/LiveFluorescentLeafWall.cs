using Everglow.Myth.TheFirefly.Dusts;

namespace Everglow.Myth.TheFirefly.Walls;

public class LiveFluorescentLeafWall : ModWall
{
	public override void SetStaticDefaults()
	{
		Main.wallHouse[Type] = true;
		DustType = ModContent.DustType<FluorescentTreeDust>();
				HitSound = SoundID.Dig;
		AddMapEntry(new Color(8, 22, 48));
	}
}