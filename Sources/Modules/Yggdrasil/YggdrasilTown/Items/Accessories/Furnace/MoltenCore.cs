using Terraria.Enums;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Accessories.Furnace;

public class MoltenCore : ModItem
{
	private const float OnePercent = 1f;
	private const int DamageReduction = 5;
	private const int DefenseBonus = 5;
	private const int CritBonus = 5;
	private const int DamageBonus = 5;
	private const int MoveSpeedBonus = 5;
	private const int JumpSpeedBonus = 5;

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
			player.endurance = OnePercent - ((OnePercent - player.endurance) * DamageReduction / 100f);
			player.statDefense += DefenseBonus;
			player.GetCritChance(DamageClass.Generic) += CritBonus;
			player.GetDamage(DamageClass.Generic) += DamageBonus / 100f;
			player.moveSpeed += MoveSpeedBonus / 100f;
			player.jumpSpeedBoost += JumpSpeedBonus / 100f;
		}
	}
}