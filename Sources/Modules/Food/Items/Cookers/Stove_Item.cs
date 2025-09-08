using Everglow.Food.Tiles;

namespace Everglow.Food.Items.Cookers;

public class Stove_Item : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<Stove>());
        Item.width = 68;
        Item.height = 48;
        Item.value = 1000;
    }
}