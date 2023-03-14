namespace Everglow.Myth.MiscItems.Weapons.Slingshots.Projectiles
{
	internal class RichMahoganySlingshot : SlingshotProjectile
	{
		public override void SetDef()
		{
			ShootProjType = ModContent.ProjectileType<NormalAmmo>();
		}
	}
}
