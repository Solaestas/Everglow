namespace Everglow.Myth.MiscItems.Weapons.Slingshots.Projectiles;

internal class AmethystSlingshot : GemSlingshotProjectile
{
	public override void SetDef()
	{
		ShootProjType = ModContent.ProjectileType<AmethystBead>();
		TexPath = "MiscItems/Weapons/Slingshots/Projectiles/Amethyst";
		base.SetDef();
	}
}
