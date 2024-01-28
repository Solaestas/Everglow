namespace Everglow.Myth.TheFirefly.Items;

public class CorruptOrbStonePost : ModItem
{
	//TODO:Translate:腐色戒律石柱
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.CorruptOrbStonePost>());
		Item.width = 18;
		Item.height = 34;
	}
}