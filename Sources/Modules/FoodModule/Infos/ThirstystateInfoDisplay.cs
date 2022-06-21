namespace Everglow.Sources.Modules.FoodModule.Infos
{
    class ThirstystateInfoDisplay : InfoDisplay
    {
        public override void SetStaticDefaults()
        {
            InfoName.SetDefault("Thirsty State");
        }


        public override bool Active()
        {
            return Main.LocalPlayer.GetModPlayer<ThirstystateInfoDisplayplayer>().ShowThirstystate;
        }

        public override string DisplayValue()
        {

            bool Thirstystate = Main.LocalPlayer.GetModPlayer<FoodModPlayer>().Thirstystate;
            if (Thirstystate)
            {
                return $"You want to drink.";
            }
            else
            {
                return $"You do not want to drink.";
            }
        }
    }

    public class ThirstystateInfoDisplayplayer : ModPlayer
    {
        public bool AccOsmoticPressureMonitor;
        public bool ShowThirstystate;
        public override void ResetEffects()
        {
            AccOsmoticPressureMonitor = false;
            ShowThirstystate = false;
        }

        public override void UpdateEquips()
        {
            if (AccOsmoticPressureMonitor)
                ShowThirstystate = true;
        }
    }
}
