using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
namespace MythMod.Items.Corals
{
    public class PirateFlag : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("海盗船旗帜");
            Tooltip.SetDefault("");
        }
        public override void SetDefaults()
        {
            base.item.width = 48;
            base.item.height = 14;
            base.item.rare = 0;
            base.item.scale = 1f;
            base.item.createTile = base.mod.TileType("海盗船旗帜");
            base.item.useStyle = 1;
            base.item.useTurn = true;
            base.item.useAnimation = 15;
            base.item.useTime = 10;
            base.item.autoReuse = true;
            base.item.consumable = true;
            base.item.maxStack = 999;
            base.item.value = 10000;
        }
        public override void AddRecipes()
        {
        }
    }
}
