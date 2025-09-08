namespace Everglow.Minortopography.GiantPinetree.Items;

public class SnowPineWoodWall : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableWall(ModContent.WallType<TilesAndWalls.PineWoodWall>());
        Item.width = 20;
        Item.height = 20;
    }
}