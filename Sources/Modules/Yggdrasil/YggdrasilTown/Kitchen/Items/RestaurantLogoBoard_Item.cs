using Everglow.Yggdrasil.YggdrasilTown.Kitchen.Tiles;

namespace Everglow.Yggdrasil.YggdrasilTown.Kitchen.Items;

public class RestaurantLogoBoard_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<RestaurantLogoBoard>());
		Item.width = 40;
		Item.height = 24;
		Item.value = 100;
	}
}