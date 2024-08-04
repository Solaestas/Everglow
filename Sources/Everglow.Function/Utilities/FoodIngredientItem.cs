namespace Everglow.Commons.Utilities;

public abstract class FoodIngredientItem : ModItem
{
	public int SlicedItemType = -1;
	public int SliceDustType = -1;

	public override void SetDefaults()
	{
		Item.value = 1;
		Item.maxStack = Item.CommonMaxStack;
		Item.rare = ItemRarityID.White;
	}

	public virtual void DefaultAsIngredient(int value)
	{
		Item.value = value;
		Item.maxStack = Item.CommonMaxStack;
		Item.rare = ItemRarityID.White;
	}

	public virtual void SliceDown(int i, int j)
	{
		if (SlicedItemType > 0 && SliceDustType > 0)
		{
			Item.stack--;
			if (Item.stack <= 0)
			{
				Item.active = false;
			}
			Item.NewItem(null, new Vector2(i, j) * 16 + new Vector2(8, -8), SlicedItemType, 1);
			for (int t = 0; t < 12; t++)
			{
				Dust dust = Dust.NewDustDirect(new Vector2(i, j) * 16, 16, 16, SliceDustType);
				dust.velocity.Y -= 3;
			}
		}
	}


	public static bool IsIngredient(int itemType)
	{
		List<int> vanillFoodType = new List<int>();
		vanillFoodType.Add(ItemID.SpicyPepper);
		vanillFoodType.Add(ItemID.BottledWater);
		vanillFoodType.Add(ItemID.BottledHoney);
		if (vanillFoodType.Contains(itemType))
		{
			return true;
		}
		return new Item(itemType).ModItem is FoodIngredientItem;
	}
}