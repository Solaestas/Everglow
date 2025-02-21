using Everglow.Food.Tiles;

namespace Everglow.Food.Items.Cookers;

public class SteamBox_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.maxStack = Item.CommonMaxStack;
		Item.width = 32;
		Item.height = 40;
		Item.value = 1000;
	}
}