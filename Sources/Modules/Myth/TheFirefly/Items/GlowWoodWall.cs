namespace Everglow.Myth.TheFirefly.Items;

public class GlowWoodWall : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableWall(ModContent.WallType<Walls.GlowWoodWall>());
        Item.width = 24;
        Item.height = 24;
    }
}