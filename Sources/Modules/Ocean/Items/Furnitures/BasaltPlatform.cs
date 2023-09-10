using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
namespace Everglow.Ocean.Items.Furnitures
{
	public class BasaltPlatform : ModItem
	{
		public override void SetStaticDefaults()
		{
            // // DisplayName.AddTranslation(GameCulture.Chinese, "玄武岩箱子");
        }

		public override void SetDefaults()
		{
			Item.width = 8;
			Item.height = 10;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = 1;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Everglow.Ocean.Tiles.玄武岩平台>();
		}

		public override void AddRecipes()
		{
			Recipe modRecipe = CreateRecipe(2);
			modRecipe.AddIngredient(null, "Basalt", 1);
			modRecipe.AddTile(16);
			modRecipe.Register();
		}
	}
}
