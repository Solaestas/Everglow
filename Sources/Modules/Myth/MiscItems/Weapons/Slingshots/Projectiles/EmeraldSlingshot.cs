namespace Everglow.Myth.MiscItems.Weapons.Slingshots.Projectiles;

internal class EmeraldSlingshot : GemSlingshotProjectile
{
	public override void SetDef()
	{
		ShootProjType = ModContent.ProjectileType<EmeraldBead>();
		TexPath = "MiscItems/Weapons/Slingshots/Projectiles/Emerald";
		base.SetDef();
	}
}
