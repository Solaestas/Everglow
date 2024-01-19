using Everglow.Myth.Misc.Projectiles.Accessory;

namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class CrystalClub_smash : ClubProj_Smash_metal
{
	public override void SetDef()
	{
		ReflectStrength = 8f;
		base.SetDef();
	}
	public override string Texture => "Everglow/" + ModAsset.Melee_CrystalClubPath;
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		for (int x = 0; x < 2; x++)
		{
			Vector2 velocity = new Vector2(0, Main.rand.NextFloat(2f, 6f)).RotatedByRandom(6.283) - Projectile.velocity * 0.2f;
			var p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), target.Center + velocity * -2, velocity, ModContent.ProjectileType<IchorCurrent>(), Projectile.damage / 3, Projectile.knockBack, Projectile.owner, 3f/*If ai[0] equal to 3, another ai will be execute*/);
			p.friendly = false;
			p.CritChance = Projectile.CritChance;
		}
		target.AddBuff(BuffID.Ichor, (int)(818 * Omega));
	}
	public override void AI()
	{
		base.AI();
		Player player = Main.player[Projectile.owner];
		for (float x = 0; x < Omega + 0.6 + player.velocity.Length() / 180f; x += 0.05f)
		{
			Vector2 pos = (trailVecs2.ToArray()[trailVecs2.Count - 1] + trailVecs2.ToArray()[trailVecs2.Count - 1]) / 2f;
			float factor = Main.rand.NextFloat(0, 1f);
			if (trailVecs2.Count > 1)
			{
				pos = (trailVecs2.ToArray()[trailVecs2.Count - 1] * factor + trailVecs2.ToArray()[trailVecs2.Count - 2] * (1 - factor));
			}
			pos = (pos - Projectile.Center) * 0.9f + Projectile.Center - player.velocity * factor;
			Vector2 vel = Vector2.zeroVector;
			if (trailVecs2.Count > 1)
			{
				vel = trailVecs2.ToArray()[trailVecs2.Count - 1] - trailVecs2.ToArray()[trailVecs2.Count - 2];
			}
			if (trailVecs2.Count > 2)
			{
				vel = (trailVecs2.ToArray()[trailVecs2.Count - 1] - trailVecs2.ToArray()[trailVecs2.Count - 2]) * factor + (trailVecs2.ToArray()[trailVecs2.Count - 2] - trailVecs2.ToArray()[trailVecs2.Count - 3]) * (1 - factor);
			}
			vel += player.velocity;
			vel *= Main.rand.NextFloat(0.1f, 0.3f);
		}
	}
}