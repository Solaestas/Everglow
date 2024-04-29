namespace Everglow.CagedDomain.Items;

public class LightbulbBand_item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.LightbulbBand>());
		Item.width = 24;
		Item.height = 28;
		Item.value = 40;
	}
}
