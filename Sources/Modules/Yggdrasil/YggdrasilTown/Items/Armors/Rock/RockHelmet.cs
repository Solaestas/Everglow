namespace Everglow.Yggdrasil.YggdrasilTown.Items.Armors.Rock;

[AutoloadEquip(EquipType.Head)]
public class RockHelmet : ModItem
{
	public const int MeleeCritChanceBonus = 5;

	public const int DistanceRequirementPerDefense = 333;

	public override void SetDefaults()
	{
		Item.width = 28;
		Item.height = 26;
		Item.value = Item.sellPrice(silver: 95);
		Item.rare = ItemRarityID.Green;
		Item.defense = 6;
	}

	public override void UpdateEquip(Player player)
	{
		player.GetCritChance(DamageClass.Melee) += MeleeCritChanceBonus;
	}

	public override bool IsArmorSet(Item head, Item body, Item legs)
	{
		return body.type == ModContent.ItemType<RockPlateMail>() && legs.type == ModContent.ItemType<RockGreaves>();
	}

	public override void UpdateArmorSet(Player player)
	{
		player.GetModPlayer<RockArmorSetPlayer>().EnableRockArmorSet = true;
	}
}

public class RockArmorSetPlayer : ModPlayer
{
	public bool EnableRockArmorSet { get; set; } = false;

	private int MoveDistance { get; set; } = 0;

	public int DefenseBonus => MoveDistance / RockHelmet.DistanceRequirementPerDefense <= 12
			? MoveDistance / RockHelmet.DistanceRequirementPerDefense
			: 12;

	public override void PostUpdate()
	{
		MoveDistance += (int)(Player.position - Player.oldPosition).Length();
		Player.statDefense += DefenseBonus;
	}

	public override void OnHurt(Player.HurtInfo info)
	{
		MoveDistance = 0;
	}
}