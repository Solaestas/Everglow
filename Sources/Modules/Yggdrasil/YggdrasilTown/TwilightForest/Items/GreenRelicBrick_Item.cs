using Everglow.Yggdrasil.YggdrasilTown.TwilightForest.Tiles;

namespace Everglow.Yggdrasil.YggdrasilTown.TwilightForest.Items;

public class GreenRelicBrick_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<GreenRelicBrick>());
		Item.width = 16;
		Item.height = 16;
	}
}