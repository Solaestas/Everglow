namespace Everglow.Sources.Modules.YggdrasilModule.YggdrasilTown.Items
{
    public class StreetLantern : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Lantern Post");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "灯笼柱");
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 28;
            Item.rare = 0;
            Item.scale = 1f;
            Item.createTile = ModContent.TileType<Tiles.StreetLantern>();
            Item.useStyle = 1;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.maxStack = 999;
            Item.value = 1000;
        }
    }
}
