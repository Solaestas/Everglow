namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class ChlorophyteClub : ClubProj_Metal
{
	private int flyClubCooling = 0;

	public override void SetCustomDefaults()
	{
		Beta = 0.0066f;
		MaxOmega = 0.471f;
		ReflectStrength = 3f;
		ReflectTexturePath = ModAsset.ChlorophyteClub_light_Mod;
	}

	public override void AI()
	{
		base.AI();
		if (flyClubCooling > 0)
		{
			flyClubCooling--;
		}
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		if (flyClubCooling <= 0 && Omega > 0.3f)
		{
			flyClubCooling = (int)(170 - Omega * 150);
			Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ChlorophyteClub_fly>(), (int)(Projectile.damage * 0.3f), Projectile.knockBack * 0.4f, Projectile.owner, 0);
		}
	}
}