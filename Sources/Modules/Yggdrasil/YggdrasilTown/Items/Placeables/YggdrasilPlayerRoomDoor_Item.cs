using Everglow.Yggdrasil.YggdrasilTown.Tiles;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class YggdrasilPlayerRoomDoor_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<YggdrasilPlayerRoomDoor>());
		Item.width = 50;
		Item.height = 46;
	}
}