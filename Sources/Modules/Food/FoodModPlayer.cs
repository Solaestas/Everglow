namespace Everglow.Sources.Modules.Food
{
    public class FoodModPlayer : ModPlayer
    {
        /// <summary>
        /// 玩家当前饱食度
        /// </summary>
        public int CurrentSatiety { get; set; }

        /// <summary>
        /// 玩家最大饱食度
        /// </summary>
        public int MaximumSatiety { get; set; }

        public FoodModPlayer()
        {
            CurrentSatiety = 0;
            MaximumSatiety = 50;
        }

        /// <summary>
        /// 如果能吃下，返回true，否则为false
        /// </summary>
        /// <param name="foodInfo"></param>
        /// <returns></returns>
        public bool CanEat(FoodInfo foodInfo)
        {
            if(CurrentSatiety + foodInfo.Satiety <= MaximumSatiety)
            {
                return true;
            }
            return false;
        }
    }
}
