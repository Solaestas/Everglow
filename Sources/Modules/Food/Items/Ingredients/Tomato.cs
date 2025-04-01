using Everglow.Food.Dusts;

namespace Everglow.Food.Items.Ingredients;

public class Tomato : FoodIngredientItem
{
	public override void SetDefaults()
	{
		DefaultAsIngredient(100);
		SlicedItemType = ModContent.ItemType<TomatoPieces>();
		SliceDustType = ModContent.DustType<TomatoDust>();
	}
}