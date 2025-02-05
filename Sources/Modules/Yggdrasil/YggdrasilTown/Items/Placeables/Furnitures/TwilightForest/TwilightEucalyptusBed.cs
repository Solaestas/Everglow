namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables.Furnitures.TwilightForest;

public class TwilightEucalyptusBed : BedItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.TwilightForest.Furnitures.TwilightEucalyptusBed>());
		base.SetDefaults();
	}

	public override void AddRecipes()
	{
		Recipe recipe = CreateRecipe();
		// Add recipe.
		recipe.Register();
	}
}