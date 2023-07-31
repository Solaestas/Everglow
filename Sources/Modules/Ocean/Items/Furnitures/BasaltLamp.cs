using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;

namespace Everglow.Ocean.Items.Furnitures
{
	public class BasaltLamp : ModItem
	{
		public override void SetStaticDefaults()
		{
            // Tooltip.SetDefault("玄武岩灯");
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
			Item.createTile = ModContent.TileType<Everglow.Ocean.Tiles.玄武岩灯>();
		}
		public override void AddRecipes()
		{
			Recipe modRecipe = CreateRecipe(1);
			modRecipe.AddIngredient(null, "Basalt", 3);
			modRecipe.AddIngredient(null, "LavaStone", 1);
			modRecipe.AddTile(16);
			modRecipe.Register();
		}
	}
}
