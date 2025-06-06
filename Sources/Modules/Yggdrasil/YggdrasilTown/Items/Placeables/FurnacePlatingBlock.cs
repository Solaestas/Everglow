namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class FurnacePlatingBlock : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.FurnacePlatingTile>());
		Item.width = 16;
		Item.height = 16;
	}
}