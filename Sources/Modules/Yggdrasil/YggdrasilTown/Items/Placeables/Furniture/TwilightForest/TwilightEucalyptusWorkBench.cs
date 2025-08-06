using Everglow.Commons.ItemAbstracts.Furniture;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables.Furniture.TwilightForest;

public class TwilightEucalyptusWorkBench : WorkBenchItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.TwilightForest.Furnitures.TwilightEucalyptusWorkBench>());
		base.SetDefaults();
	}

	public override void AddRecipes()
	{
		Recipe recipe = CreateRecipe();
		// Add recipe.
		recipe.Register();
	}
}