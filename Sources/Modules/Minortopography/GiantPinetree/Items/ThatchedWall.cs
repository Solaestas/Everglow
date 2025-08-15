namespace Everglow.Minortopography.GiantPinetree.Items;

public class ThatchedWall : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableWall(ModContent.WallType<TilesAndWalls.ThatchedWall>());
        Item.width = 20;
        Item.height = 20;
    }
}