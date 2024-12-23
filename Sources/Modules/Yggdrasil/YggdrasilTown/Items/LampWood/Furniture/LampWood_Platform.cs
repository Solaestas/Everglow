using Everglow.Yggdrasil.YggdrasilTown.Tiles.LampWood.Furniture;
using Terraria.GameContent.Creative;
namespace Everglow.Yggdrasil.YggdrasilTown.Items.LampWood.Furniture;

public class LampWood_Platform : ModItem
{
	public override void SetStaticDefaults()
	{
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 200;
	}

	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<LampWoodPlatform>());
		Item.width = 24;
		Item.height = 18;
	}

	public override void AddRecipes()
	{
		Recipe recipe = CreateRecipe(2);
		recipe.AddIngredient(ModContent.ItemType<LampWood_Wood>(), 1);
		recipe.Register();
	}
}