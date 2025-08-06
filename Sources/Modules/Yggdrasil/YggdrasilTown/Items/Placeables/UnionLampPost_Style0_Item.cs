namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class UnionLampPost_Style0_Item : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.UnionLampPost_Style0>());
        Item.width = 18;
        Item.height = 38;
        Item.value = 1000;
    }
}