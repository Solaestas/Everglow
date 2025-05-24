using Everglow.Yggdrasil.YggdrasilTown.Dusts;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles;

public class TownBrick_Khaki : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileMergeDirt[Type] = false;

		Main.tileBlendAll[Type] = true;
		Main.tileBlockLight[Type] = true;

		DustType = ModContent.DustType<TownBrick_Khaki_Dust>();
		HitSound = default;

		AddMapEntry(new Color(176, 161, 138));
	}
}