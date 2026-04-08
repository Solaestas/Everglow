using Everglow.Minortopography.GiantPinetree.TilesAndWalls.Elevator;

namespace Everglow.Minortopography.GiantPinetree.Items;

public class GiantPineTreeElevator_Item : ModItem
{
	public override string LocalizationCategory => Commons.Utilities.LocalizationUtils.Categories.Placeables;

	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<GiantPineTreeElevator_Winch>());
		Item.width = 42;
		Item.height = 64;
		Item.value = 20000;
	}
}