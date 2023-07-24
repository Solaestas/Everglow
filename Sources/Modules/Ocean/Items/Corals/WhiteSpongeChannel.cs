using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
namespace MythMod.Items.Corals
{
    public class WhiteSpongeChannel : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("白色海绵管");
            Tooltip.SetDefault("");
        }
        public override void SetDefaults()
        {
            base.item.width = 40;
            base.item.height = 24;
            base.item.rare = 2;
            base.item.scale = 1f;
            base.item.maxStack = 999;
            base.item.value = 3000;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(null, "WhiteSponge", 1);
            recipe2.SetResult(this, 5);
            recipe2.AddRecipe();
        }
    }
}
