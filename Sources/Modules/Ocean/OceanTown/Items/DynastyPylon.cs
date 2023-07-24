using Terraria.Localization;
namespace Everglow.Ocean.Items.Town
{
    public class LanternCrystal : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Dynasty Pylon");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "皇城晶塔");
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 42;
            Item.rare = 1;
            Item.scale = 1f;
            Item.createTile = ModContent.TileType<Tiles.Town.LanternCrystal>();
            //Item.createTile = ModContent.TileType<Tiles.Tree1.ExampleTransportBall>();
            Item.useStyle = 1;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.maxStack = 999;
            Item.value = 10000;
        }
    }
}
