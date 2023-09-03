namespace Everglow.Minortopography.Common
{
	public class GlobalItemRecipe : GlobalItem
	{
		public override void AddRecipes()
		{
			Recipe recipe = Recipe.Create(ItemID.PineTreeBlock);
			recipe.AddIngredient(ItemID.BorealWood);
			recipe.AddTile(TileID.LivingLoom);
			recipe.Register();
		}
	}
}