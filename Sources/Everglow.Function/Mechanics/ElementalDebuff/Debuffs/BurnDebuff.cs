namespace Everglow.Commons.Mechanics.ElementalDebuff.Debuffs;

public class BurnDebuff : ElementalDebuff
{
	public BurnDebuff()
		: base(ElementalDebuffType.Burn, ModAsset.StarSlash, Color.Orange)
	{
		BuildUpMax = 1000;
		DurationMax = 60;
		DotDamage = 2;
		ProcDamage = 200;
	}

	public override void PostProc(NPC npc)
	{
		npc.AddBuff(BuffID.OnFire3, 1200);
	}
}