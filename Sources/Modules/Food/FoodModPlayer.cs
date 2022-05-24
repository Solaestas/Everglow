namespace Everglow.Sources.Modules.Food
{
    public class FoodModPlayer : ModPlayer
    {

        /// <summary>
        /// 玩家当前CurrentSatiety
        /// </summary>
        public int CurrentSatiety
        {
            get; set;
        }

        /// <summary>
        /// 玩家最大CurrentSatiety
        /// </summary>
        public int MaximumSatiety
        {
            get; set;
        }
        /// <summary>
        /// 玩家当前渴觉状态
        /// </summary>
        public bool Thirstystate
        {
            get; set;
        }
        public FoodModPlayer()
        {
            CurrentSatiety = 0;
            MaximumSatiety = 50;
            Thirstystate = true; 
        }
        
        /// <summary>
        /// 如果能吃下，返回true，否则为false
        /// </summary>
        /// <param name="foodInfo"></param>
        /// <returns></returns>
        public bool CanEat(FoodInfo foodInfo)
        {
            if (CurrentSatiety + foodInfo.Satiety <= MaximumSatiety)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 如果能喝下，返回true，否则为false
        /// </summary>
        public bool CanDrink(DrinkInfo drinkInfo)
        {
            if (Thirstystate)
            {
                return true;
            }
            return false;
        }
        /*
         
         
         */
        /// <summary>
        /// 以下为计时器
        /// </summary>
        public int SatietyLossTimer { get; private set; }//饱食损失计时器
        public int ThirstyChangeTimer { get; private set; }//口渴变化计时器
        public override void PostUpdate()
        {
            FoodState(); 
            if (!Player.active)
            {
                CurrentSatiety = 0;
                Thirstystate = true;
            }
        }

        public void FoodState()
        {
            //从吃食物后开始计时
            if(CurrentSatiety > 0){
                 SatietyLossTimer++;
            }
            //从喝饮料后开始计时
            if (!Thirstystate)
            {
                ThirstyChangeTimer++;
            }

            //每三十秒减少一饱食度
            if (SatietyLossTimer>=180)
            {
                CurrentSatiety -= 1 ;
                SatietyLossTimer = 0;
            }
            if (CurrentSatiety <= 0)
            {
                CurrentSatiety = 0;
            }
            //每五分钟从口渴变得不口渴
            if (ThirstyChangeTimer >= 180)
            {
                Thirstystate = true ;
                ThirstyChangeTimer = 0;
            }

        }

        
    }
}
