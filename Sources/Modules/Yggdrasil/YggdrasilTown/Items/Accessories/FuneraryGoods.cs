namespace Everglow.Yggdrasil.YggdrasilTown.Items.Accessories;

public class FuneraryGoods : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Accessories;

    public const float EffectChance = 0.125f;
    public const int ManaRecovery = 100;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.width = 18;
        Item.height = 22;

        Item.value = Item.buyPrice(platinum: 0, gold: 1);
        Item.rare = ItemRarityID.Blue;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        // 1. On kill enemy, has a 12.5% chance to recover 100 mana immediately
        // ====================================================================
        player.GetModPlayer<FuneraryGoodsPlayer>().FuneraryGoodsEnable = true;
    }
}

public class FuneraryGoodsPlayer : ModPlayer
{
    public bool FuneraryGoodsEnable { get; set; }

    public override void ResetEffects()
    {
        FuneraryGoodsEnable = false;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (FuneraryGoodsEnable && !target.active)
        {
            if (Main.rand.NextFloat() <= FuneraryGoods.EffectChance)
            {
                Player.HealMana(FuneraryGoods.ManaRecovery);
            }
        }
    }
}