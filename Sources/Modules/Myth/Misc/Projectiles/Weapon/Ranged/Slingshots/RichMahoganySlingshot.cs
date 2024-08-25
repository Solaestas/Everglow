using Everglow.Commons.Weapons.Slingshots;

namespace Everglow.Myth.Misc.Projectiles.Weapon.Ranged.Slingshots;

internal class RichMahoganySlingshot : SlingshotProjectile
{
	public override void SetDef()
	{
		ShootProjType = ModContent.ProjectileType<NormalAmmo>();
	}
}
