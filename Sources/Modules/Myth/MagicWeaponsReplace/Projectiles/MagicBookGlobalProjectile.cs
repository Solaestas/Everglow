using Terraria;
namespace Everglow.Myth.MagicWeaponsReplace.Projectiles;

internal class MagicBookGlobalProjectile : GlobalProjectile
{
	public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
	{
		if (projectile.type == ProjectileID.LunarFlare)
		{
			if (!crit)
			{
				//crit = Main.rand.NextFloat() < MoonNight.stars.Count * 0.002f;
			}
			else
			{
				//damage += (int)(MoonNight.stars.Count * 0.006f * damage);
			}
		}
	}
}
