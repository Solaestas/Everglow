using Everglow.Yggdrasil.Common.Elevator.Tiles;

namespace Everglow.Yggdrasil.Common.Elevator.Items;

public class YggdrasilTownElevator_Item : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public override void SetDefaults()
    {
		Item.DefaultToPlaceableTile(ModContent.TileType<YggdrasilTownElevator_Winch>());
		Item.width = 42;
		Item.height = 64;
		Item.value = 20000;
    }
}