using Terraria.GameContent.Creative;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Armors.Witherbark;

[AutoloadEquip(EquipType.Body)]
public class WitherbarkBreastPlate : ModItem
{
	public override void SetStaticDefaults()
	{
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}

	public override void SetDefaults()
	{
		Item.width = 26;
		Item.height = 28;

		Item.rare = ItemRarityID.Green;
		Item.value = Item.buyPrice(0, 0, 60, 0);

		Item.defense = 2;
	}

	public override void UpdateEquip(Player player)
	{
		player.whipRangeMultiplier += 0.2f;
	}
}