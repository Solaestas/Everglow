using Everglow.Food.Dusts;

namespace Everglow.Food.Items.Ingredients;

public class Tofu : FoodIngredientItem
{
	public override void SetDefaults()
	{
		DefaultAsIngredient(100);
		SlicedItemType = ModContent.ItemType<TofuCubes>();
		SliceDustType = ModContent.DustType<TofuDust>();
	}
}