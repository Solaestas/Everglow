using Terraria.Localization;

namespace MythMod.OceanMod.Items
{
    public class MarbleCono : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("MarbleCono");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "大理石芋螺");
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.maxStack = 999;
            Item.rare = 1;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = 1;
            Item.consumable = true;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.createTile = ModContent.TileType<OceanMod.Tiles.MarbleCono>();
        }
    }
}
