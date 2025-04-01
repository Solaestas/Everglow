using Everglow.Food.Dusts;

namespace Everglow.Food.Items.Ingredients;

public class Garlic : FoodIngredientItem
{
	public override void SetDefaults()
	{
		DefaultAsIngredient(10);
		SlicedItemType = ModContent.ItemType<MincedGarlic>();
		SliceDustType = ModContent.DustType<GarlicDust>();
	}
}