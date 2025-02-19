using Everglow.CagedDomain.Tiles.HangingTiles;

namespace Everglow.CagedDomain.Items;

public class FluoriteHangLamp : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<HangingFluoriteLamp>());
		Item.width = 14;
		Item.height = 36;
	}
}