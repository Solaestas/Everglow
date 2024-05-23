namespace Everglow.Myth.TheFirefly.Items;

public class GlowCrystal : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.GlowCrystal>());
		Item.width = 16;
		Item.height = 16;
	}
}