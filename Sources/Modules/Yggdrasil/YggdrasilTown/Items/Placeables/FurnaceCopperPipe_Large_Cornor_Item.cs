
namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class FurnaceCopperPipe_Large_Cornor_Item : ModItem
{
	public int State = 0;

	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.FurnaceCopperPipe_Large_V>());
		Item.width = 16;
		Item.height = 16;
	}
}