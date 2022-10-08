namespace Everglow.Sources.Modules.YggdrasilModule.HurricaneMaze.Items
{
    public class CyanWindGranite : ModItem
    {
        public override void SetStaticDefaults()
        {
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
            Item.createTile = ModContent.TileType<Tiles.CyanWindGranite>();
        }
    }
}
