using System;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.Items.Walls
{
	// Token: 0x0200066F RID: 1647
    public class OceanBrickWall : ModItem
	{
		// Token: 0x06001C9C RID: 7324 RVA: 0x00009B22 File Offset: 0x00007D22
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Bluewave Wall");
            base.DisplayName.AddTranslation(GameCulture.Chinese, "海蓝砖墙");
		}

		// Token: 0x06001C9D RID: 7325 RVA: 0x000B5A14 File Offset: 0x000B3C14
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
            base.item.createWall = base.mod.WallType("海蓝砖墙");
		}

		// Token: 0x06001C9E RID: 7326 RVA: 0x000B5AB0 File Offset: 0x000B3CB0
		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(base.mod);
            modRecipe.AddIngredient(null, "OceanBlueBlock", 1);
			modRecipe.AddTile(18);
			modRecipe.SetResult(this, 4);
			modRecipe.AddRecipe();
		}
	}
}
