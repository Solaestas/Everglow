using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;

namespace Everglow.Ocean.Items.Furnitures
{
	public class BasaltBed : ModItem
	{
		public override void SetStaticDefaults()
		{
            // DisplayName.SetDefault("BasaltBed");
            // // DisplayName.AddTranslation(GameCulture.Chinese, "玄武岩床");
        }
		public override void SetDefaults()
		{
			Item.SetNameOverride("玄武岩床");
			Item.width = 28;
			Item.height = 20;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = 1;
			Item.value = 0;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Everglow.Ocean.Tiles.玄武岩床>();
		}
		public override void AddRecipes()
		{
			Recipe modRecipe = CreateRecipe(1);
			modRecipe.AddIngredient(null, "Basalt", 15);
            modRecipe.AddIngredient(225, 5);
			modRecipe.AddTile(16);
			modRecipe.Register();
		}
	}
}
