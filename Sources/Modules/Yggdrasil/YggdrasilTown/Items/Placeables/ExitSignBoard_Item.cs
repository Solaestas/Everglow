using Everglow.Yggdrasil.YggdrasilTown.Tiles;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class ExitSignBoard_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Exit_Sign_Board>());
		Item.width = 22;
		Item.height = 10;
		Item.value = 0;
	}
}