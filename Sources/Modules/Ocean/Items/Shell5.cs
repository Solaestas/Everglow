using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MythMod.Items.Shells
{
	// Token: 0x0200015B RID: 347
    public class Shell5 : ModItem
	{
		// Token: 0x060005E3 RID: 1507 RVA: 0x00041728 File Offset: 0x0003F928
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("金星宝螺");
            base.Tooltip.SetDefault("");
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x00041780 File Offset: 0x0003F980
		public override void SetDefaults()
		{
			base.item.width = 28;
			base.item.height = 28;
			base.item.useAnimation = 20;
			base.item.useTime = 20;
            base.item.maxStack = 999;
            base.item.rare = 8;
            base.item.value = Item.sellPrice(1, 50, 0, 0);
		}
	}
}
