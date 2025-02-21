using Everglow.Yggdrasil.YggdrasilTown.Kitchen.Tiles;

namespace Everglow.Yggdrasil.YggdrasilTown.Kitchen.Items;

public class ChineseKitchenDoor_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<ChineseKitchenDoor>());
		Item.width = 50;
		Item.height = 46;
	}
}