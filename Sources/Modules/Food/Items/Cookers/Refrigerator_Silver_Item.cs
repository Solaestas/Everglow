using Everglow.Food.Tiles;

namespace Everglow.Food.Items.Cookers;

public class Refrigerator_Silver_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Refrigerator_Silver>());
		Item.value = 1000;
	}
}