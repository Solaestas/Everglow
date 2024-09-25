namespace Everglow.Yggdrasil.YggdrasilTown.Items.Armors.Twilight;

[AutoloadEquip(EquipType.Body)]
public class TwilightWoodBreastplate : ModItem
{
	public const int MaxLifeBonus = 15;

	public override void SetDefaults()
	{
		Item.width = 28;
		Item.height = 26;
		Item.value = Item.sellPrice(silver: 37, copper: 50);
		Item.rare = ItemRarityID.White;
		Item.defense = 4;
	}

	public override void UpdateEquip(Player player)
	{
		player.statLifeMax2 += MaxLifeBonus;
	}
}