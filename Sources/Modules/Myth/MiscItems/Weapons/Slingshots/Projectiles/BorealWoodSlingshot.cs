namespace Everglow.Myth.MiscItems.Weapons.Slingshots.Projectiles;

internal class BorealWoodSlingshot : SlingshotProjectile
{
	public override void SetDef()
	{
		ShootProjType = ModContent.ProjectileType<NormalAmmo>();
	}
}
