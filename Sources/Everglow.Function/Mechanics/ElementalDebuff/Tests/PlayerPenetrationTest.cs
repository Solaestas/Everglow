using Everglow.Commons.Utilities;

namespace Everglow.Commons.Mechanics.ElementalDebuff.Tests;

public class PlayerPenetrationTest : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Accessories;

    public override string Texture => ModAsset.Point_Mod;

    public override void SetDefaults()
    {
        Item.width = 32;
        Item.height = 32;

        Item.accessory = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetElementalPenetration(ElementalDebuffType.Generic) += 0.1f;
        player.GetElementalPenetration(ElementalDebuffType.Burn) += 0.05f;
    }
}