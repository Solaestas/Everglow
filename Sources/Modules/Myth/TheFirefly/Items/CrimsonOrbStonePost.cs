namespace Everglow.Myth.TheFirefly.Items;

public class CrimsonOrbStonePost : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.CrimsonOrbStonePost>());
        Item.width = 18;
        Item.height = 34;
    }
}