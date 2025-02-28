using Everglow.Food.Tiles;

namespace Everglow.Food.Items.Cookers;

public class CookwareRack_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<CookwareRack>());
		Item.width = 22;
		Item.height = 18;
		Item.value = 1000;
	}
}