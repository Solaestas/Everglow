using System;
using Terraria.ModLoader;
using Terraria.Localization;
namespace MythMod.Items.Furnitures
{
	public class BasaltCandlestick : ModItem
	{
		public override void SetStaticDefaults()
		{
            DisplayName.AddTranslation(GameCulture.Chinese, "玄武岩烛台");
        }
		public override void SetDefaults()
		{
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
			item.createTile = mod.TileType("玄武岩烛台");
		}
		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(null, "Basalt", 5);
			modRecipe.AddIngredient(null, "LavaStone", 5);
			modRecipe.SetResult(this, 1);
			modRecipe.AddTile(16);
			modRecipe.AddRecipe();
		}
	}
}
