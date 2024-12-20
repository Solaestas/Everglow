namespace Everglow.Yggdrasil.YggdrasilTown.TwilightForest.Items.Furnitures;

public class TwilightEucalyptusClock : ClockItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Everglow.Yggdrasil.YggdrasilTown.TwilightForest.Tiles.Furnitures.TwilightEucalyptusClock>());
		base.SetDefaults();
	}

	public override void AddRecipes()
	{
		Recipe recipe = CreateRecipe();
		// Add recipe.
		recipe.Register();
	}
}