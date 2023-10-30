namespace Everglow.Yggdrasil.YggdrasilTown.Items;

public class GiantBell : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.GiantBell_Tile>());
		Item.width = 20;
		Item.height = 24;
		Item.value = 61000;
	}
}
