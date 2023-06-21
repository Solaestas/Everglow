namespace Everglow.Myth.Misc.Projectiles.Weapon.Ranged.Slingshots;

internal class EmeraldSlingshot : GemSlingshotProjectile
{
	public override void SetDef()
	{
		ShootProjType = ModContent.ProjectileType<EmeraldBead>();
		TexPath = "Misc/Projectiles/Weapon/Ranged/Slingshots/Emerald";
		base.SetDef();
	}
}
