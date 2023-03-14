namespace Everglow.Myth.TheTusk.Projectiles.Weapon
{
	class ToothMagicHit : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 68;
			Projectile.height = 68;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 120;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 3;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			return false;
		}
	}
}
