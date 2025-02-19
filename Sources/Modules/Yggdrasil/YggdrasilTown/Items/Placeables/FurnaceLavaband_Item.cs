namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class FurnaceLavaband_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.FurnaceLavaband>());
		Item.width = 16;
		Item.height = 16;
	}
}