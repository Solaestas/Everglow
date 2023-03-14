namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Slingshots.Projectiles
{
	internal class CactusSlingShot : SlingshotProjectile
	{
		public override void SetDef()
		{
			ShootProjType = ModContent.ProjectileType<NormalAmmo>();
		}
	}
}
