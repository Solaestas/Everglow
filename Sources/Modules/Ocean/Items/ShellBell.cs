using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Everglow.Ocean.Items.Shells
{
    public class ShellBell : ModItem
	{
		public override void SetStaticDefaults()
		{
            // // base.DisplayName.SetDefault("贝壳风铃");
            // base.Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			base.Item.width = 28;
			base.Item.height = 28;
			base.Item.useAnimation = 20;
			base.Item.useTime = 20;
            base.Item.maxStack = 999;
            base.Item.rare = 3;
            base.Item.value = Item.sellPrice(0, 5, 0, 0);
            base.Item.useAnimation = 15;
            base.Item.useTime = 10;
            base.Item.useStyle = 1;
            base.Item.consumable = true;
            base.Item.useTurn = true;
            base.Item.autoReuse = true;
            base.Item.createTile = base.Mod.Find<ModTile>("贝壳风铃").Type;
        }
	}
}
