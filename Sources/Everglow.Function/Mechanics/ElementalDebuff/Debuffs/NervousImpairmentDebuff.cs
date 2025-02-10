namespace Everglow.Commons.Mechanics.ElementalDebuff.Debuffs;

public class NervousImpairmentDebuff : ElementalDebuff
{
	public NervousImpairmentDebuff()
		: base(ElementalDebuffType.NervousImpairment, ModAsset.StarSlash, Color.LightSeaGreen)
	{
		BuildUpMax = 1000;
		DurationMax = 720;
		DotDamage = 25;
		ProcDamage = 0;
	}

	public override void PostProc(NPC npc)
	{
		npc.AddBuff(BuffID.Confused, 720);
	}
}