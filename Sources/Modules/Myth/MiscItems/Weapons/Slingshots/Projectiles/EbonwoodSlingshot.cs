namespace Everglow.Myth.MiscItems.Weapons.Slingshots.Projectiles
{
	internal class EbonwoodSlingshot : SlingshotProjectile
	{
		public override void SetDef()
		{
			ShootProjType = ModContent.ProjectileType<NormalAmmo>();
		}
	}
}
