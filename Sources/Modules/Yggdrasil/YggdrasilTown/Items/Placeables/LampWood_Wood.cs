namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class LampWood_Wood : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.LampWood.LampWood_Wood_Tile>());
    }
}
