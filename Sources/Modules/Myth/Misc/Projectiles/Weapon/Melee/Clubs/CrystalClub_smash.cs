using Everglow.Commons.VFX.CommonVFXDusts;
using Everglow.Myth.Misc.Dusts;

namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class CrystalClub_smash : ClubProjSmash_Reflect
{
	public override void SetDef()
	{
		ReflectStrength = 8f;
		base.SetDef();
	}

	public override string Texture => ModAsset.CrystalClub_Mod;

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
		for (float x = 0; x < Omega + 0.6 + Owner.velocity.Length() / 180f; x += 0.15f)
		{
			Vector2 pos = (SmashTrailVecs.ToArray()[SmashTrailVecs.Count - 1] + SmashTrailVecs.ToArray()[SmashTrailVecs.Count - 1]) / 2f;
			float factor = Main.rand.NextFloat(0, 1f);
			if (SmashTrailVecs.Count > 1)
			{
				pos = SmashTrailVecs.ToArray()[SmashTrailVecs.Count - 1] * factor + SmashTrailVecs.ToArray()[SmashTrailVecs.Count - 2] * (1 - factor);
			}
			pos = (pos - Projectile.Center) * 0.9f + Projectile.Center - Owner.velocity * factor;
			Vector2 vel = Vector2.zeroVector;
			if (SmashTrailVecs.Count > 1)
			{
				vel = SmashTrailVecs.ToArray()[SmashTrailVecs.Count - 1] - SmashTrailVecs.ToArray()[SmashTrailVecs.Count - 2];
			}
			if (SmashTrailVecs.Count > 2)
			{
				vel = (SmashTrailVecs.ToArray()[SmashTrailVecs.Count - 1] - SmashTrailVecs.ToArray()[SmashTrailVecs.Count - 2]) * factor + (SmashTrailVecs.ToArray()[SmashTrailVecs.Count - 2] - SmashTrailVecs.ToArray()[SmashTrailVecs.Count - 3]) * (1 - factor);
			}
			vel += Owner.velocity;
			vel *= 0.0001f;
			Dust d0 = Dust.NewDustDirect(pos, 0, 0, ModContent.DustType<CrystalScale>());
			d0.alpha = 150;
			d0.velocity = vel;
		}
	}

	public override void Smash(int level)
	{
		if (level == 0)
		{
			for (int t = 0; t < 5; t++)
			{
				Vector2 vel = new Vector2(0, -Main.rand.NextFloat(7, 21) * Owner.gravDir).RotatedBy((t - 2) * 0.75f + Main.rand.NextFloat(-0.24f, 0.24f));
				var crystal = new HolyCrystal
				{
					velocity = vel,
					Active = true,
					Visible = true,
					position = Owner.Bottom,
					maxTime = Main.rand.Next(76, 84),
					scale = Main.rand.Next(8, 15),
					ai = new float[] { Main.rand.NextFloat(100f), Main.rand.NextFloat(1f), Projectile.damage * 0.5f },
				};
				Ins.VFXManager.Add(crystal);
			}
		}
		else
		{
			for (int t = 0; t < 9; t++)
			{
				Vector2 vel = new Vector2(0, -Main.rand.NextFloat(7, 30) * Owner.gravDir).RotatedBy((t - 4) * 0.4f + Main.rand.NextFloat(-0.24f, 0.24f));
				var crystal = new HolyCrystal
				{
					velocity = vel,
					Active = true,
					Visible = true,
					position = Owner.Bottom,
					maxTime = Main.rand.Next(86, 94),
					scale = Main.rand.Next(11, 18),
					ai = new float[] { Main.rand.NextFloat(100f), Main.rand.NextFloat(1f), Projectile.damage * 0.5f },
				};
				Ins.VFXManager.Add(crystal);
			}
		}
		base.Smash(level);
	}
}