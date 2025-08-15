namespace Everglow.Minortopography.GiantPinetree.Items;

public class SnowPineLeavesWall : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableWall(ModContent.WallType<TilesAndWalls.PineLeavesWall>());
        Item.width = 20;
        Item.height = 20;
    }
}