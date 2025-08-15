using Terraria.GameContent.Creative;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Armors.Molluscs;

[AutoloadEquip(EquipType.Head)]
public class PearlMolluscsHelmet : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Armor;

    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    }

    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 20;
        Item.value = Item.buyPrice(silver: 60);
        Item.rare = ItemRarityID.Green;
        Item.defense = 5;
    }

    override public void UpdateEquip(Player player)
    {
        player.GetDamage<MeleeDamageClass>() += 0.04f; // // Increases melee damage by 4%.
        player.GetCritChance<MeleeDamageClass>() += 4; // Increase melee critical chance by 4%.
    }

    public override bool IsArmorSet(Item head, Item body, Item legs)
    {
        return body.type == ModContent.ItemType<ShellMolluscsBreastPlate>() && legs.type == ModContent.ItemType<MolluscsLeggings>();
    }

    public override void UpdateArmorSet(Player player)
    {
        player.lifeRegen += 2; // Increases life regeneration by 2.
        player.GetAttackSpeed<MeleeDamageClass>() += 0.1f; // Increases melee attack speed by 10%.
    }
}