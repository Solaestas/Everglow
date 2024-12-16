using Terraria.GameContent.Creative;
using Everglow.Commons.Utilities;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Furnace.Furnitures;

public class HeatproofSink_item : SinkItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Everglow.Yggdrasil.YggdrasilTown.Tiles.FurnaceTiles.HeatproofSink>());
		base.SetDefaults();
	}
}