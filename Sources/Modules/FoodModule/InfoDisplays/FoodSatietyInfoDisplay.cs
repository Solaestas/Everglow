namespace Everglow.Sources.Modules.FoodModule.InfoDisplays
{
    internal class FoodSatietyInfoDisplay : InfoDisplay
    {
        public override void SetStaticDefaults()
        {
            InfoName.SetDefault(Terraria.Localization.Language.GetTextValue("Mods.Everglow.InfoDisplay.Satiety"));
        }


        public override bool Active()
        {
            return Main.LocalPlayer.GetModPlayer<FoodSatietyInfoDisplayplayer>().ShowCurrentSatiety;
        }

        public override string DisplayValue()
        {

            int CurrentSatiety = Main.LocalPlayer.GetModPlayer<FoodModPlayer>().CurrentSatiety;
            return $"{CurrentSatiety} Satiety";
        }
    }

    public class FoodSatietyInfoDisplayplayer : ModPlayer
    {
        public bool AccBloodGlucoseMonitor;
        public bool ShowCurrentSatiety;
        public override void ResetEffects()
        {
            AccBloodGlucoseMonitor = false;
            ShowCurrentSatiety = false;
        }

        public override void UpdateEquips()
        {
            if (AccBloodGlucoseMonitor)
            {
                ShowCurrentSatiety = true;
            }
        }
    }
}
