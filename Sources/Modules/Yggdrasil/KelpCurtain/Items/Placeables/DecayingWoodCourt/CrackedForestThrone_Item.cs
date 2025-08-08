using Everglow.Yggdrasil.KelpCurtain.Tiles.DecayingWoodCourt;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Placeables.DecayingWoodCourt;

public class CrackedForestThrone_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<CrackedForestThrone>());
		base.SetDefaults();
	}
}