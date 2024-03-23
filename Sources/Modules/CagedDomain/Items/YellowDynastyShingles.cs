namespace Everglow.CagedDomain.Items;

public class YellowDynastyShingles : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.YellowDynastyShingles>());
		Item.width = 16;
		Item.height = 16;
		Item.value = 40;
	}
}
