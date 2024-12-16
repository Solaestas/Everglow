using Terraria.GameContent.Creative;
using Everglow.Commons.Utilities;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Furnace.Furnitures;

public class HeatproofDoor_item : DoorItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Everglow.Yggdrasil.YggdrasilTown.Tiles.FurnaceTiles.HeatproofDoorClosed>());
		base.SetDefaults();
	}
}