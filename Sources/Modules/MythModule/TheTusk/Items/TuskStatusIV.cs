using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.TheTusk.Items
{
    public class TuskStatusIV : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crimson Statue IV");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "猩红碑文石·其四");
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
            Item.createTile = ModContent.TileType<Tiles.StrangeTuskStone4>();
            Item.placeStyle = 0;
        }
    }
}
