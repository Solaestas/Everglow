using Everglow.Commons.Mechanics.ElementalDebuff.Projectiles;

namespace Everglow.Commons.Mechanics.ElementalDebuff.Debuffs;

public class CorrosionDebuff : ElementalDebuff
{
	public CorrosionDebuff()
		: base(ElementalDebuffType.Corrosion, ModAsset.StarSlash, Color.Purple)
	{
		BuildUpMax = 1000;
		DurationMax = 720;
		DotDamage = 2;
		ProcDamage = 200;
	}

	public override void PostProc(NPC npc)
	{
		Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, npc.velocity, ModContent.ProjectileType<Corrosion_Projectile>(), 20, 0.5f, ai0: npc.whoAmI);
	}
}