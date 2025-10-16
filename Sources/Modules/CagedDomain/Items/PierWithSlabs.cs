namespace Everglow.CagedDomain.Items;

public class PierWithSlabs : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.PierWithSlabs>());
        Item.width = 26;
        Item.height = 26;
        Item.value = 2046;
    }
}
