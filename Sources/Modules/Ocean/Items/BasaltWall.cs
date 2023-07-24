using System;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.Items.Walls
{
    public class BasaltWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("VolcanoStone Wall");
            base.DisplayName.AddTranslation(GameCulture.Chinese, "玄武岩墙");
		}
		public override void SetDefaults()
		{
			base.item.width = 12;
			base.item.height = 12;
			base.item.maxStack = 999;
			base.item.useTurn = true;
			base.item.autoReuse = true;
			base.item.useAnimation = 15;
			base.item.useTime = 7;
			base.item.useStyle = 1;
			base.item.consumable = true;
            base.item.createWall = base.mod.WallType("BackGWall");
		}
		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(base.mod);
            modRecipe.AddIngredient(null, "Basalt", 1);
			modRecipe.AddTile(18);
			modRecipe.SetResult(this, 4);
			modRecipe.AddRecipe();
		}
	}
}
