using Everglow.Commons.Weapons.Slingshots;

namespace Everglow.Myth.Misc.Projectiles.Weapon.Ranged.Slingshots;

internal class AmbiguousNight : SlingshotProjectile
{
	public override void SetDef()
	{
		ShootProjType = ModContent.ProjectileType<AmbiguousNightAmmo>();
		SlingshotLength = 8;
		SplitBranchDis = 8;
	}
}
