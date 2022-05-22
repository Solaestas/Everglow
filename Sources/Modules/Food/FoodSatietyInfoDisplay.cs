using System.Linq;
using Terraria;
using Terraria.ModLoader;


namespace Everglow.Sources.Modules.Food
{
	class FoodSatietyInfoDisplay : InfoDisplay
	{
		public override void SetStaticDefaults() {
			InfoName.SetDefault("Current Satiety");
		}


		public override bool Active() {
			return Main.LocalPlayer.GetModPlayer<FoodSatietyInfoDisplayplayer>().showCurrentSatiety;
		}

		public override string DisplayValue() {

			int CurrentSatiety = Main.LocalPlayer.GetModPlayer<FoodModPlayer>().CurrentSatiety;
			return $"{CurrentSatiety} Satiety .";
		}
	}

	public class FoodSatietyInfoDisplayplayer : ModPlayer
	{
		public bool accBloodGlucoseMonitor;
		public bool showCurrentSatiety;
		public override void ResetEffects() {
			accBloodGlucoseMonitor = false;
			showCurrentSatiety = false;
		}

		public override void UpdateEquips() {
			if (accBloodGlucoseMonitor)
				showCurrentSatiety = true;
		}
	}
}
