namespace Everglow.Sources.Modules.Food
{
    public class FoodModPlayer : ModPlayer
    {
        public int CurrentSatiety { get; set; }

        public int MaximumSatiety { get; set; }

        public FoodModPlayer()
        {
            CurrentSatiety = 0;
            MaximumSatiety = 50;
        }

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
