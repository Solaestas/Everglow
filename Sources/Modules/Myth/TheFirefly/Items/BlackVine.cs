namespace Everglow.Myth.TheFirefly.Items;

public class BlackVine : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.BlackVine>());
		Item.width = 30;
		Item.height = 30;
	}
}