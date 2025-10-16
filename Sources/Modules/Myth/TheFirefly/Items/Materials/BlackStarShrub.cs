namespace Everglow.Myth.TheFirefly.Items.Materials;

public class BlackStarShrub : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Materials;

    public override void SetDefaults()
    {
        Item.width = 32;
        Item.height = 24;
        Item.maxStack = 999;
        Item.value = 100;
        Item.rare = ItemRarityID.White;
    }
}