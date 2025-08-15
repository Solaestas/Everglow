using Everglow.Yggdrasil.YggdrasilTown.Kitchen.Tiles;
using Everglow.Yggdrasil.YggdrasilTown.Tiles;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class CanteenDoor_Item : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<CanteenDoor>());
        Item.width = 16;
        Item.height = 16;
    }
}