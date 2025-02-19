namespace Everglow.Commons.Mechanics.ElementalDebuff;

public class ElementalDebuffPlayer : ModPlayer
{
	public ElementalPenetrationData ElementalPenetration { get; } = new();

	public override void ResetEffects()
	{
		ElementalPenetration.ResetEffects();
	}
}