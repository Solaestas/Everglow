namespace Everglow.Myth.TheFirefly.Items;

public class LiveFluorescentLeafWall : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableWall(ModContent.WallType<Walls.LiveFluorescentLeafWall>());
        Item.width = 24;
        Item.height = 24;
    }
}