namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables.Furnitures.TwilightForest;

public class TwilightEucalyptusLamp : LampItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.TwilightForest.Furnitures.TwilightEucalyptusLamp>());
		base.SetDefaults();
	}

	public override void AddRecipes()
	{
		Recipe recipe = CreateRecipe();
		// Add recipe.
		recipe.Register();
	}
}