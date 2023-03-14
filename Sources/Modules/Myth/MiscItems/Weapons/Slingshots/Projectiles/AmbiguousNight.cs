namespace Everglow.Myth.MiscItems.Weapons.Slingshots.Projectiles
{
	internal class AmbiguousNight : SlingshotProjectile
	{
		public override void SetDef()
		{
			ShootProjType = ModContent.ProjectileType<AmbiguousNightAmmo>();
			SlingshotLength = 8;
			SplitBranchDis = 8;
		}
	}
}
