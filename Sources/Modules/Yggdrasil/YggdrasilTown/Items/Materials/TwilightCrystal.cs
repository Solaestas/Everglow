namespace Everglow.Yggdrasil.YggdrasilTown.Items.Materials;

public class TwilightCrystal : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Materials;

    public override void SetDefaults()
    {
        Item.width = 16;
        Item.height = 16;
        Item.maxStack = Item.CommonMaxStack;
    }
}