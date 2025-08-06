namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons.TwilightForest;

public class TwilightEucalyptusCuffSickle : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.MeleeWeapons;

    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 28;

        Item.DamageType = DamageClass.Melee;
        Item.damage = 30;
        Item.knockBack = 0.4f;
        Item.crit = 4;

        Item.useTime = Item.useAnimation = 6;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.UseSound = SoundID.Item1;
        Item.autoReuse = true;
    }
}