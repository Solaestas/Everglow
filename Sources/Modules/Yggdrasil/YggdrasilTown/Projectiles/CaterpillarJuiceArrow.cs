namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class CaterpillarJuiceArrow : ModProjectile
{
	private const int MaxDuration = 6 * 60;
	private const int MinDuration = 4 * 60;

	public override void SetDefaults()
	{
		Projectile.width = 10;
		Projectile.height = 10;

		Projectile.arrow = true;
		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Ranged;

		Projectile.penetrate = 1;

		Projectile.timeLeft = 1200;
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		Main.rand.Next(MinDuration, MaxDuration);
		target.AddBuff(BuffID.Oiled, 180);
	}
}