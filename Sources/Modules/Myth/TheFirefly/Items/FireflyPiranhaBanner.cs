namespace Everglow.Myth.TheFirefly.Items;

public class FireflyPiranhaBanner : ModItem
{
	//TODO:Translate:流萤虎鱼旗
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.FireflyPiranhaBanner>());
		Item.width = 12;
		Item.height = 34;
		Item.value = 2000;
	}
}
