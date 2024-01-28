namespace Everglow.Myth.TheFirefly.Items;

public class BlackVine : ModItem
{
	//TODO:Translate:黑皮藤蔓
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.BlackVine>());
		Item.width = 30;
		Item.height = 30;
	}
}