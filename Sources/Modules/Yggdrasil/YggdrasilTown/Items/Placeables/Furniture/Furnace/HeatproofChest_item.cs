using Everglow.Commons.ItemAbstracts.Furniture;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables.Furniture.Furnace;

public class HeatproofChest_item : ChestItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.FurnaceTiles.HeatproofChest>());
		base.SetDefaults();
	}
}