using System.Linq;
using Terraria;
using Terraria.ModLoader;


namespace Everglow.Sources.Modules.Food
{
	class ThirstystateInfoDisplay : InfoDisplay
	{
		public override void SetStaticDefaults() {
			InfoName.SetDefault("Thirsty State");
		}


		public override bool Active() {
			return Main.LocalPlayer.GetModPlayer<ThirstystateInfoDisplayplayer>().showThirstystate;
		}

		public override string DisplayValue() {

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
		public bool accOsmoticPressureMonitor;
		public bool showThirstystate;
		public override void ResetEffects() {
			accOsmoticPressureMonitor = false;
			showThirstystate = false;
		}

		public override void UpdateEquips() {
			if (accOsmoticPressureMonitor)
				showThirstystate = true;
		}
	}
}
