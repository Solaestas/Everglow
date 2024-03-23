using Everglow.Yggdrasil.KelpCurtain.Dusts;

namespace Everglow.Yggdrasil.KelpCurtain.Tiles;

public class CyatheaWood : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileMergeDirt[Type] = true;
		Main.tileBlockLight[Type] = true;
		DustType = ModContent.DustType<YggdrasilCyatheaTrunkDust>();
		AddMapEntry(new Color(96, 72, 42));
	}
}