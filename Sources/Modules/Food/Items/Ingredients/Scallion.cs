using Everglow.Food.Dusts;

namespace Everglow.Food.Items.Ingredients;

public class Scallion : FoodIngredientItem
{
	public override void SetDefaults()
	{
		DefaultAsIngredient(6);
		SlicedItemType = ModContent.ItemType<ChoppedScallion>();
		SliceDustType = ModContent.DustType<ScallionDust>();
	}
}