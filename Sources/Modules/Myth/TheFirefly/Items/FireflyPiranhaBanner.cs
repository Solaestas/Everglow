namespace Everglow.Myth.TheFirefly.Items;

public class FireflyPiranhaBanner : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.FireflyPiranhaBanner>());
		Item.width = 12;
		Item.height = 34;
		Item.value = 2000;
	}
}
