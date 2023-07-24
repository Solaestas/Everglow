using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.Items.Gems
{
    public class LargeOlivine : ModItem
	{
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("大橄榄石");
			base.Tooltip.SetDefault("For Capture the Gem. It drops when you die");
            base.DisplayName.AddTranslation(GameCulture.Chinese, "大橄榄石");
			base.Tooltip.AddTranslation(GameCulture.Chinese, "适合夺取宝石。你死后掉落");
		}
		public override void SetDefaults()
		{
			base.item.width = 20;
			base.item.height = 20;
			base.item.maxStack = 1;
			base.item.value = 0;
			base.item.rare = 1;
		}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Olivine", 15); 
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
        public override void UpdateInventory(Player player)
        {
            ((MythPlayer)player.GetModPlayer(base.mod, "MythPlayer")).LargeOlivine = true;
        }
    }
}
