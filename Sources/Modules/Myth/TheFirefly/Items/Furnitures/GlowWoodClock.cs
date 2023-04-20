using Terraria.GameContent.Creative;

namespace Everglow.Myth.TheFirefly.Items.Furnitures;

public class GlowWoodClock : ModItem
{
	public override void SetStaticDefaults()
	{
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}

	public override void SetDefaults()
	{
		Item.width = 26;
		Item.height = 22;
		Item.maxStack = 99;
		Item.useTurn = true;
		Item.autoReuse = true;
		Item.useAnimation = 15;
		Item.useTime = 10;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.consumable = true;
		Item.value = 500;
		Item.createTile = ModContent.TileType<Tiles.Furnitures.GlowWoodClock>();
	}

	public override void AddRecipes()
	{
		Recipe recipe = CreateRecipe();
		recipe.AddIngredient(ModContent.ItemType<GlowWood>(), 10);
		recipe.AddIngredient(ItemID.Glass, 6);
		recipe.AddIngredient(ItemID.IronBar, 3);
		recipe.AddRecipeGroup(RecipeGroupID.IronBar, 2);
		recipe.AddTile(TileID.WorkBenches);
		recipe.Register();
	}
}