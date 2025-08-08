namespace Everglow.Myth.TheFirefly.Items;

public class LiveFluorescentWoodWall : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableWall(ModContent.WallType<Walls.LiveFluorescentWoodWall>());
        Item.width = 24;
        Item.height = 24;
    }
}