namespace Everglow.Myth.Misc.Projectiles.Weapon.Ranged.Slingshots;

internal class DiamondSlingshot : GemSlingshotProjectile
{
	public override void SetDef()
	{
		ShootProjType = ModContent.ProjectileType<DiamondBead>();
		TexPath = "Misc/Projectiles/Weapon/Ranged/Slingshots/Diamond";
		base.SetDef();
	}
}
