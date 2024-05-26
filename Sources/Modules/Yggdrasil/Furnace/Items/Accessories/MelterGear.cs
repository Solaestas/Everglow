using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.Social.Base;

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
	}
}

internal class MelterGearPlayer : ModPlayer
{
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
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