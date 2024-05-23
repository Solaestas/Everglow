namespace Everglow.SpellAndSkull.Projectiles.MagnetSphere;

internal class MagnetSphereBook : MagicBookProjectile
{
	public override void SetDef()
	{
		DustType = DustID.Electric;
		ItemType = ItemID.MagnetSphere;
		ProjType = ModContent.ProjectileType<MagnetSphereII>();
		MulStartPosByVelocity = 6f;
		MulVelocity = 2f;
		effectColor = new Color(15, 185, 115, 100);
	}
}