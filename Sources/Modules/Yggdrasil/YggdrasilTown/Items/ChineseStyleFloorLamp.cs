namespace Everglow.Sources.Modules.YggdrasilModule.YggdrasilTown.Items
{
    public class ChineseStyleFloorLamp : ModItem
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 38;
            Item.rare = ItemRarityID.White;
            Item.scale = 1f;
            Item.createTile = ModContent.TileType<Tiles.ChineseStyleFloorLamp>();
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
