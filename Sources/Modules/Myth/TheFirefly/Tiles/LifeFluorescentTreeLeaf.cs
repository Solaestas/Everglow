using Everglow.Sources.Modules.MythModule.Common;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Tiles
{
    public class LifeFluorescentTreeLeaf : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileMerge[Type][ModContent.TileType<LifeFluorescentTreeWood>()] = true;
            Main.tileMerge[ModContent.TileType<LifeFluorescentTreeWood>()][Type] = true;
            DustType = ModContent.DustType<Dusts.FluorescentLeafDust>();

            AddMapEntry(new Color(0, 53, 158));
        }
    }
}