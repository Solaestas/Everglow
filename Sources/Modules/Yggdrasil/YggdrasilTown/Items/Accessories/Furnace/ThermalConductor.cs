namespace Everglow.Yggdrasil.YggdrasilTown.Items.Accessories.Furnace;

public class ThermalConductor : ModItem
{
	public const int EffectCheckFrameCount = 60;
	public const float StatusTriggerCondition = 0.2f;
	public const float StatusTriggerRate = 0.33f;
	public const int DebuffDuration = 180;
	public const int ManaHeal = 100;

	public override void SetDefaults()
	{
		Item.width = 44;
		Item.height = 46;
		Item.accessory = true;
		Item.rare = ItemRarityID.Blue;
		Item.value = Item.buyPrice(gold: 2);
	}

	public override void UpdateAccessory(Player player, bool hideVisual)
	{
		// 1. + 60 Mana
		// ============
		player.statManaMax2 += 60;

		// 2. + 10% Mana Cost
		// ==================
		player.manaCost += 0.1f;

		// 3. Mana regeneration with a penalty mechanic
		// ============================================
		player.GetModPlayer<ThermalConductorPlayer>().ThermalConductorEnable = true;
	}
}

internal class ThermalConductorPlayer : ModPlayer
{
	public bool ThermalConductorEnable { get; set; } = false;

	public override void ResetEffects()
	{
		ThermalConductorEnable = false;
	}

	// 3. Mana regeneration with a penalty mechanic
	// ============================================
	// When the player's mana falls below [StatusTriggerCondition],
	// there is a [StatusTriggerRate] chance of gaining [ManaHeal] mana
	// along with a Curse Inferno debuff with [DebuffDuration] duration.
	public override void PostUpdateMiscEffects()
	{
		if (ThermalConductorEnable)
		{
			if (Main.time % ThermalConductor.EffectCheckFrameCount == 0)
			{
				// statMana is more than trigger condition
				if (Player.statMana >= ThermalConductor.StatusTriggerCondition * Player.statManaMax2)
				{
					return;
				}

				// RNG
				if (Main.rand.NextFloat() >= ThermalConductor.StatusTriggerRate)
				{
					return;
				}

				// Heal Mana
				Player.HealMana(ThermalConductor.ManaHeal);

				// Add Debuff
				Player.AddBuff(BuffID.CursedInferno, ThermalConductor.DebuffDuration);
			}
		}
	}
}