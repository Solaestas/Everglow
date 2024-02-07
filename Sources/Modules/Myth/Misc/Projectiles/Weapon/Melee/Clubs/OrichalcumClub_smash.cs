namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class OrichalcumClub_smash : ClubProj_Smash_metal
{
	public override string Texture => "Everglow/" + ModAsset.Melee_OrichalcumClubPath;
	public override void Smash(int level)
	{
		Player player = Main.player[Projectile.owner];
		if (level == 0)
		{
			for (int x = 0; x < 12; x++)
			{

				Projectile p0 = Projectile.NewProjectileDirect(null, player.Center + new Vector2(0, -4), Vector2.zeroVector, ModContent.ProjectileType<OrichalcumPedal_slash>(), Projectile.damage / 2, 0, player.whoAmI, 1.4f);
				p0.timeLeft = Main.rand.Next(120, 136) + x * 3;
			}
		}
		if (level == 1)
		{
			for (int x = 0; x < 17; x++)
			{
				Projectile p0 = Projectile.NewProjectileDirect(null, player.Center + new Vector2(0, -14), Vector2.zeroVector, ModContent.ProjectileType<OrichalcumPedal_slash>(), (int)(Projectile.damage * 0.85f), 0, player.whoAmI, 1.85f);
				p0.timeLeft = Main.rand.Next(118, 126) + x * 7;
			}
		}
		base.Smash(level);
	}
}
