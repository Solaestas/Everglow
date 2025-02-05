using Everglow.Yggdrasil.YggdrasilTown.Tiles.TwilightForest;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.TwilightForest;

public class TwilightEucalyptusWood_Item : ModItem
{
	// TODO:暮木
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<TwilightEucalyptusWood>());
		Item.width = 16;
		Item.height = 16;
	}
}