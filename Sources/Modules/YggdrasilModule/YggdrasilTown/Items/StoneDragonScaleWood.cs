namespace Everglow.Sources.Modules.YggdrasilModule.YggdrasilTown.Items
{
    public class StoneDragonScaleWood : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Fossilized Dragon Scale Wood");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "石化龙鳞木");
        }
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.rare = 0;
            Item.scale = 1f;
            Item.createTile = ModContent.TileType<Tiles.StoneScaleWood>();
            Item.useStyle = 1;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
            Item.value = 1000;
        }
    }
}
