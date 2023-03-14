namespace Everglow.Myth.TheFirefly.Items;

public class MothScaleDust : ModItem
{
	public override void SetStaticDefaults()
	{
		
	}

	public override void SetDefaults()
	{
		
		Item.width = 20;
		Item.height = 12;
		Item.maxStack = 999;
	}

	public override void AddRecipes()
	{
		Recipe recipe = CreateRecipe();
		recipe.AddIngredient(ModContent.ItemType<GlowingFirefly>(), 1);
		recipe.AddTile(TileID.WorkBenches);
		recipe.Register();
	}
}