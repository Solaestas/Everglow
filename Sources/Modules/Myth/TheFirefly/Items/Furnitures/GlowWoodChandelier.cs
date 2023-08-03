using Everglow.Myth.TheFirefly.Items;
using Terraria.GameContent.Creative;

namespace Everglow.Myth.TheFirefly.Items.Furnitures;

public class GlowWoodChandelier : ModItem
{
	public override void SetStaticDefaults()
	{
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}

	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furnitures.GlowWoodChandelier>());
		Item.width = 26;
		Item.height = 28;
		Item.value = 2000;
	}

	public override void AddRecipes()
	{
		Recipe recipe = CreateRecipe();
		recipe.AddIngredient(ModContent.ItemType<GlowWood>(), 4);
		recipe.AddIngredient(ModContent.ItemType<GlowWoodTorch>(), 4);
		recipe.AddIngredient(ItemID.Chain, 1);
		recipe.AddTile(TileID.WorkBenches);
		recipe.Register();
	}
}