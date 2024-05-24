using Terraria.Enums;

namespace Everglow.Yggdrasil.Furnace.Items.Accessories;

public class ThermalConductor : ModItem
{
	public static readonly double StatusTriggerCondition = 0.2d;
	public static readonly double StatusTriggerRate = 0.33d;
	public static readonly int DebuffDuration = 300;
	public static readonly int ManaHeal = 100;

	public override void SetDefaults()
	{
		Item.width = 44;
		Item.height = 46;
		Item.accessory = true;
		Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(gold: 2));
	}

	public override void UpdateAccessory(Player player, bool hideVisual)
	{
		// 1. + 60 Mana
		player.statManaMax2 += 60;

		// 2. + 10% Mana Cost
		player.manaCost += 0.1f;

		// 3. When the player's mana falls below [StatusTriggerCondition],
		// there is a [StatusTriggerRate] chance of gaining [ManaHeal] mana
		// along with a Curse Inferno debuff with [DebuffDuration] duration.
		player.GetModPlayer<ThermalConductorPlayer>();
	}
}

public class ThermalConductorPlayer : ModPlayer
{
	public override void PreUpdate()
	{
		if (Main.rand.NextBool(60))
		{
			// statMana is more than trigger condition
			if (Player.statMana >= ThermalConductor.StatusTriggerCondition * Player.statManaMax2)
			{
				return;
			}

			// Random
			Random random = new Random();
			if (random.NextDouble() >= ThermalConductor.StatusTriggerRate)
			{
				return;
			}

			// Heal Mana
			Player.statMana = Player.statMana + ThermalConductor.ManaHeal > Player.statManaMax2 ?
					Player.statManaMax2 :
					Player.statMana + ThermalConductor.ManaHeal;

			// Add Debuff
			Player.AddBuff(BuffID.CursedInferno, ThermalConductor.DebuffDuration);
		}
	}
}