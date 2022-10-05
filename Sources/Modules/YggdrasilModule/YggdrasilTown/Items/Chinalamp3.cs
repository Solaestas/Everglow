namespace Everglow.Sources.Modules.YggdrasilModule.YggdrasilTown.Items
{
    public class Chinalamp3 : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ancient Custom Lamp");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "古韵台灯");
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 26;
            Item.rare = ItemRarityID.White;
            Item.scale = 1f;
            Item.createTile = ModContent.TileType<Tiles.Chinalamp3>();
            Item.useStyle = ItemUseStyleID.Swing;
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
