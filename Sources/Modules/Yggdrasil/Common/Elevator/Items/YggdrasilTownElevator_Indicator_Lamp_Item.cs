using Everglow.Yggdrasil.Common.Elevator.Tiles;

namespace Everglow.Yggdrasil.Common.Elevator.Items;

public class YggdrasilTownElevator_Indicator_Lamp_Item : ModItem
{
	public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<YggdrasilTownElevator_Indicator_Lamp>());
		Item.width = 20;
		Item.height = 36;
		Item.value = 1000;
	}
}