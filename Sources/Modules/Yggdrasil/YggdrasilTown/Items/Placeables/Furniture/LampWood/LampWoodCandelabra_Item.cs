using Everglow.Yggdrasil.YggdrasilTown.Tiles.LampWood.Furniture;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables.Furniture.LampWood;

public class LampWoodCandelabra_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<LampWoodCandelabra>());
	}
}