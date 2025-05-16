using Everglow.Yggdrasil.KelpCurtain.Tiles;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Placeables;

public class GreenCountBrick_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<GreenCountBrick>());
	}
}