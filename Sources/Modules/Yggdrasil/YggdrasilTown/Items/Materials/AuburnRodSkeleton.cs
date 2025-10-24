namespace Everglow.Yggdrasil.YggdrasilTown.Items.Materials;

public class AuburnRodSkeleton : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Materials;

    public override void SetDefaults()
    {
        Item.width = 50;
        Item.height = 38;
        Item.value = 3000;
        Item.maxStack = Item.CommonMaxStack;
    }
}