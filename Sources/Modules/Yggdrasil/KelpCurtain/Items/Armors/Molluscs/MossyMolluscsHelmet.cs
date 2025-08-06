using Everglow.Commons.Mechanics;
using Everglow.Yggdrasil.Common;
using Terraria.GameContent.Creative;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Armors.Molluscs;

[AutoloadEquip(EquipType.Head)]
public class MossyMolluscsHelmet : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Armor;

    public const float SaveAmmoChance = 0.15f;

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
        Item.defense = 2;
    }

    override public void UpdateEquip(Player player)
    {
        player.GetDamage<RangedDamageClass>() += 0.04f; // Increases ranged damage by 4%.
        player.GetCritChance<RangedDamageClass>() += 6; // Increase ranged critical chance by 6%.
    }

    public override bool IsArmorSet(Item head, Item body, Item legs)
    {
        return body.type == ModContent.ItemType<ShellMolluscsBreastPlate>() && legs.type == ModContent.ItemType<MolluscsLeggings>();
    }

    public override void UpdateArmorSet(Player player)
    {
        player.armorPenetration += 3; // Increases armor penetration by 3.
        player.GetModPlayer<EverglowPlayer>().ammoCost *= 1 - SaveAmmoChance;
    }
}