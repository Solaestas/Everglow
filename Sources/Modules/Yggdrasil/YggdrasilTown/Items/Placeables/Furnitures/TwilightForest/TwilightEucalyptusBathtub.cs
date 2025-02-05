namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables.Furnitures.TwilightForest;

public class TwilightEucalyptusBathtub : BathtubItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.TwilightForest.Furnitures.TwilightEucalyptusBathtub>());
		base.SetDefaults();
	}

	public override void AddRecipes()
	{
		Recipe recipe = CreateRecipe();
		// Add recipe.
		recipe.Register();
	}
}