using Everglow.Yggdrasil.YggdrasilTown.Dusts;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles;

public class TownBrick_Gray : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileMergeDirt[Type] = false;

		Main.tileBlendAll[Type] = false;
		Main.tileBlockLight[Type] = true;
		Main.tileShine2[Type] = false;

		DustType = ModContent.DustType<TownBrick_Khaki_Dust>();
		HitSound = default;

		AddMapEntry(new Color(154, 165, 161));
	}
}