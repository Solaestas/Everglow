using System;
using Terraria.ModLoader;
using Terraria.Localization;

namespace MythMod.Items.Furnitures
{
	public class BasaltClock : ModItem
	{
		public override void SetStaticDefaults()
		{
            DisplayName.AddTranslation(GameCulture.Chinese, "玄武岩时钟");
        }

		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 22;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.value = 0;
			item.createTile = mod.TileType("玄武岩时钟");
		}
		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(22, 3);
			modRecipe.anyIronBar = true;
			modRecipe.AddIngredient(170, 6);
			modRecipe.AddIngredient(null, "Basalt", 10);
			modRecipe.SetResult(this, 1);
			modRecipe.AddTile(16);
			modRecipe.AddRecipe();
		}
	}
}
