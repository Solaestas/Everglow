namespace Everglow.Commons.Mechanics.ElementalDebuff;

public class ElementalDebuffPlayer : ModPlayer
{
	public float ElementPenetration { get; set; }

	public override void ResetEffects()
	{
		ElementPenetration = 0;
	}
}