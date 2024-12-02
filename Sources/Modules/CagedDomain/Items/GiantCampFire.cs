namespace Everglow.CagedDomain.Items;

public class GiantCampFire : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.GiantCampFire>());
		Item.width = 20;
		Item.height = 24;
		Item.value = 61000;
	}
}
