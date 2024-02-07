namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class TitaniumClub_smash : ClubProj_Smash_metal
{
	public override string Texture => "Everglow/" + ModAsset.Melee_TitaniumClub_Path;
	public override string TrailColorTex() => "Everglow/" + ModAsset.TitaniumClub_light_Path;
	public override void SetDef()
	{
		ReflectStrength = 5f;
		base.SetDef();
	}
	public override void Smash(int level)
	{
		Player player = Main.player[Projectile.owner];
		if (level == 0)
		{
			for (int x = 0; x < 24; x++)
			{
				Vector2 v0 = new Vector2(0, -Main.rand.NextFloat(7, 14f) * player.gravDir).RotatedBy(Main.rand.NextFloat(-1.7f, 1.7f));
				Projectile p0 = Projectile.NewProjectileDirect(null, player.Center, v0, ModContent.ProjectileType<Titanium_Fragment>(), Projectile.damage / 12, 0, player.whoAmI, 72);
				if (player.ownedProjectileCounts[ModContent.ProjectileType<Titanium_Fragment>()] > 120)
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
							Vector2 v0 = new Vector2(0, -Main.rand.NextFloat(7, 12f) * player.gravDir).RotatedBy(Main.rand.NextFloat(-1.7f, 1.7f));
							projectile.velocity = v0;
							Titanium_Fragment tF = projectile.ModProjectile as Titanium_Fragment;
							if (tF != null)
							{
								tF.AITimer = Main.rand.Next(-20, 20);
							}
						}
					}
				}
			}
		}
		if (level == 1)
		{
			for (int x = 0; x < 34; x++)
			{
				Vector2 v0 = new Vector2(0, -Main.rand.NextFloat(7, 24f) * player.gravDir).RotatedBy(Main.rand.NextFloat(-1.7f, 1.7f));
				Projectile p0 = Projectile.NewProjectileDirect(null, player.Center, v0, ModContent.ProjectileType<Titanium_Fragment>(), Projectile.damage / 10, 0, player.whoAmI, 144);
				if (player.ownedProjectileCounts[ModContent.ProjectileType<Titanium_Fragment>()] > 120)
				{
					break;
				}
			}
			for (int x = 0; x < 34; x++)
			{
				Vector2 v0 = new Vector2(0, -Main.rand.NextFloat(7, 24f) * player.gravDir).RotatedBy(Main.rand.NextFloat(-1.7f, 1.7f));
				Projectile p0 = Projectile.NewProjectileDirect(null, player.Center, v0, ModContent.ProjectileType<Titanium_Fragment>(), Projectile.damage / 10, 0, player.whoAmI, 72);
				if (player.ownedProjectileCounts[ModContent.ProjectileType<Titanium_Fragment>()] > 120)
				{
					break;
				}
			}
			foreach(Projectile projectile in Main.projectile)
			{
				if(projectile != null && projectile.active)
				{
					if(projectile.type == ModContent.ProjectileType<Titanium_Fragment>())
					{
						if(projectile.owner == Projectile.owner)
						{
							Vector2 v0 = new Vector2(0, -Main.rand.NextFloat(7, 12f) * player.gravDir).RotatedBy(Main.rand.NextFloat(-1.7f, 1.7f)) * projectile.ai[0] / 90f;
							projectile.velocity = v0;
							Titanium_Fragment tF = projectile.ModProjectile as Titanium_Fragment;
							if(tF != null)
							{
								tF.AITimer = Main.rand.Next(-20, 20);
							}
						}
					}
				}
			}
		}
		base.Smash(level);
	}
}
