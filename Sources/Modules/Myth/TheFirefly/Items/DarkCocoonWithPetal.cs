namespace Everglow.Myth.TheFirefly.Items;

public class DarkCocoonWithPetal : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.DarkCocoon_petal>());
		Item.width = 16;
		Item.height = 16;
	}
}