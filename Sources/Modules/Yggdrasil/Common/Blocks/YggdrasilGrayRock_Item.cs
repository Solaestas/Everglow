namespace Everglow.Yggdrasil.Common.Blocks;
public class YggdrasilGrayRock_Item : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<YggdrasilGrayRock>());
        Item.width = 16;
        Item.height = 16;
    }
}
