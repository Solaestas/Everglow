namespace Everglow.Myth.TheFirefly.Items;

public class GlowWood : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.FireflyWood>());
        Item.width = 16;
        Item.height = 16;
    }
}