using Everglow.Commons.Mechanics.ElementalDebuff.Projectiles;
using ReLogic.Content;

namespace Everglow.Commons.Mechanics.ElementalDebuff.Debuffs;

public class CorrosionDebuff : ElementalDebuff
{
	public CorrosionDebuff()
		: base(ElementalDebuffType.Corrosion)
	{
		BuildUpMax = 1000;
		DurationMax = 720;
		DotDamage = 2;
		ProcDamage = 200;
	}

	public override Asset<Texture2D> Texture => ModAsset.Corrosion;

	public override Color Color => Color.Purple;

	public override void PostProc(NPC npc)
	{
		Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, npc.velocity, ModContent.ProjectileType<Corrosion_Projectile>(), 20, 0.5f, ai0: npc.whoAmI);
	}
}