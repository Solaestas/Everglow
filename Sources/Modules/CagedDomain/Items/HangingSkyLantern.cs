namespace Everglow.CagedDomain.Items;

public class HangingSkyLantern : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.HangingSkyLantern>());
		Item.width = 22;
		Item.height = 32;
		Item.value = 1000;
	}
}
