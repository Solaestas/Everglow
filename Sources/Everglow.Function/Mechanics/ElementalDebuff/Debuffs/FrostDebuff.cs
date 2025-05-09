using ReLogic.Content;

namespace Everglow.Commons.Mechanics.ElementalDebuff.Debuffs;

public class FrostDebuff : ElementalDebuff
{
	public FrostDebuff()
		: base(ElementalDebuffType.Frost)
	{
		BuildUpMax = 1000;
		DurationMax = 720;
		DotDamage = 25;
		ProcDamage = 0;
	}

	public override Asset<Texture2D> Texture => ModAsset.Frost;

	public override Color Color => Color.LightSkyBlue;

	public override void PostProc(NPC npc)
	{
		npc.AddBuff(BuffID.Frozen, 120);
	}
}
