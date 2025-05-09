using Everglow.SubSpace.Tiles;
using Everglow.Yggdrasil.YggdrasilTown.Tiles;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class YggdrasilPlayerRoomDoor_Lock_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<YggdrasilPlayerRoomDoor_Lock>());
		Item.width = 50;
		Item.height = 46;
	}
}