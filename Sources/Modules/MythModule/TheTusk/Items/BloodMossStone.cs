﻿using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.TheTusk.Items
{
    public class BloodMossStone : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blood Moss Stone");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "血苔石");
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.BloodMossStone>();
        }
    }
}
