namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class JellyBallCube : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.JellyBallSecretion>());
        Item.width = 22;
        Item.height = 22;
        Item.value = 60;
        Item.maxStack = Item.CommonMaxStack;
    }
}