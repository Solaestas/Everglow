using Everglow.Yggdrasil.KelpCurtain.Tiles.DeathJadeLake;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Placeables;

public class HumicMud_Item : ModItem
{
	public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<HumicMud>());
		Item.width = 16;
		Item.height = 16;
	}
}