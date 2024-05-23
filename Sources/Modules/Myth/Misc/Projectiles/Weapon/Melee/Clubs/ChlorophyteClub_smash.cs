namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class ChlorophyteClub_smash : ClubProj_Smash_metal
{
	public override string Texture => "Everglow/" + ModAsset.Melee_ChlorophyteClub_Path;
	public override void Smash(int level)
	{
		Player player = Main.player[Projectile.owner];
		if (level == 0)
		{
			for (int i = 0; i < 3; i++)
			{
				Vector2 v0 = new Vector2(0, -1 * player.gravDir).RotatedBy((i - 1) / 6f * MathHelper.TwoPi);
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, v0, ModContent.ProjectileType<ChlorophyteClub_fly_2>(), (int)(Projectile.damage * 0.16f), Projectile.knockBack * 0.4f, Projectile.owner, 0.5f);
			}
			Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ChlorophyteClub_VFX_2>(), Projectile.damage, Projectile.knockBack * 0.4f, Projectile.owner, 0.4f, player.gravDir);
		}
		if (level == 1)
		{
			for (int i = 0; i < 5; i++)
			{
				Vector2 v0 = new Vector2(0, -1 * player.gravDir).RotatedBy((i - 2) / 10f * MathHelper.TwoPi);
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, v0, ModContent.ProjectileType<ChlorophyteClub_fly_2>(), (int)(Projectile.damage * 0.3f), Projectile.knockBack * 0.4f, Projectile.owner, 1.1f);
			}
			for (int i = 0; i < 6; i++)
			{
				Vector2 v0 = new Vector2(0, -1 * player.gravDir).RotatedBy((i - 2.5f) / 10f * MathHelper.TwoPi);
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, v0, ModContent.ProjectileType<ChlorophyteClub_fly_2>(), (int)(Projectile.damage * 0.3f), Projectile.knockBack * 0.4f, Projectile.owner, 0.8f);
			}
			Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ChlorophyteClub_VFX_2>(), Projectile.damage, Projectile.knockBack * 0.4f, Projectile.owner, 1.2f, player.gravDir);
		}
		base.Smash(level);
	}
}
