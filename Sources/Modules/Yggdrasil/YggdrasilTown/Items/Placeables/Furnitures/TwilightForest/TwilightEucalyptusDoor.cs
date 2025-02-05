using Everglow.Yggdrasil.YggdrasilTown.Tiles.TwilightForest.Furnitures;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables.Furnitures.TwilightForest;

public class TwilightEucalyptusDoor : DoorItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<TwilightEucalyptusDoorClosed>());
		base.SetDefaults();
	}

	public override void AddRecipes()
	{
		Recipe recipe = CreateRecipe();
		// Add recipe.
		recipe.Register();
	}
}