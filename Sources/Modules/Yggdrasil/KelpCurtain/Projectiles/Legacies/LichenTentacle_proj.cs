using Everglow.Commons.Weapons.Whips;
using Everglow.Yggdrasil.KelpCurtain.Buffs;
using Everglow.Yggdrasil.KelpCurtain.VFXs;

namespace Everglow.Yggdrasil.KelpCurtain.Projectiles.Legacies;

public class LichenTentacle_proj : WhipProjectile
{
	public override void SetDef()
	{
		DustType = ModContent.DustType<Dusts.LichenSlime>();
		WhipLength = 250;
		VerticalFrameCount = 5;
	}

	public override void AI()
	{
		base.AI();
	}

	public override void GenerateDusts()
	{
		Player player = Main.player[Projectile.owner];
		float t = (Projectile.ai[0] + 5) / TimeToFlyOut;
		if (Utils.GetLerpValue(0.1f, 0.7f, t, true) * Utils.GetLerpValue(0.9f, 0.7f, t, true) > 0.5f)
		{
			float times = player.meleeSpeed / 2f;
			if (times < 0)
			{
				times = 0;
			}
			List<Vector2> nextWhip15 = new List<Vector2>();
			FillWhipControlPoints(nextWhip15, -5);
			if (nextWhip15.Count > 10)
			{
				for (int x = 0; x < times; x++)
				{
					int randSegment = Main.rand.Next(nextWhip15.Count - 10, nextWhip15.Count);

					Vector2 spinningpoint = nextWhip15[randSegment] - nextWhip15[randSegment - 1];
					Vector2 afterVelocity = spinningpoint.RotatedBy(MathHelper.PiOver2 * player.direction * 0.4f) * 0.9f;
					var splash = new LichenSlimeSplash
					{
						velocity = afterVelocity,
						Active = true,
						Visible = true,
						position = nextWhip15[randSegment],
						maxTime = Main.rand.Next(12, 48),
						scale = Main.rand.NextFloat(6f, 18f),
						ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), Main.rand.NextFloat(0.04f, 0.05f) * player.direction, Main.rand.NextFloat(20.0f, 40.0f) },
					};
					Ins.VFXManager.Add(splash);
				}
				for (int x = 0; x < times * 2; x++)
				{
					int randSegment = Main.rand.Next(nextWhip15.Count - 10, nextWhip15.Count);

					Vector2 spinningpoint = nextWhip15[randSegment] - nextWhip15[randSegment - 1];
					Vector2 afterVelocity = spinningpoint.RotatedBy(MathHelper.PiOver2 * player.direction) * 5.9f;
					float mulScale = Main.rand.NextFloat(6f, 15f);
					var blood = new LichenSlimeDrop
					{
						velocity = afterVelocity / mulScale,
						Active = true,
						Visible = true,
						position = nextWhip15[randSegment],
						maxTime = Main.rand.Next(82, 164),
						scale = mulScale,
						rotation = Main.rand.NextFloat(6.283f),
						ai = new float[] { 0f, Main.rand.NextFloat(0.0f, 4.93f) },
					};
					Ins.VFXManager.Add(blood);
				}
			}
		}
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		target.AddBuff(ModContent.BuffType<LichenInfected>(), 45);
	}
}