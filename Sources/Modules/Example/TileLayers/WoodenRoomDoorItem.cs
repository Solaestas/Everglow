namespace Everglow.Example.TileLayers;

public class WoodenRoomDoorItem : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<WoodenRoomDoor>());
		Item.width = 20;
		Item.height = 18;
	}
}