namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class TitaniumClub_smash : ClubProjSmash_Reflective
{
	public override string Texture => ModAsset.TitaniumClub_Mod;

	public override string TrailColorTex() => ModAsset.TitaniumClub_light_Mod;

	public override void SetDef()
	{
		ReflectionStrength = 5f;
		base.SetDef();
	}

	public override void Smash(int level)
	{
		if (level == 0)
		{
			for (int x = 0; x < 6; x++)
			{
				Vector2 v0 = new Vector2(0, -Main.rand.NextFloat(7, 14f) * Owner.gravDir).RotatedBy(Main.rand.NextFloat(-1.7f, 1.7f));
				Projectile p0 = Projectile.NewProjectileDirect(null, Owner.Center, v0, ModContent.ProjectileType<Titanium_Fragment>(), Projectile.damage / 18, 0, Owner.whoAmI, 72);
				if (Owner.ownedProjectileCounts[ModContent.ProjectileType<Titanium_Fragment>()] > 120)
				{
					break;
				}
			}
			foreach (Projectile projectile in Main.projectile)
			{
				if (projectile != null && projectile.active)
				{
					if (projectile.type == ModContent.ProjectileType<Titanium_Fragment>())
					{
						if (projectile.owner == Projectile.owner)
						{
							Vector2 v0 = new Vector2(0, -Main.rand.NextFloat(7, 12f) * Owner.gravDir).RotatedBy(Main.rand.NextFloat(-1.7f, 1.7f));
							projectile.velocity = v0;
							Titanium_Fragment tF = projectile.ModProjectile as Titanium_Fragment;
							if (tF != null)
							{
								tF.AITimer = Main.rand.Next(20, 50);
							}
						}
					}
				}
			}

			Projectile.NewProjectileDirect(null, Owner.gravDir == 1 ? Owner.Bottom : Owner.Top, Vector2.zeroVector, ModContent.ProjectileType<TitaniumClub_smash_explosion>(), 0, 0, Owner.whoAmI, 20, 0, Owner.gravDir);
		}
		if (level == 1)
		{
			for (int x = 0; x < 12; x++)
			{
				Vector2 v0 = new Vector2(0, -Main.rand.NextFloat(7, 24f) * Owner.gravDir).RotatedBy(Main.rand.NextFloat(-1.7f, 1.7f));
				Projectile p0 = Projectile.NewProjectileDirect(null, Owner.Center, v0, ModContent.ProjectileType<Titanium_Fragment>(), Projectile.damage / 13, 0, Owner.whoAmI, 144);
				if (Owner.ownedProjectileCounts[ModContent.ProjectileType<Titanium_Fragment>()] > 120)
				{
					break;
				}
			}
			for (int x = 0; x < 12; x++)
			{
				Vector2 v0 = new Vector2(0, -Main.rand.NextFloat(7, 24f) * Owner.gravDir).RotatedBy(Main.rand.NextFloat(-1.7f, 1.7f));
				Projectile p0 = Projectile.NewProjectileDirect(null, Owner.Center, v0, ModContent.ProjectileType<Titanium_Fragment>(), Projectile.damage / 13, 0, Owner.whoAmI, 72);
				if (Owner.ownedProjectileCounts[ModContent.ProjectileType<Titanium_Fragment>()] > 120)
				{
					break;
				}
			}
			foreach (Projectile projectile in Main.projectile)
			{
				if (projectile != null && projectile.active)
				{
					if (projectile.type == ModContent.ProjectileType<Titanium_Fragment>())
					{
						if (projectile.owner == Projectile.owner)
						{
							Vector2 v0 = new Vector2(0, -Main.rand.NextFloat(7, 12f) * Owner.gravDir).RotatedBy(Main.rand.NextFloat(-1.7f, 1.7f)) * projectile.ai[0] / 90f;
							projectile.velocity = v0;
							Titanium_Fragment tF = projectile.ModProjectile as Titanium_Fragment;
							if (tF != null)
							{
								tF.AITimer = Main.rand.Next(20, 50);
							}
						}
					}
				}
			}

			Projectile.NewProjectileDirect(null, Owner.gravDir == 1 ? Owner.Bottom : Owner.Top, Vector2.zeroVector, ModContent.ProjectileType<TitaniumClub_smash_explosion>(), 0, 0, Owner.whoAmI, 30, 0, Owner.gravDir);
		}
		base.Smash(level);
	}
}