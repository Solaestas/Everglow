using Everglow.Food.Tiles;

namespace Everglow.Food.Items.Cookers;

public class Hob_Silver_Item : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<Hob_Silver>());
        Item.value = 1000;
    }
}