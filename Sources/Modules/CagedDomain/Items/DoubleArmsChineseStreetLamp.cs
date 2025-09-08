namespace Everglow.CagedDomain.Items;

public class DoubleArmsChineseStreetLamp : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.DoubleArmsChineseStreetLamp>());
        Item.width = 22;
        Item.height = 26;
        Item.value = 1000;
    }
}
