using Everglow.Yggdrasil.YggdrasilTown.Tiles.FurnaceTiles;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class TotalFurnaceScoreBoard_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<TotalFurnaceScoreBoard>());
	}
}