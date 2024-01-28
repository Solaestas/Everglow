using Everglow.Myth.TheFirefly.Items;
using Terraria.GameContent.Creative;

namespace Everglow.Myth.TheFirefly.Items.Furnitures;

public class GlowWoodCandelabra : ModItem
{
	//TODO:Translate:流萤烛台
	public override void SetStaticDefaults()
	{
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}

	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furnitures.GlowWoodCandelabra>());
		Item.width = 30;
		Item.height = 30;
	}

	public override void AddRecipes()
	{
		Recipe recipe = CreateRecipe();
		recipe.AddIngredient(ModContent.ItemType<GlowWood>(), 5);
		recipe.AddIngredient(ModContent.ItemType<GlowWoodTorch>(), 3);
		recipe.AddTile(TileID.WorkBenches);
		recipe.Register();
	}
}