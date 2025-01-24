using Everglow.Commons.Mechanics.ElementDebuff.Projectiles;

namespace Everglow.Commons.Mechanics.ElementDebuff.Debuffs;

public class CorrosionDebuff : ElementDebuff
{
	public CorrosionDebuff()
		: base(ElementDebuffType.Corrosion, ModAsset.StarSlash, Color.Purple)
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

	public override void Draw(SpriteBatch spriteBatch) => throw new NotImplementedException();
}