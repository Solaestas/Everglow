using Everglow.Yggdrasil.KelpCurtain.Tiles.DeathJadeLake;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Placeables;

public class NoctilucentFluoriteLump_Item : ModItem
{
	public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<NoctilucentFluoriteLump>());
		Item.width = 20;
		Item.height = 20;
		Item.rare = ItemRarityID.White;
	}
}