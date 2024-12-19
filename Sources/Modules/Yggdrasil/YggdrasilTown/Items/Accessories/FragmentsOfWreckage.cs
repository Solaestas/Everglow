namespace Everglow.Yggdrasil.YggdrasilTown.Items.Accessories;

public class FragmentsOfWreckage : ModItem
{
	public const float LifeLossForBonusMax = 400f;
	public const float DamageBonus = 0.1f;
	public const float MeleeSpeedBonus = 0.1f;
	public const float PickSpeedBonus = 0.15f;
	public const int DefenseBonus = 12;

	public override void SetDefaults()
	{
		Item.accessory = true;
		Item.width = 28;
		Item.height = 20;

		Item.rare = ItemRarityID.Green;
		Item.value = Item.buyPrice(platinum: 0, gold: 1);
	}

	public override void UpdateAccessory(Player player, bool hideVisual)
	{
		// 1. Slightly increases stats based on life loss
		// ==============================================
		var lifeLoss = player.statLifeMax2 - player.statLife;

		// A maximum of 400 life loss will be counted
		var bonusScaler = lifeLoss / LifeLossForBonusMax;
		if (bonusScaler > 1)
		{
			bonusScaler = 1;
		}
		else if (bonusScaler < 0)
		{
			bonusScaler = 0;
		}

		// +10% damage
		player.GetDamage(DamageClass.Generic) += DamageBonus * bonusScaler;

		// +10% melee speed
		player.meleeSpeed += MeleeSpeedBonus * bonusScaler;

		// +12 defense
		player.statDefense += (int)(DefenseBonus * bonusScaler);

		// +15% pick speed
		player.pickSpeed -= PickSpeedBonus * bonusScaler;
	}
}