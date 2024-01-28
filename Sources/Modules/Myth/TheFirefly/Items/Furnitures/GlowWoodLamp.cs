using Terraria.GameContent.Creative;

namespace Everglow.Myth.TheFirefly.Items.Furnitures;

public class GlowWoodLamp : ModItem
{
	//TODO:Translate:流萤灯
	public override void SetStaticDefaults()
	{
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}

	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furnitures.GlowWoodLamp>());
		Item.width = 14;
		Item.height = 30;
		Item.value = 2000;
	}

	public override void AddRecipes()
	{
		Recipe recipe = CreateRecipe();
		recipe.AddIngredient(ModContent.ItemType<GlowWood>(), 3);
		recipe.AddIngredient(ModContent.ItemType<GlowWoodTorch>(), 1);
		recipe.AddTile(TileID.WorkBenches);
		recipe.Register();
	}
}