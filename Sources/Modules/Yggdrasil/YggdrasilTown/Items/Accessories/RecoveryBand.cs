namespace Everglow.Yggdrasil.YggdrasilTown.Items.Accessories;

public class RecoveryBand : ModItem
{
	public const int EffectTriggerDamageMin = 5;
	public const int LifeRecovery = 5;
	public const int ManaRecovery = 10;

	public override void SetDefaults()
	{
		Item.accessory = true;
		Item.width = 28;
		Item.height = 20;

		Item.value = Item.buyPrice(platinum: 0, gold: 1);
		Item.rare = ItemRarityID.Blue;
	}

	public override void UpdateAccessory(Player player, bool hideVisual)
	{
		// 1. On player hurt by anything, if damage received is more than 5,
		// then recover 5 life and 10 mana
		player.GetModPlayer<RecoveryBandPlayer>().RecoveryBandEnable = true;
	}
}

public class RecoveryBandPlayer : ModPlayer
{
	public bool RecoveryBandEnable { get; set; }

	public override void ResetEffects()
	{
		RecoveryBandEnable = false;
	}

	public override void OnHurt(Player.HurtInfo info)
	{
		if (RecoveryBandEnable && info.Damage >= RecoveryBand.EffectTriggerDamageMin)
		{
			var lifeCanHeal = Player.statLifeMax2 - Player.statLife;
			var lifeHealValue = lifeCanHeal > RecoveryBand.LifeRecovery ? RecoveryBand.LifeRecovery : lifeCanHeal;
			Player.statLife += lifeHealValue;
			CombatText.NewText(Player.getRect(), CombatText.HealLife, RecoveryBand.LifeRecovery, dramatic: true, dot: false);

			var manaCanHeal = Player.statManaMax2 - Player.statMana;
			var manaHealValue = manaCanHeal > RecoveryBand.ManaRecovery ? RecoveryBand.ManaRecovery : manaCanHeal;
			Player.statMana += manaHealValue;
			CombatText.NewText(Player.getRect(), CombatText.HealMana, RecoveryBand.ManaRecovery, dramatic: true, dot: false);
		}
	}
}