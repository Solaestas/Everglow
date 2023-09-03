using Everglow.Myth.TheFirefly.Dusts;

namespace Everglow.Myth.TheFirefly.Walls;

public class LiveFluorescentWoodWall : ModWall
{
	public override void SetStaticDefaults()
	{
		Main.wallHouse[Type] = true;
		DustType = ModContent.DustType<FluorescentTreeDust>();
				HitSound = SoundID.Dig;
		AddMapEntry(new Color(33, 1, 53));
	}
}