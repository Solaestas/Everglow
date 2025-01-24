namespace Everglow.Commons.Mechanics.ElementDebuff.Debuffs;

public class NervousImpairmentDebuff : ElementDebuff
{
	public NervousImpairmentDebuff()
		: base(ElementDebuffType.NervousImpairment, ModAsset.StarSlash, Color.LightSeaGreen)
	{
		BuildUpMax = 1000;
		DurationMax = 720;
		DotDamage = 25;
		ProcDamage = 0;
	}

	public override void OnProc(NPC npc)
	{
		base.OnProc(npc);
		npc.AddBuff(BuffID.Confused, 720);
	}

	public override void Draw(SpriteBatch spriteBatch) => throw new NotImplementedException();
}