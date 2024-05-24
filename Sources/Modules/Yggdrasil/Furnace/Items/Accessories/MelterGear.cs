using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.Social.Base;

namespace Everglow.Yggdrasil.Furnace.Items.Accessories;

public class MelterGear : ModItem
{
	public static readonly int RangedAttackSpeedBonus = 9;
	public static readonly int DefenseBonus = 2;

	public static readonly double BuffTriggerRate = 0.33d;
	public static readonly int BuffDuration = 300;

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
		player.GetModPlayer<MelterGearPlayer>();
	}
}

internal class MelterGearPlayer : ModPlayer
{
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		Random random = new();

		if (target.onFire)
		{
			var playerRate = random.NextDouble();
			if (playerRate < MelterGear.BuffTriggerRate)
			{
				Player.AddBuff(BuffID.OnFire, MelterGear.BuffDuration);
			}
		}

		var targetRate = random.NextDouble();
		if (targetRate < MelterGear.BuffTriggerRate)
		{
			target.AddBuff(BuffID.OnFire, MelterGear.BuffDuration);
		}

		base.OnHitNPC(target, hit, damageDone);
	}
}