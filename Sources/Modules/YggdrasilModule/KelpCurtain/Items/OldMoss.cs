﻿namespace Everglow.Sources.Modules.YggdrasilModule.KelpCurtain.Items
{
    public class OldMoss : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Moss");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "苍苔");
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.rare = 0;
            Item.scale = 1f;
            Item.createTile = ModContent.TileType<Tiles.OldMoss>();
            Item.useStyle = 1;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
            Item.value = 100;
        }
    }
}
