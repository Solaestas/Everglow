using Everglow.Yggdrasil.YggdrasilTown.Tiles.FurnaceTiles;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class MeltingInputBox_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<MeltingInputBox>());
	}
}