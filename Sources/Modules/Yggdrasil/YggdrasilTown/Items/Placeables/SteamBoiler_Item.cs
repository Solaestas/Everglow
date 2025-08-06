using Everglow.Yggdrasil.YggdrasilTown.Tiles.FurnaceTiles;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class SteamBoiler_Item : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public int State = 0;

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<SteamBoiler>());
        Item.width = 36;
        Item.height = 28;
    }
}