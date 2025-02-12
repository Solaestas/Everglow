using ReLogic.Content;

namespace Everglow.Commons.Mechanics.ElementalDebuff.Debuffs;

public class NervousImpairmentDebuff : ElementalDebuff
{
	public NervousImpairmentDebuff()
		: base(ElementalDebuffType.NervousImpairment)
	{
		BuildUpMax = 1000;
		DurationMax = 720;
		DotDamage = 25;
		ProcDamage = 0;
	}

	public override Asset<Texture2D> Texture => ModAsset.NervousImpairment;

	public override Color Color => Color.LightSeaGreen;

	public override void PostProc(NPC npc)
	{
		npc.AddBuff(BuffID.Confused, 720);
	}
}