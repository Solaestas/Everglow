using Everglow.Sources.Modules.FoodModule.Utils;

namespace Everglow.Sources.Modules.FoodModule
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
        public string Description
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
        public string Description
        {
            get;
            set;
        }
    }
}
