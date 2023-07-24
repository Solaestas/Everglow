using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
namespace MythMod.Items.UnderSea
{
    public class BladeScale : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("利刃鳞");
            Tooltip.SetDefault("");
        }
        public override void SetDefaults()
        {
            Item refItem = new Item();
            item.width = refItem.width;
            item.height = refItem.height;
            item.maxStack = 999;
            item.value = 2000;
            item.rare = 8;
        }
    }
}