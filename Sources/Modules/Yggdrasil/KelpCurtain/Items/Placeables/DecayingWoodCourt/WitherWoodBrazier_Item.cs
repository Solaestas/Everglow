using Terraria.GameContent.Creative;
using Everglow.Commons.Utilities;
using Everglow.Commons.ItemAbstracts.Furniture;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Placeables.DecayingWoodCourt;

public class WitherWoodBrazier_Item : ChandelierItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Everglow.Yggdrasil.KelpCurtain.Tiles.DecayingWoodCourt.WitherWoodBrazier>());
		base.SetDefaults();
	}
}