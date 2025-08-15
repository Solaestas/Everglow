namespace Everglow.Yggdrasil.KelpCurtain.Items.Placeables;

public class MossProneSandSoil : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public override void SetStaticDefaults()
    {
    }
    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.MossProneSandSoil>());
    }
}
