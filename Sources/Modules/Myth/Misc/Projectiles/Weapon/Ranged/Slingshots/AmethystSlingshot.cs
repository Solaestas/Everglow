namespace Everglow.Myth.Misc.Projectiles.Weapon.Ranged.Slingshots;

internal class AmethystSlingshot : GemSlingshotProjectile
{
	public override void SetDef()
	{
		ShootProjType = ModContent.ProjectileType<AmethystBead>();
		TexPath = "Misc/Projectiles/Weapon/Ranged/Slingshots/Amethyst";
		base.SetDef();
	}
}
