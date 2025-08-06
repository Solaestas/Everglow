namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class StoneDragonScaleWood : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.StoneScaleWood>());
        Item.width = 38;
        Item.height = 30;
    }
}
