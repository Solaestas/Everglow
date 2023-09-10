using Everglow.Ocean.NPCs;
using Terraria.GameContent.Generation;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace Everglow.Ocean.OceanDeep.Items
{
    public class OceanDustCore : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("海因子");
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