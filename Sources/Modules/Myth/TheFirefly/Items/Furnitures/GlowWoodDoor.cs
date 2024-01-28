namespace Everglow.Myth.TheFirefly.Items.Furnitures;

public class GlowWoodDoor : ModItem
{
	//TODO:Translate:流萤木门\n款式1
	public override void SetStaticDefaults()
	{
	}

	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furnitures.GlowWoodDoorClosed>());
		Item.width = 18;
		Item.height = 32;
		Item.value = 2000;
	}

	public override void AddRecipes()
	{
		Recipe recipe = CreateRecipe();
		recipe.AddIngredient(ModContent.ItemType<GlowWood>(), 6);
		recipe.AddTile(TileID.WorkBenches);
		recipe.Register();
	}
}