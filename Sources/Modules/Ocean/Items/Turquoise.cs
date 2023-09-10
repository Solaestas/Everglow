using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.Items
{
	// Token: 0x020005BF RID: 1471
    public class Turquoise : ModItem
	{
		// Token: 0x060019E8 RID: 6632 RVA: 0x00008E84 File Offset: 0x00007084
		public override void SetStaticDefaults()
		{
            //base.DisplayName.SetDefault("绿松石");
            //base.// DisplayName.AddTranslation(GameCulture.Chinese, "绿松石");
		}

		// Token: 0x060019E9 RID: 6633 RVA: 0x000A8668 File Offset: 0x000A6868
		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 999;
			Item.value = Item.sellPrice(0, 0, 30, 0);
			Item.rare = 0;
		}
	}
}
