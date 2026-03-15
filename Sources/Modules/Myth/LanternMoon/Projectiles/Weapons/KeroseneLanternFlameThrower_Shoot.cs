using Everglow.Myth.LanternMoon.VFX;

namespace Everglow.Myth.LanternMoon.Projectiles.Weapons;

public class KeroseneLanternFlameThrower_Shoot : ModProjectile
{
	public int Timer = 0;

	public override void SetDefaults()
	{
		Projectile.width = 40;
		Projectile.height = 40;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.ignoreWater = false;
		Projectile.tileCollide = true;
		Projectile.timeLeft = 20;
		Projectile.penetrate = 3;
		Projectile.aiStyle = -1;
	}

	public override void AI()
	{
		Timer++;
		if (Projectile.wet)
		{
			Projectile.Kill();
		}
		float dustCount = 1;
		if(Timer > 10)
		{
			if (!Main.rand.NextBool(3))
			{
				dustCount += 1;
			}
			else
			{
				dustCount /= 6f;
			}
		}
		dustCount *= Projectile.velocity.Length() / 12f;
		float scaleValue = 1f;
		if (Projectile.velocity.Length() > 20)
		{
			scaleValue += (Projectile.velocity.Length() - 20) / 20f;
		}
		for (int h = 0; h < dustCount; h++)
		{
			float sqrtSpeed = MathF.Sqrt(Main.rand.NextFloat(1f));
			Vector2 newVelocity = new Vector2(0, sqrtSpeed * 0.3f).RotatedByRandom(MathHelper.TwoPi) + Projectile.velocity;
			float maxTime = Main.rand.Next(30, 40);
			float startTimer = 0;
			if (Projectile.timeLeft < 20)
			{
				startTimer = (20 - Projectile.timeLeft) * 0.7f;
			}
			var somg = new KeroseneLanternFlameThrower_Flame
			{
				Velocity = newVelocity,
				Active = true,
				Visible = true,
				Position = Projectile.Center + new Vector2(Main.rand.NextFloat(20), 0).RotatedByRandom(MathHelper.TwoPi) - Projectile.velocity,
				MaxTime = maxTime,
				Timer = startTimer,
				Scale = Main.rand.NextFloat(50f, 78f) * scaleValue,
				Rotation = Main.rand.NextFloat(MathHelper.TwoPi),
				RotateSpeed = Main.rand.NextFloat(-0.8f, 0.8f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), Main.rand.NextFloat(0.01f, 0.2f), Main.rand.NextFloat(MathHelper.TwoPi) },
			};
			Ins.VFXManager.Add(somg);
		}
	}

	public override void OnKill(int timeLeft) => base.OnKill(timeLeft);

	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) => base.ModifyHitNPC(target, ref modifiers);

	public override bool PreDraw(ref Color lightColor)
	{
		return false;
	}
}