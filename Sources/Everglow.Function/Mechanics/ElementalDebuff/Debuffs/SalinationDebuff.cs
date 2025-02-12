using ReLogic.Content;

namespace Everglow.Commons.Mechanics.ElementalDebuff.Debuffs;

public class SalinationDebuff : ElementalDebuff
{
	public SalinationDebuff()
		: base(ElementalDebuffType.Salination)
	{
		BuildUpMax = 1000;
		DurationMax = 720;
		DotDamage = 25;
		ProcDamage = 0;
	}

	public override Asset<Texture2D> Texture => ModAsset.Salination;

	public override Color Color => Color.LightBlue;

	public override void PostProc(NPC npc)
	{
		npc.AddBuff(BuffID.Frozen, 120);
	}
}