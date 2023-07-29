using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
namespace Everglow.Ocean.Items.Corals
{
    public class PirateFlag : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("海盗船旗帜");
            // Tooltip.SetDefault("");
        }
        public override void SetDefaults()
        {
            base.Item.width = 48;
            base.Item.height = 14;
            base.Item.rare = 0;
            base.Item.scale = 1f;
            base.Item.createTile = base.Mod.Find<ModTile>("海盗船旗帜").Type;
            base.Item.useStyle = 1;
            base.Item.useTurn = true;
            base.Item.useAnimation = 15;
            base.Item.useTime = 10;
            base.Item.autoReuse = true;
            base.Item.consumable = true;
            base.Item.maxStack = 999;
            base.Item.value = 10000;
        }
        public override void AddRecipes()
        {
        }
    }
}
