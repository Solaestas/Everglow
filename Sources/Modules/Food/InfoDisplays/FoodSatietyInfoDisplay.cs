using Terraria.Localization;

namespace Everglow.Food.InfoDisplays
{
	internal class FoodSatietyInfoDisplay : InfoDisplay
	{
		public override bool Active()
		{
			return Main.LocalPlayer.GetModPlayer<FoodSatietyInfoDisplayplayer>().ShowCurrentSatiety;
		}

		public override string DisplayValue()
		{

			int currentSatiety = Main.LocalPlayer.GetModPlayer<FoodModPlayer>().CurrentSatiety;
			int level = Main.LocalPlayer.GetModPlayer<FoodModPlayer>().SatietyLevel;
			return Language.GetTextValue("Mods.Everglow.InfoDisplay.Satiety", currentSatiety, Language.GetTextValue($"Mods.Everglow.InfoDisplay.SatietyLevel.{level}"));
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
				ShowCurrentSatiety = true;
		}
	}
}
