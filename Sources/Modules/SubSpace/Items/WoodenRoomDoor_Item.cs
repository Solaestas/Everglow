using Everglow.SubSpace.Tiles;

namespace Everglow.SubSpace.Items;
public class WoodenRoomDoor_Item : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<WoodenRoomDoor>());
        Item.width = 50;
        Item.height = 46;
    }
}
