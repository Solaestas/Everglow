using System;
using Terraria.ModLoader;
using Terraria.Localization;
namespace MythMod.Items.Furnitures
{
	public class BasaltPiano : ModItem
	{
		public override void SetStaticDefaults()
		{
            DisplayName.AddTranslation(GameCulture.Chinese, "玄武岩箱子");
        }

		public override void SetDefaults()
		{
			item.SetNameOverride("玄武岩钢琴");
			item.width = 26;
			item.height = 26;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.value = 0;
			item.createTile = mod.TileType("玄武岩钢琴");
		}

		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(154, 4);
			modRecipe.AddIngredient(null, "Basalt", 15);
			modRecipe.AddIngredient(149, 1);
			modRecipe.SetResult(this, 1);
			modRecipe.AddTile(16);
			modRecipe.AddRecipe();
		}
	}
}
