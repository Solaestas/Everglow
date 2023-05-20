using Everglow.Myth.TheFirefly.Items;
using Terraria.GameContent.Creative;

namespace Everglow.Myth.TheFirefly.Items.Furnitures;

public class GlowWoodLantern : ModItem
{
	public override void SetStaticDefaults()
	{
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}

	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furnitures.GlowWoodLantern>());
		Item.width = 16;
		Item.height = 32;
		Item.value = 2000;
	}

	public override void AddRecipes()
	{
		Recipe recipe = CreateRecipe();
		recipe.AddIngredient(ModContent.ItemType<GlowWood>(), 6);
		recipe.AddIngredient(ModContent.ItemType<GlowWoodTorch>(), 1);
		recipe.AddTile(TileID.WorkBenches);
		recipe.Register();
	}
}