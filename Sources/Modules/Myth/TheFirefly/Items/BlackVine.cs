namespace Everglow.Myth.TheFirefly.Items;

public class BlackVine : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.BlackVine>());
        Item.width = 30;
        Item.height = 30;
    }
}