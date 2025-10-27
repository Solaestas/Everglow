namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class DilapidatedDangerSigns : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.DilapidatedDangerSigns4x3>());
        Item.width = 32;
        Item.height = 22;
        Item.value = 0;
    }
}