using Everglow.Myth.TheFirefly.Items;
using Everglow.Myth.TheFirefly.Tiles.Furnitures;

namespace Everglow.Myth.TheFirefly.Items.Furnitures;

public class GlowWoodDoorType2 : ModItem
{
	//TODO:Translate:流萤木门\n款式2
	public override void SetStaticDefaults()
	{
	}

	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furnitures.GlowWoodDoorClosedType2>());
		Item.width = 38;
		Item.height = 24;
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