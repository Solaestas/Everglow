using Everglow.Commons.VFX.Scene;
using Everglow.Yggdrasil.KelpCurtain.Dusts;

namespace Everglow.Yggdrasil.KelpCurtain.Tiles.DeathJadeLake;

public class Travertine : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileMergeDirt[Type] = true;
		Main.tileNoSunLight[Type] = true;
		Main.tileBlockLight[Type] = true;
		DustType = ModContent.DustType<Travertine_Dust>();
		AddMapEntry(new Color(29, 63, 46));
	}
}