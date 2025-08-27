using Terraria.GameContent.Creative;

namespace Everglow.Commons.Weapons.Yoyos;

public abstract class YoyoItem : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.MeleeWeapons;

    public override void SetStaticDefaults()
    {
        ItemID.Sets.Yoyo[Item.type] = true;
        ItemID.Sets.GamepadExtraRange[Item.type] = 15;
        ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    }
    public virtual void SetDef()
    {

    }
    public override void SetDefaults()
    {
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.width = 24;
        Item.height = 24;
        Item.noUseGraphic = true;
        Item.UseSound = SoundID.Item1;
        Item.DamageType = DamageClass.Melee;
        Item.channel = true;
        Item.useAnimation = 5;
        Item.useTime = 5;
        Item.shootSpeed = 0f;
        Item.knockBack = 0.2f;
        Item.noMelee = true;

        ItemID.Sets.Yoyo[Item.type] = true;
        SetDef();
    }
}
