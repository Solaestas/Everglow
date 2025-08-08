using Everglow.Yggdrasil.KelpCurtain.Dusts;

namespace Everglow.Yggdrasil.KelpCurtain.Tiles;

public class JadeizedBone : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileMergeDirt[Type] = true;
		Main.tileBlockLight[Type] = true;
		DustType = ModContent.DustType<JadeizedBoneDust>();
		AddMapEntry(new Color(216, 190, 171));
	}
}