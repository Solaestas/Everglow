using Everglow.Food.Tiles;

namespace Everglow.Food.Items;

public class Stove_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Stove>());
		Item.width = 68;
		Item.height = 48;
		Item.value = 1000;
	}
}