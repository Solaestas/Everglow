namespace Everglow.Myth.TheFirefly.Items;

public class GlowWood : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.FireflyWood>());
		Item.width = 16;
		Item.height = 16;
	}
}