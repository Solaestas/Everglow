namespace Everglow.Yggdrasil.YggdrasilTown.Items.Materials;

public class BrokenMiningLamp : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Materials;

    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 32;

        Item.stack = 1;

        Item.rare = ItemRarityID.Green;
        Item.value = Item.buyPrice(gold: 1);
    }
}