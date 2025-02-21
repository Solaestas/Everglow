namespace Everglow.Food.Items;

/// <summary>
/// 食物类物品的基类，填写FoodInfo即可
/// </summary>
public abstract class FoodBase : ModItem
{
	public abstract FoodInfo FoodInfo
	{
		get;
	}
}

/// <summary>
/// 饮料类物品的基类，填写DrinkInfo即可
/// </summary>
public abstract class DrinkBase : ModItem
{
	public abstract DrinkInfo DrinkInfo
	{
		get;
	}
}