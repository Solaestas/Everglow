using System;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.Items
{
	// Token: 0x02000671 RID: 1649
    public class SealTableOfOcean : ModItem
	{
		// Token: 0x06001CA4 RID: 7332 RVA: 0x00009B49 File Offset: 0x00007D49
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("海洋封印台");
            base.DisplayName.AddTranslation(GameCulture.Chinese, "海洋封印台");
		}

		// Token: 0x06001CA5 RID: 7333 RVA: 0x000B5C5C File Offset: 0x000B3E5C
		public override void SetDefaults()
		{
			base.item.width = 28;
			base.item.height = 14;
			base.item.maxStack = 99;
			base.item.useTurn = true;
			base.item.autoReuse = true;
			base.item.useAnimation = 15;
			base.item.useTime = 10;
			base.item.useStyle = 1;
			base.item.consumable = true;
			base.item.value = 0;
            base.item.createTile = base.mod.TileType("海洋封印台");
		}
	}
}
