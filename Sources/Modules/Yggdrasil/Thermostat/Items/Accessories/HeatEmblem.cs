using Terraria.Enums;

namespace Everglow.Yggdrasil.Thermostat.Items.Accessories;

public class HeatEmblem : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 44;
		Item.height = 46;
		Item.accessory = true;
		Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(gold: 1, silver: 50));
	}

	public override void UpdateAccessory(Player player, bool hideVisual)
	{
		
	}
}