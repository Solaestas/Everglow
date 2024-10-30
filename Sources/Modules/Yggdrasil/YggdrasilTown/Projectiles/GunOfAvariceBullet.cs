using Everglow.Yggdrasil.YggdrasilTown.Items.Weapons;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class GunOfAvariceBullet : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 8;
		Projectile.height = 8;
		Projectile.aiStyle = 1;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.DamageType = DamageClass.Ranged;
		Projectile.penetrate = 1;
		Projectile.timeLeft = 600;
		Projectile.alpha = 255;
		Projectile.ignoreWater = true;
		Projectile.tileCollide = true;
		Projectile.extraUpdates = 1;

		AIType = ProjectileID.Bullet;
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		target.value += GunOfAvarice.TargetValueBonusInCopper;
	}

	public override void AI()
	{
	}
}