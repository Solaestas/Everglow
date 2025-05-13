using Everglow.Yggdrasil.YggdrasilTown.Tiles.FurnaceTiles;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class FloorScaleForPlayers_Item : ModItem
{
	public int State = 0;

	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<FloorScaleForPlayers>());
		Item.width = 34;
		Item.height = 20;
	}
}