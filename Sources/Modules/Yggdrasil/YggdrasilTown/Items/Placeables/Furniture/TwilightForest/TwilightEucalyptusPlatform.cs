namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables.Furniture.TwilightForest;

public class TwilightEucalyptusPlatform : PlatformItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.TwilightForest.Furnitures.TwilightEucalyptusPlatform>());
		base.SetDefaults();
	}

	public override void AddRecipes()
	{
		Recipe recipe = CreateRecipe();
		// Add recipe.
		recipe.Register();
	}
}