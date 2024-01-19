namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class ChlorophyteClub : ClubProj_metal
{
	public override void SetDef()
	{
		Beta = 0.0066f;
		MaxOmega = 0.471f;
		ReflectStrength = 3f;
		ReflectTexturePath = "Everglow/Myth/Misc/Projectiles/Weapon/Melee/Clubs/ChlorophyteClub_light";
	}
	public override void AI()
	{
		base.AI();
		if (FlyClubCooling > 0)
			FlyClubCooling--;
	}
	private int FlyClubCooling = 0;
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		if (FlyClubCooling <= 0 && Omega > 0.3f)
		{
			FlyClubCooling = (int)(170 - Omega * 150);
			Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ChlorophyteClub_fly>(), (int)(Projectile.damage * 0.3f), Projectile.knockBack * 0.4f, Projectile.owner);
		}
	}
}
