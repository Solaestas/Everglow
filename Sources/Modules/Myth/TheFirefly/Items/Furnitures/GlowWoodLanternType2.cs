using Everglow.Myth.TheFirefly.Items;
using Terraria.GameContent.Creative;

namespace Everglow.Myth.TheFirefly.Items.Furnitures;

public class GlowWoodLanternType2 : ModItem
{
	//TODO:Translate:流萤灯笼\n款式2
	public override void SetStaticDefaults()
	{
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}

	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furnitures.GlowWoodLanternType2>());
		Item.width = 14;
		Item.height = 30;
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