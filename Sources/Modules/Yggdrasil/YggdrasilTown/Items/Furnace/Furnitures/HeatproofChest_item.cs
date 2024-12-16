using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Furnace.Furnitures;

public class HeatproofChest_item : ChestItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Everglow.Yggdrasil.YggdrasilTown.Tiles.FurnaceTiles.HeatproofChest>());
		base.SetDefaults();
	}
}