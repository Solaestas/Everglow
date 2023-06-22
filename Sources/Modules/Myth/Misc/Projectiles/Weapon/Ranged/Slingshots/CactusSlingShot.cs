namespace Everglow.Myth.Misc.Projectiles.Weapon.Ranged.Slingshots;

internal class CactusSlingShot : SlingshotProjectile
{
	public override void SetDef()
	{
		ShootProjType = ModContent.ProjectileType<NormalAmmo>();
	}
}
