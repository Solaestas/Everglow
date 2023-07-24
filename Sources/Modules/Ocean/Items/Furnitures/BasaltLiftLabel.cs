using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
namespace MythMod.Items.Furnitures
{
    public class BasaltLiftLabel : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("玄武岩电梯楼层显示标");
        }
        public override void SetDefaults()
        {
            item.width = 48;
            item.height = 64;
            item.rare = 2;
            item.scale = 1f;
            item.createTile = mod.TileType("玄武岩电梯楼层显示标");
            item.useStyle = 1;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.autoReuse = true;
            item.consumable = true;
            item.maxStack = 999;
            item.value = 400;
        }
        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(null, "LiftLabel", 1);
            modRecipe.AddIngredient(null, "Basalt", 2);
            modRecipe.SetResult(this, 1);
            modRecipe.AddTile(18);
            modRecipe.AddRecipe();
        }
    }
}
