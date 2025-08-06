using Everglow.Yggdrasil.KelpCurtain.Buffs;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Accessories;

public class LivingWoodplate : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Accessories;

    public const int BuffDuration = 30 * 60;

    public override void SetDefaults()
    {
        Item.width = 32;
        Item.height = 22;

        Item.accessory = true;
        Item.defense = 4;

        Item.rare = ItemRarityID.Green;
        Item.value = Item.buyPrice(silver: 30);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        if (player.wet)
        {
            player.AddBuff(ModContent.BuffType<LivingWoodplateBuff>(), BuffDuration);
        }
    }
}