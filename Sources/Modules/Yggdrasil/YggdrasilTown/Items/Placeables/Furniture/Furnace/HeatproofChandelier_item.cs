using Everglow.Commons.ItemAbstracts.Furniture;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables.Furniture.Furnace;

public class HeatproofChandelier_item : ChandelierItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.FurnaceTiles.HeatproofChandelier>());
		base.SetDefaults();
	}
}