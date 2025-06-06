using Everglow.Yggdrasil.KelpCurtain.Dusts;
using Everglow.Yggdrasil.WorldGeneration;

namespace Everglow.Yggdrasil.KelpCurtain.Tiles;

public class DarkLakeBottomMud : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileMergeDirt[Type] = true;
		Main.tileBlockLight[Type] = true;

		DustType = DustType = ModContent.DustType<DarkLakeBottomMudDust>();
		MinPick = 50;
		HitSound = SoundID.Dig;
		AddMapEntry(new Color(22, 33, 30));
	}
}