namespace Everglow.Myth.TheFirefly.Items;

public class FireflyPigment : ModItem
{
	public override void SetStaticDefaults()
	{
		
	}

	public override void SetDefaults()
	{
		
		Item.width = 18;
		Item.height = 30;
		Item.maxStack = 999;
	}

	public override void AddRecipes()
	{
		Recipe recipe = CreateRecipe();
		recipe.AddIngredient(ModContent.ItemType<BlackStarShrub>(), 5);
		recipe.AddTile(TileID.WorkBenches);
		recipe.Register();

		Recipe recipe2 = CreateRecipe();
		recipe2.AddIngredient(ModContent.ItemType<FireflyMoss_Item>(), 6);
		recipe2.AddTile(TileID.WorkBenches);
		recipe2.Register();

		Recipe recipe3 = CreateRecipe();
		recipe3.AddIngredient(ModContent.ItemType<GlowingPetal>(), 5);
		recipe3.AddTile(TileID.WorkBenches);
		recipe3.Register();

		Recipe recipe4 = CreateRecipe();
		recipe4.AddIngredient(ModContent.ItemType<MothScaleDust>(), 2);
		recipe4.AddTile(TileID.WorkBenches);
		recipe4.Register();
	}
}