using System;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.Items
{
	// Token: 0x02000674 RID: 1652
    public class OceanBlueBlock : ModItem
	{
		// Token: 0x06001CB0 RID: 7344 RVA: 0x00009BBE File Offset: 0x00007DBE
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Waveblue Block");
            base.DisplayName.AddTranslation(GameCulture.Chinese, "海蓝砖");
		}

		// Token: 0x06001CB1 RID: 7345 RVA: 0x000B5F1C File Offset: 0x000B411C
		public override void SetDefaults()
		{
			base.item.width = 20;
			base.item.height = 20;
			base.item.maxStack = 999;
			base.item.value = 2000;
			base.item.rare = 0;
			base.item.useTurn = true;
			base.item.autoReuse = true;
			base.item.useAnimation = 15;
			base.item.useTime = 10;
			base.item.useStyle = 1;
			base.item.consumable = true;
            base.item.createTile = base.mod.TileType("海蓝砖");
		}

		// Token: 0x06001CB2 RID: 7346 RVA: 0x000B5FD0 File Offset: 0x000B41D0
	}
}
