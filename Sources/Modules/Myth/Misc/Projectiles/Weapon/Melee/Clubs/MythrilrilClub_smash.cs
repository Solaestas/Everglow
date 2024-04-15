namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class MythrilClub_smash : ClubProj_Smash_metal
{
	public override string Texture => "Everglow/" + ModAsset.Melee_MythrilClubPath;
	public override string TrailColorTex() => "Everglow/" + ModAsset.MythrilClub_lightPath;
	public override void Smash(int level)
	{
		if (level == 0)
		{
			for(int i = -3;i <=3;i++)
			{
				if(i == 0)
				{
					continue;
				}
				for (int j = -3; j <= 3; j++)
				{
					Projectile p0 = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + new Vector2(i * 90 + Main.rand.NextFloat(-45, 45), 60), new Vector2(0, -8 + Math.Abs(i * 1.5f) + Main.rand.NextFloat(-3, 3)), ModContent.ProjectileType<MythrilClub_smashMagicBall>(), (int)(Projectile.damage * 0.4f), Projectile.knockBack * 0.4f, Projectile.owner, 0.24f);
					p0.rotation = Main.rand.NextFloat(6.283f);
					p0.timeLeft = Math.Abs(i * 8) + 120 + j * 2;
					int count = 0;
					while (TileCollisionUtils.PlatformCollision(p0.Center))
					{
						p0.Center += new Vector2(0, -8);
						if (count > 200)
						{
							break;
						}
					}
					count = 0;
					while (!TileCollisionUtils.PlatformCollision(p0.Center) && !TileCollisionUtils.PlatformCollision(p0.Center + new Vector2(0, 8)))
					{
						p0.Center += new Vector2(0, 8);
						if (count > 200)
						{
							break;
						}
					}
				}
			}
		}
		if (level == 1)
		{
			for (int i = -6; i <= 6; i++)
			{
				for (int j = -3; j <= 3; j++)
				{
					if (i == 0)
					{
						continue;
					}
					Projectile p0 = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + new Vector2(i * 90 + Main.rand.NextFloat(-45, 45), 60), new Vector2(0, -9 + Math.Abs(i * 0.7f) + Main.rand.NextFloat(-3, 3)), ModContent.ProjectileType<MythrilClub_smashMagicBall>(), (int)(Projectile.damage * 0.4f), Projectile.knockBack * 0.4f, Projectile.owner, 0.24f);
					p0.rotation = Main.rand.NextFloat(6.283f);
					p0.timeLeft = Math.Abs(i * 8) + 120 + j * 2;
					int count = 0;
					while (TileCollisionUtils.PlatformCollision(p0.Center))
					{
						p0.Center += new Vector2(0, -8);
						if (count > 200)
						{
							break;
						}
					}
					count = 0;
					while (!TileCollisionUtils.PlatformCollision(p0.Center) && !TileCollisionUtils.PlatformCollision(p0.Center + new Vector2(0, 8)))
					{
						p0.Center += new Vector2(0, 8);
						if (count > 200)
						{
							break;
						}
					}
				}
			}
		}
		base.Smash(level);
	}
}