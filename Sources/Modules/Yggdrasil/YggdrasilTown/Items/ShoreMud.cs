namespace Everglow.Yggdrasil.YggdrasilTown.Items;

public class ShoreMud : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.DarkMud>());
		Item.width = 16;
		Item.height = 16;
	}
}
