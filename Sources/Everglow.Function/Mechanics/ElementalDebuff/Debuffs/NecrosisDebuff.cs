using ReLogic.Content;

namespace Everglow.Commons.Mechanics.ElementalDebuff.Debuffs;

public class NecrosisDebuff : ElementalDebuff
{
	public NecrosisDebuff()
		: base(ElementalDebuffType.Necrosis)
	{
		BuildUpMax = 1000;
		DurationMax = 60;
		DotDamage = 2;
		ProcDamage = 200;
	}

	public override Asset<Texture2D> Texture => ModAsset.Necrosis;

	public override Color Color => new Color(0.1f, 0.1f, 0.1f, 1f);
}