using Everglow.Yggdrasil.KelpCurtain.Tiles.DeathJadeLake;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Placeables;

public class WaterErodedChestInlaidWithGold_Item : ModItem
{
	public override string LocalizationCategory => LocalizationUtils.Categories.Placeables;

	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<WaterErodedChestInlaidWithGold>());
		Item.width = 32;
		Item.height = 26;
		Item.value = 16200;
	}
}