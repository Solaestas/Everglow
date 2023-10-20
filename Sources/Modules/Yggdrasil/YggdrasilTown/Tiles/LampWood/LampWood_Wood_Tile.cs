using Everglow.Yggdrasil.YggdrasilTown.Dusts;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles.LampWood;

public class LampWood_Wood_Tile : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileBlendAll[Type] = false;
		Main.tileBlockLight[Type] = true;
		DustType = ModContent.DustType<StoneDragonScaleWoodDust>();
		HitSound = SoundID.Dig;

		AddMapEntry(new Color(50, 51, 51));
	}
}
