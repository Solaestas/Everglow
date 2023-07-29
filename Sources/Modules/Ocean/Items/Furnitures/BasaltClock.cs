using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;

namespace Everglow.Ocean.Items.Furnitures
{
	public class BasaltClock : ModItem
	{
		public override void SetStaticDefaults()
		{
            // DisplayName.AddTranslation(GameCulture.Chinese, "玄武岩时钟");
        }

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 22;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = 1;
			Item.consumable = true;
			Item.value = 0;
			Item.createTile = Mod.Find<ModTile>("玄武岩时钟").Type;
		}
		public override void AddRecipes()
		{
			Recipe modRecipe = CreateRecipe(1);
			modRecipe.AddIngredient(22, 3);
			modRecipe.anyIronBar = true;
			modRecipe.AddIngredient(170, 6);
			modRecipe.AddIngredient(null, "Basalt", 10);
			modRecipe.AddTile(16);
			modRecipe.Register();
		}
	}
}
