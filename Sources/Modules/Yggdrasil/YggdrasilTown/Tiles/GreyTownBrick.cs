using Everglow.Yggdrasil.YggdrasilTown.Dusts;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles;

public class GreyTownBrick : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileMergeDirt[Type] = false;

		Main.tileBlendAll[Type] = false;
		Main.tileBlockLight[Type] = true;
		Main.tileShine2[Type] = false;

		DustType = ModContent.DustType<StoneDragonScaleWoodDust>();
		MinPick = 100;
		HitSound = SoundID.Dig;

		AddMapEntry(new Color(84, 84, 84));
	}
}
