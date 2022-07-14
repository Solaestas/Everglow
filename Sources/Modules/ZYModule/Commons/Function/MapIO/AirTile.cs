using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ObjectData;

namespace I.MapIO
{
    internal class AirTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            AddMapEntry(Color.White);
        }
    }

    internal class AirTileItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.useTime = 5;
            Item.useAnimation = 5;
            Item.createTile = ModContent.TileType<AirTile>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
        }
    }
}
