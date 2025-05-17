using ReLogic.Content;

namespace Everglow.Commons.Mechanics.ElementalDebuff.Debuffs;

public class BloodRotDebuff : ElementalDebuff
{
	public BloodRotDebuff()
		: base(ElementalDebuffType.BloodRot)
	{
		BuildUpMax = 1000;
		DurationMax = 720;
		DotDamage = 25;
		ProcDamage = 0;
	}

	public override Asset<Texture2D> Texture => ModAsset.BloodRot;

	public override Color Color => Color.DarkRed;

	public override void PostProc(NPC npc)
	{
		npc.AddBuff(BuffID.Bleeding, 720);
	}
}
