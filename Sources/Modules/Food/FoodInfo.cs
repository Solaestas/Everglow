using Everglow.Sources.Modules.Food.DataStructures;

namespace Everglow.Sources.Modules.Food
{
    public class FoodInfo
    {
        public int Satiety
        {
            get;
            set;
        }

        public int BuffType
        {
            get;
            set;
        }

        public FoodDuration BuffTime
        {
            get;
            set;
        }
    }
    public class DrinkInfo
    {
        public bool Thirsty
        {
            get;
            set;
        }

        public int BuffType
        {
            get;
            set;
        }

        public FoodDuration BuffTime
        {
            get;
            set;
        }
    }
}
