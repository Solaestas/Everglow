namespace Everglow.Yggdrasil.YggdrasilTown.Items;

public class DarkSludge : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.DarkSludge>());
		Item.width = 16;
		Item.height = 16;
	}
}
