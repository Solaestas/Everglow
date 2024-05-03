using Everglow.Commons.TileHelper;

namespace Everglow.CagedDomain.Items;

public class LightbulbBand_item : CableTileItem
{
	public override int TileType => ModContent.TileType<Tiles.LightbulbBand>();

	public override void SetDefaults()
	{
		base.SetDefaults();
		Item.width = 24;
		Item.height = 28;
		Item.value = 40;
	}
}