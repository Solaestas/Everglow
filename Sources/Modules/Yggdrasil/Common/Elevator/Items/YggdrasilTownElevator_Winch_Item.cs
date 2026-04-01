using Everglow.Yggdrasil.Common.Elevator.Tiles;

namespace Everglow.Yggdrasil.Common.Elevator.Items;

public class YggdrasilTownElevator_Winch_Item : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public override void SetDefaults()
    {
        Item.width = 38;
        Item.height = 16;
        Item.createTile = ModContent.TileType<YggdrasilTownElevator_Winch>();
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useAnimation = 15;
        Item.useTime = 15;
        Item.consumable = true;
        Item.maxStack = Item.CommonMaxStack;
        Item.value = 1000;
    }
}