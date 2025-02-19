namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class FurnaceCopperPipe_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.FurnaceCopperPipe>());
		Item.width = 16;
		Item.height = 16;
	}
}