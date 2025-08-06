using Everglow.Commons.ItemAbstracts.Furniture;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables.Furniture.Furnace;

public class HeatproofTable_item : TableItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.FurnaceTiles.HeatproofTable>());
		base.SetDefaults();
	}
}