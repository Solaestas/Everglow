using Everglow.Food.Dusts;

namespace Everglow.Food.Items.Ingredients;

public class Eggplant : FoodIngredientItem
{
	public override void SetDefaults()
	{
		DefaultAsIngredient(100);
		SlicedItemType = ModContent.ItemType<EggplantCubes>();
		SliceDustType = ModContent.DustType<EggplantDust>();
	}
}