using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.Items.Furnitures
{
	public class BasaltTank : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("");
			// Tooltip.AddTranslation(GameCulture.Chinese, "玄武岩工作台");
		}
        public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 26;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = 1;
			Item.consumable = true;
			Item.value = 0;
			Item.createTile = ModContent.TileType<Everglow.Ocean.Tiles.玄武岩水槽>();
		}
		public override void AddRecipes()
		{
			Recipe modRecipe = CreateRecipe(1);
			modRecipe.AddIngredient(null, "Basalt", 6);
			modRecipe.AddIngredient(206, 1);
			modRecipe.AddTile(16);
			modRecipe.Register();
		}
	}
}
