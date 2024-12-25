using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ObjectData;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles;

public class FurnaceCopperPipe : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileSolidTop[Type] = true;
		Main.tileMergeDirt[Type] = false;
		Main.tileNoAttach[Type] = true;
		Main.tileBlendAll[Type] = true;
		Main.tileBlockLight[Type] = false;
		DustType = DustID.Copper;
		HitSound = default;

		AddMapEntry(new Color(132, 84, 58));
	}

	public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak) => base.TileFrame(i, j, ref resetFrame, ref noBreak);
}