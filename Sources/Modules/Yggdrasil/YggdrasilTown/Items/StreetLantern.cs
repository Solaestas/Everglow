namespace Everglow.Yggdrasil.YggdrasilTown.Items;

public class StreetLantern : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.StreetLantern>());
		Item.width = 16;
		Item.height = 28;
		Item.value = 1000;
	}
}
