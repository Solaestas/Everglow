namespace Everglow.Yggdrasil.YggdrasilTown.Items.Materials;

public class CaterpillarJuice : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Materials;

    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 32;
        Item.value = 100;
        Item.maxStack = Item.CommonMaxStack;
    }
}