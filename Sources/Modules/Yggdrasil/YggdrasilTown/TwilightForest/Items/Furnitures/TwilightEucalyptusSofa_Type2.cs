namespace Everglow.Yggdrasil.YggdrasilTown.TwilightForest.Items.Furnitures;

public class TwilightEucalyptusSofa_Type2 : SofaItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Everglow.Yggdrasil.YggdrasilTown.TwilightForest.Tiles.Furnitures.TwilightEucalyptusSofa_Type2>());
		base.SetDefaults();
	}

	public override void AddRecipes()
	{
		Recipe recipe = CreateRecipe();
		// Add recipe.
		recipe.Register();
	}
}