namespace Everglow.Yggdrasil.YggdrasilTown.Items.Accessories;

public class CelesteStoneWaistPendant : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Accessories;

    public override void SetDefaults()
    {
        Item.width = 24;
        Item.height = 44;
        Item.accessory = true;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.buyPrice(silver: 92);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        // 1. + 30 Mana
        player.statManaMax2 += 30;

        // 2. + 2 mana regen (if mana is between 40% and 60%)
        if (player.statMana > player.statManaMax * 0.4f && player.statMana < player.statManaMax * 0.6f)
        {
            player.manaRegen += 2;
        }
    }
}