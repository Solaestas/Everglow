using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Everglow.Ocean.Items.Shells
{
	// Token: 0x0200015B RID: 347
    public class Shell6 : ModItem
	{
		// Token: 0x060005E3 RID: 1507 RVA: 0x00041728 File Offset: 0x0003F928
		public override void SetStaticDefaults()
		{
            // // base.DisplayName.SetDefault("马蹄钟螺");
            // base.Tooltip.SetDefault("");
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x00041780 File Offset: 0x0003F980
		public override void SetDefaults()
		{
			base.Item.width = 28;
			base.Item.height = 28;
            base.Item.maxStack = 999;
            base.Item.rare = 3;
            base.Item.value = Item.sellPrice(0, 15, 0, 0);
            base.Item.useAnimation = 15;
            base.Item.useTime = 10;
            base.Item.useStyle = 1;
            base.Item.consumable = true;
            base.Item.useTurn = true;
            base.Item.autoReuse = true;
            base.Item.createTile = base.Mod.Find<ModTile>("马蹄钟螺").Type;
        }
	}
}
