using Terraria.GameContent.Creative;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.LampWood;

public class LampWood_Chest_Item : ModItem
{
	public override void SetStaticDefaults()
	{
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.LampWood.LampWood_Chest>());
	}
	public override void AddRecipes()
	{
		Recipe recipe = CreateRecipe();
		recipe.AddIngredient(ModContent.ItemType<LampWood_Wood>(), 8);
		recipe.AddRecipeGroup(RecipeGroupID.IronBar, 2);
		recipe.AddTile(TileID.WorkBenches);
		recipe.Register();
	}
}