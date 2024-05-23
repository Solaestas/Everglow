using Everglow.Myth.Misc.Dusts;

namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class CrystalClub_smash : ClubProj_Smash_metal
{
	public override void SetDef()
	{
		ReflectStrength = 8f;
		base.SetDef();
	}
	public override string Texture => "Everglow/" + ModAsset.Melee_CrystalClub_Path;
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		int type = 0;
		switch (Main.rand.Next(3))
		{
			case 0:
				type = DustID.BlueCrystalShard;
				break;
			case 1:
				type = DustID.PinkCrystalShard;
				break;
			case 2:
				type = DustID.PurpleCrystalShard;
				break;
		}
		for (float d = 0.1f; d < Omega; d += 0.04f)
		{
			var D = Dust.NewDustDirect(target.Center - new Vector2(4)/*Dust的Size=8x8*/, 0, 0, type, 0, 0, 150, default, Main.rand.NextFloat(0.4f, 1.1f));
			D.noGravity = true;
			D.velocity = new Vector2(0, Main.rand.NextFloat(Omega * 25f)).RotatedByRandom(6.283);
		}
	}
	public override void AI()
	{
		base.AI();
		Player player = Main.player[Projectile.owner];
		for (float x = 0; x < Omega + 0.6 + player.velocity.Length() / 180f; x += 0.15f)
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
			vel *= 0.0001f;
			Dust d0 = Dust.NewDustDirect(pos,0,0,ModContent.DustType<CrystalScale>());
			d0.alpha = 150;
			d0.velocity = vel;
		}
	}
}