namespace Everglow.Yggdrasil.YggdrasilTown.Items;

public class FurnacePlatingBlock : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.FurnacePlatingTile>());
		Item.width = 16;
		Item.height = 16;
	}
}