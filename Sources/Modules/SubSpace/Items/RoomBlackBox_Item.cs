using Everglow.SubSpace.Tiles;
using SubworldLibrary;

namespace Everglow.SubSpace.Items;

public class RoomBlackBox_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<RoomBlackBox>());
		Item.width = 24;
		Item.height = 24;
	}
}