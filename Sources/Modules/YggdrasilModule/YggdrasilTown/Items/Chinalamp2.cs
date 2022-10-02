namespace Everglow.Sources.Modules.YggdrasilModule.YggdrasilTown.Items
{
    public class Chinalamp2 : ModItem
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
            Item.rare = 0;
            Item.scale = 1f;
            Item.createTile = ModContent.TileType<Tiles.Chinalamp2>();
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
