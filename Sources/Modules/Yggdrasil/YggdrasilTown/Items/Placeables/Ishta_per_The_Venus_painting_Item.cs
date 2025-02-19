namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class Ishta_per_The_Venus_painting_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Ishta_per_The_Venus_painting>());
		Item.width = 16;
		Item.height = 16;
	}
}