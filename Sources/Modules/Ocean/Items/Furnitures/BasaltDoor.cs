using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
namespace Everglow.Ocean.Items.Furnitures
{
	public class BasaltDoor : ModItem
	{
		public override void SetStaticDefaults()
		{
		}

		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 28;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = 1;
			Item.consumable = true;
			Item.value = 0;
			Item.createTile = Mod.Find<ModTile>("玄武岩门Closed").Type;
		}

		public override void AddRecipes()
		{
			Recipe modRecipe = CreateRecipe(1);
			modRecipe.AddIngredient(null, "Basalt", 6);
			modRecipe.AddTile(16);
			modRecipe.Register();
		}
	}
}
