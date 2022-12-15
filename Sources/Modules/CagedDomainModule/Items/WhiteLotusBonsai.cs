﻿namespace Everglow.Sources.Modules.CagedDomainModule.Items
{
    public class WhiteLotusBonsai : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 64;
            Item.maxStack = 999;
            Item.value = 10000;
            Item.rare = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.WhiteLotusBonsai>();
            Item.placeStyle = 0;
            Item.useTime = 10;
            Item.useAnimation = 10;
        }
        public override bool? UseItem(Player player)
        {
            return base.UseItem(player);
        }
        public override void HoldItem(Player player)
        {
            Item.placeStyle = Math.Max(player.direction, 0);
            base.HoldItem(player);
        }
    }
}