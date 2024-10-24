namespace Everglow.Yggdrasil.YggdrasilTown.Items.Armors.Rock;

[AutoloadEquip(EquipType.Body)]
public class RockPlateMail : ModItem
{
	public const int MeleeDamageBonus = 5;

	public override void SetDefaults()
	{
		Item.width = 28;
		Item.height = 26;
		Item.value = Item.sellPrice(gold: 1, silver: 7);
		Item.rare = ItemRarityID.Green;
		Item.defense = 8;
	}

	public override void UpdateEquip(Player player)
	{
		player.GetDamage(DamageClass.Melee) += MeleeDamageBonus / 100f;
	}
}