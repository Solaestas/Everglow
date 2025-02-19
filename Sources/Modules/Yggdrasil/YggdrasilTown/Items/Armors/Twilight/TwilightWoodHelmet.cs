namespace Everglow.Yggdrasil.YggdrasilTown.Items.Armors.Twilight;

[AutoloadEquip(EquipType.Head)]
public class TwilightWoodHelmet : ModItem
{
	public const int DamageBonus = 7;
	public const int CritChanceBonus = 4;

	public const int ArmorSetDamageBonus = 3;
	public const int ArmorSetCritChanceBonus = 3;
	public const int ArmorSetSaveAmmoChance = 15;

	public override void SetDefaults()
	{
		Item.width = 28;
		Item.height = 26;
		Item.value = Item.buyPrice(silver: 37, copper: 50);
		Item.rare = ItemRarityID.White;
		Item.defense = 2;
	}

	public override void UpdateEquip(Player player)
	{
		player.GetDamage(DamageClass.Ranged) += DamageBonus / 100f;
		player.GetCritChance(DamageClass.Ranged) += CritChanceBonus;
	}

	public override bool IsArmorSet(Item head, Item body, Item legs)
	{
		return body.type == ModContent.ItemType<TwilightWoodBreastplate>() &&
			legs.type == ModContent.ItemType<TwilightWoodLeggings>();
	}

	public override void UpdateArmorSet(Player player)
	{
		player.GetDamage(DamageClass.Ranged) += ArmorSetDamageBonus / 100f;
		player.GetCritChance(DamageClass.Ranged) += ArmorSetCritChanceBonus;
		player.GetModPlayer<TwilightWoodArmorSetPlayer>().EnableTwilightWoodArmorSet = true;
	}
}

public class TwilightWoodArmorSetPlayer : ModPlayer
{
	public bool EnableTwilightWoodArmorSet { get; set; } = false;

	public override bool CanConsumeAmmo(Item weapon, Item ammo)
	{
		if (EnableTwilightWoodArmorSet)
		{
			if (Main.rand.NextFloat() <= TwilightWoodHelmet.ArmorSetSaveAmmoChance / 100f)
			{
				return false;
			}
		}
		return true;
	}
}