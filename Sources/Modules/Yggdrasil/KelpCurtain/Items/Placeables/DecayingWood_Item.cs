using Everglow.Yggdrasil.KelpCurtain.Tiles;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Placeables;

public class DecayingWood_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<DecayingWood>());
		Item.width = 24;
		Item.height = 22;
		Item.rare = ItemRarityID.White;
	}
}