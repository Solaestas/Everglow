using Everglow.Yggdrasil.KelpCurtain.Dusts;

namespace Everglow.Yggdrasil.KelpCurtain.Tiles.DeathJadeLake;

public class HumicMud : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileMergeDirt[Type] = true;
		Main.tileBlockLight[Type] = true;
		DustType = ModContent.DustType<HumicMud_Dust>();
		AddMapEntry(new Color(34, 43, 39));
	}
}