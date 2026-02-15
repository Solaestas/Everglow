using Everglow.Myth.LanternMoon.VFX;
using Spine;

namespace Everglow.Myth.LanternMoon.Projectiles.Weapons;

public class LanternYoyo_fireYoyo : ModProjectile
{
	public Projectile MainProjYoyo = null;

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
		if (MainProjYoyo is null || !MainProjYoyo.active || MainProjYoyo.type != ModContent.ProjectileType<LanternYoyoProjectile>())
		{
			if (Projectile.timeLeft > 60)
			{
				Projectile.timeLeft = 60;
			}
			return;
		}
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
		if (toTarget.Length() > 13)
		{
			Projectile.velocity = toTarget.SafeNormalize(Vector2.Zero) * 6.5f;
		}
		else
		{
			Projectile.Center = targetPos;
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
				index++;
			}
		}
		return index;
	}

	public override void OnKill(int timeLeft)
	{
		if(timeLeft > 0)
		{
			for (int g = 0; g < 12; g++)
			{
				Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(7f, 10f)).RotatedByRandom(MathHelper.TwoPi);
				var spark = new LanternExplosionSpark
				{
					Velocity = newVelocity,
					Active = true,
					Visible = true,
					Position = Projectile.Center + new Vector2(Main.rand.NextFloat(20), 0).RotatedByRandom(MathHelper.TwoPi) + newVelocity * 3,
					RotateSpeed = Main.rand.NextFloat(-0.7f, 0.7f),
					VelocityRotateSpeed = Main.rand.NextFloat(0.05f, 0.25f) * (g % 2 - 0.5f) * 2,
					Rotation = Main.rand.NextFloat(MathHelper.TwoPi),
					MaxTime = Main.rand.Next(30, 60),
					Scale = Main.rand.NextFloat(2f, 3f),
					Frame = Main.rand.Next(0, 4),
				};
				Ins.VFXManager.Add(spark);
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