using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MythMod.Items.Shells
{
    public class ShellBell : ModItem
	{
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("贝壳风铃");
            base.Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			base.item.width = 28;
			base.item.height = 28;
			base.item.useAnimation = 20;
			base.item.useTime = 20;
            base.item.maxStack = 999;
            base.item.rare = 3;
            base.item.value = Item.sellPrice(0, 5, 0, 0);
            base.item.useAnimation = 15;
            base.item.useTime = 10;
            base.item.useStyle = 1;
            base.item.consumable = true;
            base.item.useTurn = true;
            base.item.autoReuse = true;
            base.item.createTile = base.mod.TileType("贝壳风铃");
        }
	}
}
