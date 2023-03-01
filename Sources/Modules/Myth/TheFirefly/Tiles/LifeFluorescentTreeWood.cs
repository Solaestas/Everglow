using Everglow.Sources.Modules.MythModule.Common;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Tiles
{
    public class LifeFluorescentTreeWood : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileMerge[Type][ModContent.TileType<LifeFluorescentTreeLeaf>()] = true;
            Main.tileMerge[ModContent.TileType<LifeFluorescentTreeLeaf>()][Type] = true;
            DustType = ModContent.DustType<Dusts.FluorescentLeafDust>();
            ItemDrop = ModContent.ItemType<Items.GlowWood>();
            AddMapEntry(new Color(55, 24, 63));
        }
    }
}