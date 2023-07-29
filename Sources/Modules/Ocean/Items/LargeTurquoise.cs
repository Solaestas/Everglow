using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.Items
{
    public class LargeTurquoise : ModItem
	{
		public override void SetStaticDefaults()
		{
   //         base.DisplayName.SetDefault("大绿松石");
			//base.Tooltip.SetDefault("For Capture the Gem. It drops when you die");
   //         base.DisplayName.AddTranslation(GameCulture.Chinese, "大绿松石");
			//base.Tooltip.AddTranslation(GameCulture.Chinese, "适合夺取宝石。你死后掉落(这个版本暂且不能使用)");
		}
		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 1;
			Item.value = 0;
			Item.rare = 1;
		}
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Everglow.Ocean.Items.Turquoise>(), 15);
			recipe.Register();
        }
	}
}
