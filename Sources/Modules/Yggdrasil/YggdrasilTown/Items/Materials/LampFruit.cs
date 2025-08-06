namespace Everglow.Yggdrasil.YggdrasilTown.Items.Materials;

public class LampFruit : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Materials;

    public override void SetDefaults()
    {
        Item.damage = 2;
        Item.ammo = Item.type;
        Item.consumable = true;
        Item.DamageType = DamageClass.Ranged;
        Item.width = 24;
        Item.height = 24;
        Item.value = 50;
        Item.maxStack = Item.CommonMaxStack;
    }
}