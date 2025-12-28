using Everglow.CagedDomain.Tiles;

namespace Everglow.CagedDomain.Items;

public class ClothesShopBonsai_Feature_Nature_Item : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<WhiteCallaLily>());
        Item.width = 22;
        Item.height = 22;
    }

    public override void HoldItem(Player player)
    {
        Item.placeStyle = Math.Max(player.direction, 0);
        base.HoldItem(player);
    }
}