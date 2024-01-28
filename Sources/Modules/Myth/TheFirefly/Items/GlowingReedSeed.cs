namespace Everglow.Myth.TheFirefly.Items;

public class GlowingReedSeed : ModItem
{
	//TODO:Translate:流萤苇絮
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.GlowingReed>());
		Item.width = 18;
		Item.height = 14;
	}
}