using Terraria.ID;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Placeables;

public class DragonScaleWood : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.DragonScaleWood>());
        Item.width = 24;
        Item.height = 22;
        Item.rare = ItemRarityID.White;
    }
}
