using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.Items.Walls
{
	// Token: 0x0200066F RID: 1647
    public class OceanBrickWall : ModItem
	{
		// Token: 0x06001C9C RID: 7324 RVA: 0x00009B22 File Offset: 0x00007D22
		public override void SetStaticDefaults()
		{
			// // base.DisplayName.SetDefault("Bluewave Wall");
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "海蓝砖墙");
		}

		// Token: 0x06001C9D RID: 7325 RVA: 0x000B5A14 File Offset: 0x000B3C14
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
            base.Item.createWall = base.Mod.Find<ModWall>("海蓝砖墙").Type;
		}

		// Token: 0x06001C9E RID: 7326 RVA: 0x000B5AB0 File Offset: 0x000B3CB0
		public override void AddRecipes()
		{
			Recipe modRecipe = /* base */Recipe.Create(this.Type, 4);
            modRecipe.AddIngredient(null, "OceanBlueBlock", 1);
			modRecipe.AddTile(18);
			modRecipe.Register();
		}
	}
}
