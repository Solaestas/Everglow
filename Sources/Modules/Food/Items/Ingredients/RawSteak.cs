using Everglow.Food.Dusts;

namespace Everglow.Food.Items.Ingredients;

public class RawSteak : FoodIngredientItem
{
	public override void SetDefaults()
	{
		DefaultAsIngredient(500);
		SlicedItemType = ModContent.ItemType<GroundMeat>();
		SliceDustType = ModContent.DustType<SteakDust>();
	}
}