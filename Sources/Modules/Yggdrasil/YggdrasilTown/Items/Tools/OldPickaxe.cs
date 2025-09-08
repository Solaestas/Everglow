namespace Everglow.Yggdrasil.YggdrasilTown.Items.Tools;

public class OldPickaxe : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Tools;

    public override void SetDefaults()
    {
        Item.width = 22;
        Item.height = 22;
        Item.pick = 40;

        Item.useStyle = ItemUseStyleID.Swing;
        Item.UseSound = SoundID.Item1;
        Item.autoReuse = true;
        Item.useTime = Item.useAnimation = 14;
        Item.tileBoost = 1;
        Item.useTurn = true;

        Item.DamageType = DamageClass.Melee;
        Item.damage = 6;
        Item.knockBack = 2f;

        Item.rare = ItemRarityID.Green;
        Item.value = Item.buyPrice(platinum: 0, gold: 1);
    }
}