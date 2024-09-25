namespace Everglow.Yggdrasil.YggdrasilTown.Items.LampWood.Furniture;

// TODO: Replace sprite
public class LampWoodDoor : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.LampWood.Furniture.LampWoodDoorClosed>());
	}
}