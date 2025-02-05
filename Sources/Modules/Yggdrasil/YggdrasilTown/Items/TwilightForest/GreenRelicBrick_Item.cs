using Everglow.Yggdrasil.YggdrasilTown.Tiles.TwilightForest;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.TwilightForest;

public class GreenRelicBrick_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<GreenRelicBrick>());
		Item.width = 16;
		Item.height = 16;
	}
}