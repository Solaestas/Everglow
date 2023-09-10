using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
namespace Everglow.Ocean.Items.Furnitures
{
	public class BasaltBookshelf : ModItem
	{
		public override void SetStaticDefaults()
		{
            // // DisplayName.AddTranslation(GameCulture.Chinese, "玄武岩书架");
        }
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 20;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = 1;
			Item.consumable = true;
			Item.value = 1000;
			Item.createTile = ModContent.TileType<Everglow.Ocean.Tiles.玄武岩书架>();
		}
		public override void AddRecipes()
		{
			Recipe modRecipe = CreateRecipe(1);
			modRecipe.AddIngredient(null, "Basalt", 20);
			modRecipe.AddIngredient(149, 10);
			modRecipe.AddTile(16);
			modRecipe.Register();
		}
	}
}
