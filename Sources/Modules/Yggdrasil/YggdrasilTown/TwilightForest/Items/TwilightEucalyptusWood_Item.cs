using Everglow.Yggdrasil.YggdrasilTown.TwilightForest.Tiles;

namespace Everglow.Yggdrasil.YggdrasilTown.TwilightForest.Items;

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