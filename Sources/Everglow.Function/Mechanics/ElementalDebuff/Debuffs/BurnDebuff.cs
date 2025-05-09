using ReLogic.Content;

namespace Everglow.Commons.Mechanics.ElementalDebuff.Debuffs;

public class BurnDebuff : ElementalDebuff
{
	public BurnDebuff()
		: base(ElementalDebuffType.Burn)
	{
		BuildUpMax = 1000;
		DurationMax = 720;
		DotDamage = 2;
		ProcDamage = 200;
	}

	public override Asset<Texture2D> Texture => ModAsset.Burn;

	public override Color Color => Color.Orange;

	public override void PostProc(NPC npc)
	{
		npc.AddBuff(BuffID.OnFire3, 1200);
	}
}