using Everglow.Myth.Misc.Projectiles.Weapon.Magic.FireFeatherMagic;
using Terraria.DataStructures;

namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class CobaltClub_smash : ClubProj_Smash_metal
{
	public override string Texture => "Everglow/" + ModAsset.CobaltClub_Path;
	public override string TrailColorTex() => "Everglow/" + ModAsset.CobaltClub_light_Path;
	public override void OnSpawn(IEntitySource source)
	{
		Projectile.ai[1] = -60;
		base.OnSpawn(source);
	}
	public override void AI()
	{
		Player player = Main.player[Projectile.owner];
		Projectile.ai[1]++;
		if(Projectile.ai[1] % 6 == 5 && Projectile.ai[1] > 0)
		{
			Projectile.ai[1] = 0;
			Vector2 addPos = new Vector2(0, -54).RotatedByRandom(6.283) + new Vector2(0, -60 * player.gravDir);
			Projectile p0 = Projectile.NewProjectileDirect(null, Projectile.Center + addPos + new Vector2(0, -60 * player.gravDir), -addPos * 0.1f + new Vector2(0, 12 * player.gravDir), ModContent.ProjectileType<CobaltClub_falling_Shoot>(), Projectile.damage / 4, 0, player.whoAmI, 1.85f);
		}
		base.AI();
	}
}
