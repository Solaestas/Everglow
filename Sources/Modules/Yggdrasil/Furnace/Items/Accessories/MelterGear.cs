using Terraria.Enums;

namespace Everglow.Yggdrasil.Furnace.Items.Accessories;

public class MelterGear : ModItem
{
	public const int RangedAttackSpeedBonus = 9;
	public const int DefenseBonus = 2;

	public const float BuffTriggerRate = 0.33f;
	public const int BuffDuration = 300;

	public override void SetDefaults()
	{
		Item.width = 44;
		Item.height = 46;
		Item.accessory = true;
		Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(gold: 2));
	}

	public override void UpdateAccessory(Player player, bool hideVisual)
	{
		// 1. +9% Attack Spped
		player.GetAttackSpeed(DamageClass.Ranged) += RangedAttackSpeedBonus / 100f;

		// 2. +2 Defense
		player.statDefense += DefenseBonus;

		// 3. Hit Enhancement
		// Upon striking an enemy,
		// there is a 33% chance to inflict an "On Fire!" debuff on target, lasting for 5 seconds.
		// Additionally, while the target has "On Fire" debuff,
		// there is a 33% chance to inflict an "On Fire!" debuff on wearer, lasting for 5 seconds.
		// (The "On Fire" debuff inflicted by this attack will not trigger the second effect.)
		player.GetModPlayer<MelterGearPlayer>().MelterGearEnable = true;
	}
}

internal class MelterGearPlayer : ModPlayer
{
	public bool MelterGearEnable = false;

	public override void ResetEffects()
	{
		MelterGearEnable = false;
	}

	// 3. Hit Enhancement
	// Upon striking an enemy,
	// there is a 33% chance to inflict an "On Fire!" debuff on target, lasting for 5 seconds.
	// Additionally, while the target has "On Fire" debuff,
	// there is a 33% chance to inflict an "On Fire!" debuff on wearer, lasting for 5 seconds.
	// (The "On Fire" debuff inflicted by this attack will not trigger the second effect.)
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		if (MelterGearEnable)
		{
			if (target.onFire)
			{
				if (Main.rand.NextFloat() < MelterGear.BuffTriggerRate)
				{
					Player.AddBuff(BuffID.OnFire, MelterGear.BuffDuration);
				}
			}

			if (Main.rand.NextFloat() < MelterGear.BuffTriggerRate)
			{
				target.AddBuff(BuffID.OnFire, MelterGear.BuffDuration);
			}
		}
	}
}