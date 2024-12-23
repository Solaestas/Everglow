using Terraria.GameContent.Creative;
using Everglow.Commons.Utilities;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Furnace.Furnitures;

public class HeatproofSofa_item : SofaItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Everglow.Yggdrasil.YggdrasilTown.Tiles.FurnaceTiles.HeatproofSofa>());
		base.SetDefaults();
	}
}