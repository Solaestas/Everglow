using Terraria.GameContent.Creative;
using Everglow.Commons.Utilities;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Furnace.Furnitures;

public class HeatproofTable_item : TableItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Everglow.Yggdrasil.YggdrasilTown.Tiles.FurnaceTiles.HeatproofTable>());
		base.SetDefaults();
	}
}