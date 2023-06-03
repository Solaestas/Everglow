namespace Everglow.Myth.TheFirefly.Items;

public class FireflyMoss : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.DarkCocoonMoss>());
		Item.width = 20;
		Item.height = 18;
	}
}