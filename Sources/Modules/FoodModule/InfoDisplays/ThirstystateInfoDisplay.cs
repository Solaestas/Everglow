namespace Everglow.Sources.Modules.FoodModule.InfoDisplays
{
    internal class ThirstystateInfoDisplay : InfoDisplay
    {
        public override void SetStaticDefaults()
        {
            InfoName.SetDefault(Terraria.Localization.Language.GetTextValue("Mods.Everglow.InfoDisplay.StateOfThirst"));
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
                return Terraria.Localization.Language.GetTextValue("Mods.Everglow.InfoDisplay.Thirsty");
            }
            else
            {
                return Terraria.Localization.Language.GetTextValue("Mods.Everglow.InfoDisplay.NotThirsty");
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
            {
                ShowThirstystate = true;
            }
        }
    }
}
