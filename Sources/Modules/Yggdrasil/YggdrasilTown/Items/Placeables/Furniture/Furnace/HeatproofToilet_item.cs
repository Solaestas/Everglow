using Everglow.Commons.Templates.Furniture;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables.Furniture.Furnace;

public class HeatproofToilet_item : ToiletItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.FurnaceTiles.HeatproofToilet>());
		base.SetDefaults();
	}
}