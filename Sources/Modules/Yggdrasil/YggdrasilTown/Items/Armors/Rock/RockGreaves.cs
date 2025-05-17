namespace Everglow.Yggdrasil.YggdrasilTown.Items.Armors.Rock;

[AutoloadEquip(EquipType.Legs)]
public class RockGreaves : ModItem
{
	private const int MoveSpeedBonus = 5;

	public override void SetDefaults()
	{
		Item.width = 28;
		Item.height = 26;
		Item.value = Item.buyPrice(silver: 94);
		Item.rare = ItemRarityID.Green;
		Item.defense = 6;
	}

	public override void UpdateEquip(Player player)
	{
		player.moveSpeed += MoveSpeedBonus / 100f;
	}
}