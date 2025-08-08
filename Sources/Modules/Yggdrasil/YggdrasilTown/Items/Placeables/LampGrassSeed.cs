namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class LampGrassSeed : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public override void SetDefaults()
    {
        Item.width = 18;
        Item.height = 14;
    }
}