using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
namespace Everglow.Ocean.Items.UnderSea
{
    public class BladeScale : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("利刃鳞");
            // Tooltip.SetDefault("");
        }
        public override void SetDefaults()
        {
            Item refItem = new Item();
            Item.width = refItem.width;
            Item.height = refItem.height;
            Item.maxStack = 999;
            Item.value = 2000;
            Item.rare = 8;
        }
    }
}