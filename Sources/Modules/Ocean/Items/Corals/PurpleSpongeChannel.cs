using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
namespace Everglow.Ocean.Items.Corals
{
    public class PurpleSpongeChannel : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("紫色海绵管");
            // Tooltip.SetDefault("");
        }
        public override void SetDefaults()
        {
            base.Item.width = 40;
            base.Item.height = 24;
            base.Item.rare = 2;
            base.Item.scale = 1f;
            base.Item.maxStack = 999;
            base.Item.value = 3000;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(3);
            recipe.AddIngredient(null, "PurpleLittleSponge", 1);
            recipe.Register();
            Recipe recipe2 = CreateRecipe(5);
            recipe2.AddIngredient(null, "PurpleSponge", 1);
            recipe2.Register();
        }
    }
}
