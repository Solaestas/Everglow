namespace Everglow.Sources.Modules.YggdrasilModule.YggdrasilTown.Items
{
    public class YellowDynastyShingles : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Yellow Dynasty Shingles");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "黄王朝瓦");
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.rare = ItemRarityID.White;
            Item.scale = 1f;
            Item.createTile = ModContent.TileType<Tiles.YellowDynastyShingles>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.maxStack = 999;
            Item.value = 1000;
        }
    }
}
