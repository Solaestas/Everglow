using Terraria.Localization;

namespace MythMod.OceanMod.Items
{
    public class Alcyonarian : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Alcyonarian");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "海鸡冠");
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
            Item.createTile = ModContent.TileType<OceanMod.Tiles.Alcyonarian>();
        }
    }
}
