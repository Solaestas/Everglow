using Everglow.Myth;

namespace Everglow.Myth.TheFirefly.Items;

public class FireflyMoss : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.FireflyMoss>());
		Item.width = 18;
		Item.height = 14;
	}
}