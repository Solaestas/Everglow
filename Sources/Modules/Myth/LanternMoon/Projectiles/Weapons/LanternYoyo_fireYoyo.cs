using Everglow.Myth.LanternMoon.VFX;
using Spine;

namespace Everglow.Myth.LanternMoon.Projectiles.Weapons;

public class LanternYoyo_fireYoyo : ModProjectile
{
	public Projectile MainProjYoyo = null;

	public bool FoundTarget = false;

	public override void SetDefaults()
	{
		Projectile.aiStyle = -1;
		Projectile.width = 16;
		Projectile.height = 16;
		Projectile.tileCollide = true;
		Projectile.ignoreWater = false;
		Projectile.penetrate = 1;
		Projectile.timeLeft = 36000000;
		Projectile.hostile = false;
		Projectile.friendly = true;
	}

	public override void AI()
	{
		if(Projectile.wet && !Projectile.lavaWet)
		{
			Projectile.Kill();
		}
		if (MainProjYoyo is null || !MainProjYoyo.active || MainProjYoyo.type != ModContent.ProjectileType<LanternYoyoProjectile>())
		{
			if (Projectile.timeLeft > 60)
			{
				Projectile.timeLeft = 60;
			}
			return;
		}
		var target = Projectile.FindTargetWithinRange(100);
		if (target is not null)
		{
			FoundTarget = true;
			Vector2 toNPCTarget = target.Center - Projectile.Center;
			toNPCTarget = toNPCTarget.NormalizeSafe() * 13f;
			Projectile.velocity = Projectile.velocity * 0.92f + toNPCTarget * 0.08f;
		}
		else
		{
			if(FoundTarget)
			{
				if (Projectile.timeLeft > 60)
				{
					Projectile.timeLeft = 60;
				}
			}
		}

		if(!FoundTarget)
		{
			int index = AllocateIndex();
			if (index >= 5)
			{
				if (Projectile.timeLeft > 60)
				{
					Projectile.timeLeft = 60;
				}
				return;
			}
			Vector2 targetPos = MainProjYoyo.Center + new Vector2(0, 1).RotatedBy((float)Main.time * 0.06f + index * MathHelper.TwoPi / 5f) * 90;
			Vector2 toTarget = targetPos - Projectile.Center;
			if (toTarget.Length() > 130)
			{
				Projectile.velocity = toTarget / 20f;
			}
			else if (toTarget.Length() > 13)
			{
				Projectile.velocity = toTarget.SafeNormalize(Vector2.Zero) * 6.5f;
			}
			else
			{
				Projectile.Center = targetPos;
			}
		}
		Projectile.rotation += 0.05f;
	}

	public int AllocateIndex()
	{
		int index = 0;
		foreach (var proj in Main.projectile)
		{
			if (proj is not null && proj.active && proj.type == Type && proj.whoAmI < Projectile.whoAmI)
			{
				LanternYoyo_fireYoyo lYfY = proj.ModProjectile as LanternYoyo_fireYoyo;
				if (lYfY is not null && lYfY.MainProjYoyo == MainProjYoyo)
				{
					index++;
				}
			}
		}
		return index;
	}

	public override void OnKill(int timeLeft)
	{
		if(timeLeft > 0)
		{
			for (int g = 0; g < 6; g++)
			{
				Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(12f, 20f)).RotatedByRandom(MathHelper.TwoPi);
				var spark = new HitEffectSpark
				{
					Velocity = newVelocity,
					Active = true,
					Visible = true,
					Position = Projectile.Center,
					MaxTime = Main.rand.Next(16, 20),
					DrawColor = new Color(1f, 0.4f, 0, 0),
					LightFlat = 1f,
					SpeedDecay = 0.8f,
					GravityAcc = 0.15f,
					SelfLight = true,
					Scale = Main.rand.NextFloat(16f, 28f),
				};
				Ins.VFXManager.Add(spark);
			}
			for (int g = 0; g < 6; g++)
			{
				float sqrtSpeed = MathF.Sqrt(Main.rand.NextFloat(1f));
				Vector2 newVelocity = new Vector2(0, sqrtSpeed * 2).RotatedByRandom(MathHelper.TwoPi);
				var somg = new LanternFlameDust
				{
					Velocity = newVelocity,
					Active = true,
					Visible = true,
					Position = Projectile.Center + new Vector2(Main.rand.NextFloat(20), 0).RotatedByRandom(MathHelper.TwoPi),
					MaxTime = Main.rand.Next(30, 45),
					Scale = Main.rand.NextFloat(32f, 48f),
					Rotation = Main.rand.NextFloat(MathHelper.TwoPi),
					RotateSpeed = Main.rand.NextFloat(-0.8f, 0.8f),
					ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 },
				};
				Ins.VFXManager.Add(somg);
			}
		}
		base.OnKill(timeLeft);
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		target.AddBuff(BuffID.OnFire, 150);
		base.OnHitNPC(target, hit, damageDone);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
		Color drawColor = Color.Lerp(new Color(1f, 0.48f, 0.1f, 0), new Color(1f, 1f, 0.7f, 0), MathF.Sin((float)Main.time * 0.08f + Projectile.whoAmI) * 0.5f + 0.5f);
		if(Projectile.timeLeft < 60)
		{
			drawColor *= Projectile.timeLeft / 60f;
		}
		Lighting.AddLight(Projectile.Center, new Vector3(drawColor.R, drawColor.G * 0.7f, drawColor.B * 0.5f) / 300f);
		Texture2D spot = Commons.ModAsset.LightPoint2.Value;
		Main.EntitySpriteDraw(spot, Projectile.Center - Main.screenPosition, null, drawColor, MathHelper.PiOver2, spot.Size() * 0.5f, 2f, SpriteEffects.None, 0f);
		Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, drawColor, Projectile.rotation, tex.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);
		return false;
	}
}