using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.Items.Walls
{
	// Token: 0x0200066F RID: 1647
    public class OceanRoomWall : ModItem
	{
		// Token: 0x06001C9C RID: 7324 RVA: 0x00009B22 File Offset: 0x00007D22
		public override void SetStaticDefaults()
		{
			// base.DisplayName.SetDefault("VolcanoStone Wall");
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "海洋密室砖墙");
		}

		// Token: 0x06001C9D RID: 7325 RVA: 0x000B5A14 File Offset: 0x000B3C14
		public override void SetDefaults()
		{
			base.Item.width = 24;
			base.Item.height = 24;
			base.Item.maxStack = 999;
			base.Item.useTurn = true;
			base.Item.autoReuse = true;
			base.Item.useAnimation = 15;
			base.Item.useTime = 7;
			base.Item.useStyle = 1;
			base.Item.consumable = true;
            base.Item.createWall = ModContent.WallType<Everglow.Ocean.Walls.海洋密室砖墙>();
		}

		// Token: 0x06001C9E RID: 7326 RVA: 0x000B5AB0 File Offset: 0x000B3CB0
		public override void AddRecipes()
		{
			Recipe modRecipe = /* base */Recipe.Create(this.Type, 4);
            modRecipe.AddIngredient(null, "OceanStoneBlock", 1);
			modRecipe.AddTile(18);
			modRecipe.Register();
		}
	}
}
