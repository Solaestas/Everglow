using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.Items.Walls
{
    public class BasaltWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			// // base.DisplayName.SetDefault("VolcanoStone Wall");
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "玄武岩墙");
		}
		public override void SetDefaults()
		{
			base.Item.width = 12;
			base.Item.height = 12;
			base.Item.maxStack = 999;
			base.Item.useTurn = true;
			base.Item.autoReuse = true;
			base.Item.useAnimation = 15;
			base.Item.useTime = 7;
			base.Item.useStyle = 1;
			base.Item.consumable = true;
            base.Item.createWall = base.Mod.Find<ModWall>("BackGWall").Type;
		}
		public override void AddRecipes()
		{
			Recipe modRecipe = /* base */Recipe.Create(this.Type, 4);
            modRecipe.AddIngredient(null, "Basalt", 1);
			modRecipe.AddTile(18);
			modRecipe.Register();
		}
	}
}
