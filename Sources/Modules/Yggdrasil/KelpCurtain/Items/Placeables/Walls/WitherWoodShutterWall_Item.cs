using Everglow.Yggdrasil.KelpCurtain.Walls;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Placeables.Walls;

public class WitherWoodShutterWall_Item : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableWall(ModContent.WallType<WitherWoodShutterWall>());
        Item.width = 24;
        Item.height = 24;
    }
}