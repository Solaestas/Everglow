using System;
using Terraria.ModLoader;
using Terraria.Localization;

namespace MythMod.Items.Furnitures
{

	public class BasaltBathtub : ModItem
	{
		public override void SetStaticDefaults()
		{
            DisplayName.AddTranslation(GameCulture.Chinese, "玄武岩浴缸");
        }
		public override void SetDefaults()
		{
			base.item.SetNameOverride("玄武岩浴缸");
			base.item.width = 28;
			base.item.height = 20;
			base.item.maxStack = 999;
			base.item.useTurn = true;
			base.item.autoReuse = true;
			base.item.useAnimation = 15;
			base.item.useTime = 10;
			base.item.useStyle = 1;
			base.item.value = 0;
			base.item.consumable = true;
			base.item.createTile = base.mod.TileType("玄武岩浴缸");
		}
		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(base.mod);
			modRecipe.AddIngredient(null, "Basalt", 14);
			modRecipe.SetResult(this, 1);
			modRecipe.AddTile(16);
			modRecipe.AddRecipe();
		}
	}
}
