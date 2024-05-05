using Everglow.Commons.TileHelper;

namespace Everglow.CagedDomain.Items.CableItems;

public class RedGlassbulbBand_item : CableTileItem
{
	public override int TileType => ModContent.TileType<Tiles.CableTiles.RedGlassbulbBand_bulb>();

	public override void SetDefaults()
	{
		base.SetDefaults();
		Item.width = 24;
		Item.height = 28;
		Item.value = 40;
	}
}