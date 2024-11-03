namespace Everglow.Yggdrasil.YggdrasilTown.Items.LampWood.Furniture;

// TODO: Add Recipes
public class LampWoodDoor : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 14;
		Item.height = 28;
		Item.value = 150;
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.LampWood.Furniture.LampWoodDoorClosed>());
	}

	public override void AddRecipes()
	{

	}
}