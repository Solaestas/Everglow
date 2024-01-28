namespace Everglow.Myth.TheFirefly.Items;

public class GlowCrystal : ModItem
{
	//TODO:Translate:夜光凝胶块
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.GlowCrystal>());
		Item.width = 16;
		Item.height = 16;
	}
}