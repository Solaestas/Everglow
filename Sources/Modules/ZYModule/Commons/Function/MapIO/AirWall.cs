using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.ZYModule.Commons.Function.MapIO
{
    internal class AirWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            AddMapEntry(Color.White);
        }
    }


    internal class AirWallItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.useTime = 5;
            Item.useAnimation = 5;
            Item.createTile = ModContent.WallType<AirWall>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
        }
    }
}
