using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.Items.Gems
{
    public class LargeOlivine : ModItem
	{
		public override void SetStaticDefaults()
		{
            // base.DisplayName.SetDefault("大橄榄石");
			// base.Tooltip.SetDefault("For Capture the Gem. It drops when you die");
            // base.DisplayName.AddTranslation(GameCulture.Chinese, "大橄榄石");
			// base.Tooltip.AddTranslation(GameCulture.Chinese, "适合夺取宝石。你死后掉落");
		}
		public override void SetDefaults()
		{
			base.Item.width = 20;
			base.Item.height = 20;
			base.Item.maxStack = 1;
			base.Item.value = 0;
			base.Item.rare = 1;
		}
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(null, "Olivine", 15); 
            recipe.Register();
        }
        public override void UpdateInventory(Player player)
        {
            ((OceanContentPlayer)player.GetModPlayer(base.Mod, "OceanContentPlayer")).LargeOlivine = true;
        }
    }
}
