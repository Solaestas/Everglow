namespace Everglow.Sources.Modules.YggdrasilModule.YggdrasilTown.Items
{
    public class CyanVineOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cyan Ore");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "青缎矿");
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 20;
            Item.rare = ItemRarityID.Orange;
            Item.scale = 1f;
            Item.createTile = 0;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.maxStack = 999;
            Item.value = 400;
            Item.createTile = ModContent.TileType<Tiles.CyanVine.CyanVineStone>();
        }
    }
}
