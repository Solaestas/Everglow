namespace Everglow.Sources.Modules.Food.Items
{
    /// <summary>
    /// 食物类物品的基类，填写FoodInfo即可
    /// </summary>
    public abstract class FoodBase : ModItem
    {
        public abstract FoodInfo FoodInfo { get; }
    }
}
