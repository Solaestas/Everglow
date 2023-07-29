using Terraria.Localization;

namespace Everglow.Ocean.Items
{
    public class GoldGorgonianBranch : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("GoldGorgonianBranch");
            // DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "金柳珊瑚枝");
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 36;
            Item.maxStack = 999;
            Item.rare = 0;
            Item.value = Item.sellPrice(0, 0, 3, 0);
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useStyle = 1;
            Item.consumable = true;
            Item.useTurn = true;
            Item.autoReuse = true;
            //Item.createTile = ModContent.TileType<Tiles.GoldGorgonianLarge>();
        }
    }
}
