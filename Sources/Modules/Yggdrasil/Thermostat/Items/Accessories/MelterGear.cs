using Terraria.Enums;

namespace Everglow.Yggdrasil.Thermostat.Items.Accessories;

public class MelterGear : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 44;
		Item.height = 46;
		Item.accessory = true;
		Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(gold: 2));
	}

	public override void UpdateAccessory(Player player, bool hideVisual)
	{

	}
}