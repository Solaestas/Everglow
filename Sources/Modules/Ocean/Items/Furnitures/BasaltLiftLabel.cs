using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
namespace Everglow.Ocean.Items.Furnitures
{
    public class BasaltLiftLabel : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("玄武岩电梯楼层显示标");
        }
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 64;
            Item.rare = 2;
            Item.scale = 1f;
            Item.createTile = Mod.Find<ModTile>("玄武岩电梯楼层显示标").Type;
            Item.useStyle = 1;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.maxStack = 999;
            Item.value = 400;
        }
        public override void AddRecipes()
        {
            Recipe modRecipe = CreateRecipe(1);
            modRecipe.AddIngredient(null, "LiftLabel", 1);
            modRecipe.AddIngredient(null, "Basalt", 2);
            modRecipe.AddTile(18);
            modRecipe.Register();
        }
    }
}
