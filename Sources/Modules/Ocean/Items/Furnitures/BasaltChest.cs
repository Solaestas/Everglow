using System;
using Terraria.ModLoader;
using Terraria.Localization;

namespace MythMod.Items.Furnitures
{
	public class BasaltChest : ModItem
	{
		public override void SetStaticDefaults()
		{
            DisplayName.AddTranslation(GameCulture.Chinese, "玄武岩箱");
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
			item.createTile = mod.TileType("玄武岩箱");
		}
		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(null, "Basalt", 8);
			modRecipe.AddIngredient(22, 2);
			modRecipe.anyIronBar = true;
			modRecipe.SetResult(this, 1);
			modRecipe.AddTile(16);
			modRecipe.AddRecipe();
		}
	}
}
