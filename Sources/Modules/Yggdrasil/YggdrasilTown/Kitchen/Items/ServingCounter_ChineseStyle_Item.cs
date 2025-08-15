using Everglow.Yggdrasil.YggdrasilTown.Kitchen.Tiles;

namespace Everglow.Yggdrasil.YggdrasilTown.Kitchen.Items;

public class ServingCounter_ChineseStyle_Item : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<ServingCounter_ChineseStyle>());
        Item.width = 24;
        Item.height = 40;
        Item.value = 1000;
    }
}