namespace Everglow.Myth.TheFirefly.Items;

public class CrimsonOrbStonePost : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.CrimsonOrbStonePost>());
		Item.width = 18;
		Item.height = 34;
	}
}