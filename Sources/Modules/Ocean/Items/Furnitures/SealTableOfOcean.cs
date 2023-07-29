using System;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.Items
{
	// Token: 0x02000671 RID: 1649
    public class SealTableOfOcean : ModItem
	{
		// Token: 0x06001CA4 RID: 7332 RVA: 0x00009B49 File Offset: 0x00007D49
		public override void SetStaticDefaults()
		{
            // // base.DisplayName.SetDefault("海洋封印台");
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "海洋封印台");
		}

		// Token: 0x06001CA5 RID: 7333 RVA: 0x000B5C5C File Offset: 0x000B3E5C
		public override void SetDefaults()
		{
			base.Item.width = 28;
			base.Item.height = 14;
			base.Item.maxStack = 99;
			base.Item.useTurn = true;
			base.Item.autoReuse = true;
			base.Item.useAnimation = 15;
			base.Item.useTime = 10;
			base.Item.useStyle = 1;
			base.Item.consumable = true;
			base.Item.value = 0;
            base.Item.createTile = base.Mod.Find<ModTile>("海洋封印台").Type;
		}
	}
}
