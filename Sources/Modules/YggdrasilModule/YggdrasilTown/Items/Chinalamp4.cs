namespace Everglow.Sources.Modules.YggdrasilModule.YggdrasilTown.Items
{
    public class Chinalamp4 : ModItem
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 26;
            Item.rare = 0;
            Item.scale = 1f;
            Item.createTile = ModContent.TileType<Tiles.Chinalamp4>();
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
