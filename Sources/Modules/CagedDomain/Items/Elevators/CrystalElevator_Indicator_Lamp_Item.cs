using Everglow.CagedDomain.Tiles.Elevators;

namespace Everglow.CagedDomain.Items.Elevators;

public class CrystalElevator_Indicator_Lamp_Item : ModItem
{
	public override string LocalizationCategory => Commons.Utilities.LocalizationUtils.Categories.Placeables;

	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<CrystalElevator_Indicator_Lamp>());
		Item.width = 20;
		Item.height = 36;
		Item.value = 1000;
	}
}