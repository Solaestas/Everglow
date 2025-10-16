using Everglow.Yggdrasil.KelpCurtain.Tiles.DeathJadeLake;
using Everglow.Yggdrasil.YggdrasilTown.Tiles.TwilightForest;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Placeables;

public class AlgaeCoveredChest_Item : ModItem
{
	public override string LocalizationCategory => LocalizationUtils.Categories.Placeables;

	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<AlgaeCoveredChest>());
		Item.width = 32;
		Item.height = 26;
		Item.value = 2550;
	}
}