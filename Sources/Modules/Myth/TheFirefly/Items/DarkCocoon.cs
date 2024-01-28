namespace Everglow.Myth.TheFirefly.Items;

public class DarkCocoon : ModItem
{
	//TODO:Translate:暗茧壳
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.DarkCocoon>());
		Item.width = 16;
		Item.height = 16;
	}
}