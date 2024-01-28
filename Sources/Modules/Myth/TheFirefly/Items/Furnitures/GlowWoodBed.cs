using Terraria.GameContent.Creative;

namespace Everglow.Myth.TheFirefly.Items.Furnitures;

public class GlowWoodBed : ModItem
{
	//TODO:Translate:流萤床
	public override void SetStaticDefaults()
	{
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}

	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furnitures.GlowWoodBed>());
		Item.width = 32;
		Item.height = 22;
	}

	public override void AddRecipes()
	{
		Recipe recipe = CreateRecipe();
		recipe.AddIngredient(ModContent.ItemType<GlowWood>(), 15);
		recipe.AddIngredient(ItemID.Silk, 5);
		recipe.AddTile(TileID.WorkBenches);
		recipe.Register();
	}
}