using Everglow.Myth.LanternMoon.Tiles;

namespace Everglow.Myth.LanternMoon.Items;

public class FireCrackerLauncher : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<FireCrackerLauncher_tile>());
		Item.width = 16;
		Item.height = 32;
		Item.value = 1000;
	}
}
