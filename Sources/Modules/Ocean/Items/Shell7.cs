using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MythMod.Items.Shells
{
    public class Shell7 : ModItem
	{
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("女王凤凰螺");
            base.Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			base.item.width = 54;
			base.item.height = 66;
            base.item.maxStack = 999;
            base.item.rare = 8;
            base.item.value = Item.sellPrice(0, 1, 50, 0);
            base.item.useAnimation = 15;
            base.item.useTime = 10;
            base.item.useStyle = 1;
            base.item.consumable = true;
            base.item.useTurn = true;
            base.item.autoReuse = true;
            base.item.createTile = base.mod.TileType("女王凤凰螺");
        }
	}
}
