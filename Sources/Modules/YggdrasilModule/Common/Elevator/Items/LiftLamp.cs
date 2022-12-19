namespace Everglow.Sources.Modules.YggdrasilModule.Common.Elevator.Items
{
    public class LiftLamp : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 36;
            Item.createTile = ModContent.TileType<Tiles.LiftLamp>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.consumable = true;
            Item.maxStack = 999;
            Item.value = 1000;
        }
    }
}
