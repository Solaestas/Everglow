using Terraria.Localization;
namespace MythMod.OceanMod.Items
{
    public class Basalt : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Basalt");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "玄武岩");
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.rare = 0;
            Item.scale = 1f;
            Item.createTile = ModContent.TileType<OceanMod.Tiles.Basalt>();
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
