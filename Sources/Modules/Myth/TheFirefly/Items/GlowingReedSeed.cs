namespace Everglow.Myth.TheFirefly.Items;

public class GlowingReedSeed : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.GlowingReed>());
		Item.width = 18;
		Item.height = 14;
	}
}