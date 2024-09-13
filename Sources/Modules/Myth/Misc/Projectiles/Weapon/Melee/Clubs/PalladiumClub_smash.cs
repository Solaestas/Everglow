namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class PalladiumClub_smash : ClubProj_Smash_metal
{
	public override string Texture => "Everglow/" + ModAsset.PalladiumClub_Path;
	public override void Smash(int level)
	{
		if (level == 0)
		{
			Projectile p0 = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.zeroVector, ModContent.ProjectileType<PalladiumClub_small>(), (int)(Projectile.damage * 0.4f), Projectile.knockBack * 0.4f, Projectile.owner, 0.24f);
			p0.rotation = Main.rand.NextFloat(6.283f);
		}
		if (level == 1)
		{
			Projectile p0 = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.zeroVector, ModContent.ProjectileType<PalladiumClub_small>(), (int)(Projectile.damage * 0.72f), Projectile.knockBack * 0.4f, Projectile.owner, 0.5f);
			p0.rotation = Main.rand.NextFloat(6.283f);
		}
		base.Smash(level);
	}
}
