using Terraria.GameContent.Creative;
using Everglow.Commons.Utilities;
using Everglow.Commons.ItemAbstracts.Furniture;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Placeables.DecayingWoodCourt;

public class WitherWoodTable : TableItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Everglow.Yggdrasil.KelpCurtain.Tiles.DecayingWoodCourt.WitherWoodTable>());
		base.SetDefaults();
	}
}