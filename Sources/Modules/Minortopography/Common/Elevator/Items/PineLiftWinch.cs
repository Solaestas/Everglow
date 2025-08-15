namespace Everglow.Minortopography.Common.Elevator.Items;

public class PineLiftWinch : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public override void SetDefaults()
    {
        Item.width = 38;
        Item.height = 16;
        Item.createTile = ModContent.TileType<Tiles.PineWinch>();
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useAnimation = 15;
        Item.useTime = 15;
        Item.consumable = true;
        Item.maxStack = 999;
        Item.value = 1000;
    }
}
