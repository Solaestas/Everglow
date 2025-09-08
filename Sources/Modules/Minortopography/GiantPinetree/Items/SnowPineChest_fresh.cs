namespace Everglow.Minortopography.GiantPinetree.Items;

public class SnowPineChest_fresh : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<TilesAndWalls.SnowPineChest_fresh>());
        Item.width = 32;
        Item.height = 30;
        Item.value = 4040;
    }
}