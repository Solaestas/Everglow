using Everglow.Yggdrasil.YggdrasilTown.Tiles.TwilightForest;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class TwilightEucalyptusWood_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<TwilightEucalyptusWood>());
		Item.width = 16;
		Item.height = 16;
	}
}