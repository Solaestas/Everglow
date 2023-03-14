namespace Everglow.Myth.MiscItems.Weapons.Slingshots.Projectiles
{
	internal class WoodSlingshot : SlingshotProjectile
	{
		public override void SetDef()
		{
			ShootProjType = ModContent.ProjectileType<NormalAmmo>();
		}
	}
}
