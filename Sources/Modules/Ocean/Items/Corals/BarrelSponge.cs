using Terraria.Localization;

namespace Everglow.Ocean.Items.Corals
{
    public class BarrelSponge : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Barrel Sponge");
            // DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "桶海绵");
        }
        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 48;
            Item.maxStack = 999;
            Item.rare = 0;
            Item.value = Item.sellPrice(0, 0, 3, 0);
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useStyle = 1;
            Item.consumable = true;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.createTile = ModContent.TileType<Tiles.BarrelSponge>();
        }
    }
}
