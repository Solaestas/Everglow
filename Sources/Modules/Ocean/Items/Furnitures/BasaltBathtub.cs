using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;

namespace Everglow.Ocean.Items.Furnitures
{

	public class BasaltBathtub : ModItem
	{
		public override void SetStaticDefaults()
		{
            // DisplayName.AddTranslation(GameCulture.Chinese, "玄武岩浴缸");
        }
		public override void SetDefaults()
		{
			base.Item.SetNameOverride("玄武岩浴缸");
			base.Item.width = 28;
			base.Item.height = 20;
			base.Item.maxStack = 999;
			base.Item.useTurn = true;
			base.Item.autoReuse = true;
			base.Item.useAnimation = 15;
			base.Item.useTime = 10;
			base.Item.useStyle = 1;
			base.Item.value = 0;
			base.Item.consumable = true;
			base.Item.createTile = base.Mod.Find<ModTile>("玄武岩浴缸").Type;
		}
		public override void AddRecipes()
		{
			Recipe modRecipe = /* base */Recipe.Create(this.Type, 1);
			modRecipe.AddIngredient(null, "Basalt", 14);
			modRecipe.AddTile(16);
			modRecipe.Register();
		}
	}
}
