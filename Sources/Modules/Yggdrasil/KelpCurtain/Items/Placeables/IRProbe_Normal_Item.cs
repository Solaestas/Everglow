using Everglow.Yggdrasil.KelpCurtain.Tiles.DeathJadeLake;
using Everglow.Yggdrasil.KelpCurtain.Tiles.DeathJadeLake.UnderwaterGuillotine;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Placeables;

public class IRProbe_Normal_Item : ModItem
{
	public override string LocalizationCategory => LocalizationUtils.Categories.Placeables;

	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<IRProbe_Normal>());
		Item.width = 20;
		Item.height = 24;
		Item.value = 400;
	}
}