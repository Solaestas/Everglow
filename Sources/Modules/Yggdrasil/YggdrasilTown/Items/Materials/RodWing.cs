namespace Everglow.Yggdrasil.YggdrasilTown.Items.Materials;

public class RodWing : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Materials;

    public override void SetDefaults()
    {
        Item.width = 30;
        Item.height = 22;
        Item.value = 100;
        Item.maxStack = Item.CommonMaxStack;
    }
}