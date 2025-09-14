using Everglow.Myth.Misc.Projectiles.Weapon.Magic.FireFeatherMagic;
using Terraria.DataStructures;

namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class CobaltClub_smash : ClubProjSmash_Reflect
{
	public override string Texture => ModAsset.CobaltClub_Mod;

	public override string TrailColorTex() => ModAsset.CobaltClub_light_Mod;

	public override void OnSpawn(IEntitySource source)
	{
		Projectile.ai[1] = -60;
		base.OnSpawn(source);
	}

	public override void AI()
	{
		Projectile.ai[1]++;
		if (Projectile.ai[1] % 6 == 5 && Projectile.ai[1] > 0)
		{
			Projectile.ai[1] = 0;
			Vector2 addPos = new Vector2(0, -54).RotatedByRandom(6.283) + new Vector2(0, -60 * Owner.gravDir);
			Projectile p0 = Projectile.NewProjectileDirect(null, Projectile.Center + addPos + new Vector2(0, -60 * Owner.gravDir), -addPos * 0.1f + new Vector2(0, 12 * Owner.gravDir), ModContent.ProjectileType<CobaltClub_falling_Shoot>(), Projectile.damage / 4, 0, Owner.whoAmI, 1.85f);
		}
		base.AI();
	}
}