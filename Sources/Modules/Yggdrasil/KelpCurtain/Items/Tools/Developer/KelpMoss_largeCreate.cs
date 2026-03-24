using Everglow.Yggdrasil.KelpCurtain.Tiles.IsleOfBloom;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Tools.Developer;

public class KelpMoss_largeCreate : ModItem
{
	public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<IsleBamboo>());
		Item.width = 16;
		Item.height = 16;
	}

	public override bool CanUseItem(Player player) => base.CanUseItem(player);
}