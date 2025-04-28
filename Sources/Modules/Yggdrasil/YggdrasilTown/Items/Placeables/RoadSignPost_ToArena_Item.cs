namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class RoadSignPost_ToArena_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.RoadSignPost_ToArena>());
		Item.width = 16;
		Item.height = 16;
	}
}