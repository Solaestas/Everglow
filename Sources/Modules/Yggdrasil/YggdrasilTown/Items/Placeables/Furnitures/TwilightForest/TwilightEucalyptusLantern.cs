namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables.Furnitures.TwilightForest;

public class TwilightEucalyptusLantern : LanternItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.TwilightForest.Furnitures.TwilightEucalyptusLantern>());
		base.SetDefaults();
	}

	public override void AddRecipes()
	{
		Recipe recipe = CreateRecipe();
		// Add recipe.
		recipe.Register();
	}
}