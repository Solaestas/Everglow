namespace Everglow.Myth.MiscItems.Weapons.Slingshots.Projectiles
{
	internal class PalmWoodSlingshot : SlingshotProjectile
	{
		public override void SetDef()
		{
			ShootProjType = ModContent.ProjectileType<NormalAmmo>();
		}
	}
}
