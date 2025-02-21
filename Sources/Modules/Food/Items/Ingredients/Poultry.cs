using Everglow.Food.Dusts;

namespace Everglow.Food.Items.Ingredients;

public class Poultry : FoodIngredientItem
{
	public override void SetDefaults()
	{
		DefaultAsIngredient(211);
		SlicedItemType = ModContent.ItemType<ChoppedPoultry>();
		SliceDustType = ModContent.DustType<PoultryDust>();
	}
}