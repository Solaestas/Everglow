namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class FemaleLampLeaves_Item : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.LampWood.FemaleLampLeaves>());
        Item.width = 16;
        Item.height = 16;
    }
}
