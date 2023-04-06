namespace Everglow.Food.InfoDisplays;

internal class ThirstystateInfoDisplay : InfoDisplay
{
	public override bool Active()
	{
		return Main.LocalPlayer.GetModPlayer<ThirstystateInfoDisplayplayer>().ShowThirstystate;
	}

	public override string DisplayValue(ref Color displayColor)
	{
		bool Thirstystate = Main.LocalPlayer.GetModPlayer<FoodModPlayer>().Thirstystate;
		if (Thirstystate)
			return Terraria.Localization.Language.GetTextValue("Mods.Everglow.Food.InfoDisplay.Thirsty");
		else
		{
			return Terraria.Localization.Language.GetTextValue("Mods.Everglow.Food.InfoDisplay.NotThirsty");
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
