using Everglow.Commons.Templates.Weapons.Clubs;

namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class SpikeClub_smash : ClubProj_Smash_metal
{
	public override string Texture => "Everglow/" + ModAsset.SpikeClub_Path;
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
	{
		int k = (int)(Omega * 10);
		k = Main.rand.Next(k * 2);
		for (int x = 0; x < k; x++)
		{
			NPC.HitInfo hit = modifiers.ToHitInfo(Projectile.damage * 0.5f * Omega, Main.rand.NextFloat(100f) < Main.player[Projectile.owner].GetTotalCritChance(Projectile.DamageType), 0);
			target.StrikeNPC(hit, true, true);
			NetMessage.SendStrikeNPC(target, hit);
		}
		modifiers.Knockback *= 0.4f;
	}
	public override void Smash(int level = 0)
	{
		Player player = Main.player[Projectile.owner];
		if(level == 0)
		{
			for (int x = 0; x < 8; x++)
			{
				Vector2 v = new Vector2(0, Main.rand.NextFloat(4f, 8f)).RotatedByRandom(Math.PI * 2) * 5f;
				Vector2 v2 = new Vector2(0, 124).RotatedBy(Main.rand.NextFloat(-1f, 1f));
				Projectile p0 = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), player.Center - v * 3 + v2 + new Vector2(0, -74), v, ModContent.ProjectileType<SpikeClubSlash>(), Projectile.damage / 2, 0, player.whoAmI, Main.rand.NextFloat(-0.05f, 0.05f));
				p0.timeLeft = Main.rand.Next(120, 136);
			}
		}
		if (level == 1)
		{
			for (int x = 0; x < 24; x++)
			{
				Vector2 v = new Vector2(0, Main.rand.NextFloat(6f, 12f)).RotatedByRandom(Math.PI * 2) * 5f;
				Vector2 v2 = new Vector2(0, 204).RotatedBy(Main.rand.NextFloat(-1f, 1f));
				Projectile p0 = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), player.Center - v * 3 + v2 + new Vector2(0, -134), v, ModContent.ProjectileType<SpikeClubSlash>(), Projectile.damage / 2, 0, player.whoAmI, Main.rand.NextFloat(-0.05f, 0.05f));
				p0.timeLeft = Main.rand.Next(118, 126) + x * 3;
			}
		}
		base.Smash(level);
	}
}
