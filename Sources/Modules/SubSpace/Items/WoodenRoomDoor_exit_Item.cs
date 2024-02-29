using Everglow.SubSpace.Tiles;
using SubworldLibrary;

namespace Everglow.SubSpace.Items;
public class WoodenRoomDoor_exit_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<WoodenRoomDoor_exit>());
		Item.width = 50;
		Item.height = 46;
	}
	public override bool CanUseItem(Player player)
	{
		return SubworldSystem.IsActive<RoomWorld>();
	}
}
