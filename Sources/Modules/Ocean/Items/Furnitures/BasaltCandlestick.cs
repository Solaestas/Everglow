using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
namespace Everglow.Ocean.Items.Furnitures
{
	public class BasaltCandlestick : ModItem
	{
		public override void SetStaticDefaults()
		{
            // DisplayName.AddTranslation(GameCulture.Chinese, "玄武岩烛台");
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
			Item.createTile = Mod.Find<ModTile>("玄武岩烛台").Type;
		}
		public override void AddRecipes()
		{
			Recipe modRecipe = CreateRecipe(1);
			modRecipe.AddIngredient(null, "Basalt", 5);
			modRecipe.AddIngredient(null, "LavaStone", 5);
			modRecipe.AddTile(16);
			modRecipe.Register();
		}
	}
}
