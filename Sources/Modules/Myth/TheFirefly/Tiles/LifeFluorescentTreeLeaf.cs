using Everglow.Myth.TheFirefly.Dusts;

namespace Everglow.Myth.TheFirefly.Tiles
{
	public class LifeFluorescentTreeLeaf : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileMerge[Type][ModContent.TileType<LifeFluorescentTreeWood>()] = true;
			Main.tileMerge[ModContent.TileType<LifeFluorescentTreeWood>()][Type] = true;
			DustType = ModContent.DustType<FluorescentLeafDust>();

			AddMapEntry(new Color(0, 53, 158));
		}
	}
}