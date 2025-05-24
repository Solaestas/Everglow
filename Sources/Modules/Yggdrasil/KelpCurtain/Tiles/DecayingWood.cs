using Everglow.Yggdrasil.KelpCurtain.Dusts;

namespace Everglow.Yggdrasil.KelpCurtain.Tiles;

public class DecayingWood : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileMergeDirt[Type] = true;
		Main.tileBlockLight[Type] = true;
		DustType = ModContent.DustType<DecayingWoodDust>();
		AddMapEntry(new Color(158, 135, 105));
	}
}