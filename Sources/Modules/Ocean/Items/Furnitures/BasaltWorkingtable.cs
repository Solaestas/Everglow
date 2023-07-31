using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;

namespace Everglow.Ocean.Items.Furnitures
{
	public class BasaltWorkingtable : ModItem
	{
		public override void SetStaticDefaults()
		{
            // DisplayName.AddTranslation(GameCulture.Chinese, "玄武岩工作台");
        }
		public override void SetDefaults()
		{
			Item.SetNameOverride("玄武岩工作台");
			Item.width = 28;
			Item.height = 14;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = 1;
			Item.consumable = true;
			Item.value = 0;
			Item.createTile = ModContent.TileType<Everglow.Ocean.Tiles.玄武岩工作台>();
		}
		public override void AddRecipes()
		{
			Recipe modRecipe = CreateRecipe(1);
			modRecipe.AddIngredient(null, "Basalt", 10);
			modRecipe.AddTile(16);
			modRecipe.Register();
		}
	}
}
