namespace Everglow.Commons.Mechanics.ElementalDebuff.Debuffs;

public class NecrosisDebuff : ElementalDebuff
{
	public NecrosisDebuff()
		: base(ElementalDebuffType.Necrosis, ModAsset.StarSlash, Color.Gray)
	{
		BuildUpMax = 1000;
		DurationMax = 60;
		DotDamage = 2;
		ProcDamage = 200;
	}
}