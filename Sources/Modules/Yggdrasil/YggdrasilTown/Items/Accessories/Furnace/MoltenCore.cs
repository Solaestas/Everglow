using Terraria.Enums;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Accessories.Furnace;

public class MoltenCore : ModItem
{
	public const float OnePercent = 1f;
	public const float DamageReduction = 0.05f;
	public const int DefenseBonus = 5;
	public const int CritBonus = 5;
	public const float DamageBonus = 0.05f;
	public const float MoveSpeedBonus = 0.05f;
	public const float JumpSpeedBonus = 0.05f;

	public override void SetDefaults()
	{
		Item.width = 44;
		Item.height = 46;
		Item.accessory = true;

		// TODO: Add new rarity color: lime
		Item.SetShopValues(ItemRarityColor.Cyan9, Item.buyPrice(gold: 10));
	}

	public override void UpdateAccessory(Player player, bool hideVisual)
	{
		// 1. Increase abilities when player has some debuffs
		// ==================================================
		// Including: OnFire, OnFire3, CursedInferno
		// Increasing 5% damage receive, 5 def, 5% crit, 5% damage, 5% speed, 5% jump speed
		if (player.HasBuff(BuffID.OnFire) || player.HasBuff(BuffID.OnFire3) || player.HasBuff(BuffID.CursedInferno))
		{
			player.endurance = OnePercent - (DamageReduction * (OnePercent - player.endurance));
			player.statDefense += DefenseBonus;
			player.GetCritChance(DamageClass.Generic) += CritBonus;
			player.GetDamage(DamageClass.Generic) += DamageBonus;
			player.moveSpeed *= OnePercent + MoveSpeedBonus;
			player.jumpSpeedBoost += JumpSpeedBonus;
		}
	}
}