using System;
using Terraria.ModLoader;
using Terraria.Localization;

namespace MythMod.Items.Furnitures
{
	public class BasaltBed : ModItem
	{
		public override void SetStaticDefaults()
		{
            DisplayName.SetDefault("BasaltBed");
            DisplayName.AddTranslation(GameCulture.Chinese, "玄武岩床");
        }
		public override void SetDefaults()
		{
			item.SetNameOverride("玄武岩床");
			item.width = 28;
			item.height = 20;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.value = 0;
			item.consumable = true;
			item.createTile = mod.TileType("玄武岩床");
		}
		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(null, "Basalt", 15);
            modRecipe.AddIngredient(225, 5);
            modRecipe.SetResult(this, 1);
			modRecipe.AddTile(16);
			modRecipe.AddRecipe();
		}
	}
}
