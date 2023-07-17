using Everglow.Commons.VFX.CommonVFXDusts;

namespace Everglow.Minortopography.GiantPinetree.Projectiles;

public class FrostBall : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 10;
		Projectile.height = 10;
		Projectile.friendly = true;
		Projectile.ignoreWater = false;
		Projectile.DamageType = DamageClass.Magic;
		Projectile.tileCollide = true;
		Projectile.timeLeft = 360;
		Projectile.penetrate = 1;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 2;
	}
	public override void AI()
	{
		Projectile.rotation += 0.3f;

		if (Projectile.Center.X > Main.screenPosition.X - 100 && Projectile.Center.X < Main.screenPosition.X + Main.screenWidth + 100 && Projectile.Center.Y > Main.screenPosition.Y - 100 && Projectile.Center.Y < Main.screenPosition.Y + Main.screenWidth + 100)
		{
			if (Main.rand.NextBool(2))
			{
				Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(0f, 0.6f)).RotatedByRandom(MathHelper.TwoPi);
				var smog = new IceSmogDust
				{
					velocity = newVelocity + Projectile.velocity * Main.rand.NextFloat(0f, 0.03f),
					Active = true,
					Visible = true,
					position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) + Projectile.velocity * Main.rand.NextFloat(-3f, 2f),
					maxTime = Main.rand.Next(137, 245),
					scale = Main.rand.NextFloat(18f, 45f),
					rotation = Main.rand.NextFloat(6.283f),
					ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), Main.rand.NextFloat(-0.005f, 0.005f) }
				};
				Ins.VFXManager.Add(smog);
			}
			else
			{
				Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(0f, 0.6f)).RotatedByRandom(MathHelper.TwoPi);
				var smog = new IceSmogDust2
				{
					velocity = newVelocity + Projectile.velocity * Main.rand.NextFloat(0f, 0.03f),
					Active = true,
					Visible = true,
					position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) + Projectile.velocity * Main.rand.NextFloat(-3f, 2f),
					maxTime = Main.rand.Next(137, 245),
					scale = Main.rand.NextFloat(18f, 45f),
					rotation = Main.rand.NextFloat(6.283f),
					ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), Main.rand.NextFloat(-0.005f, 0.005f) }
				};
				Ins.VFXManager.Add(smog);
			}
			if (Main.rand.NextBool(3))
			{
				Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(0f, 0.6f)).RotatedByRandom(MathHelper.TwoPi);
				var smog = new SnowPieceDust
				{
					velocity = newVelocity + Projectile.velocity * Main.rand.NextFloat(0f, 0.03f),
					Active = true,
					Visible = true,
					coord0 = new Vector2(Main.rand.NextFloat(0.1f, 0.2f), 0).RotatedByRandom(6.283),
					coord1 = new Vector2(Main.rand.NextFloat(0.1f, 0.2f), 0).RotatedByRandom(6.283),
					position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) + Projectile.velocity * Main.rand.NextFloat(-3f, 2f),
					maxTime = Main.rand.Next(37, 125),
					scale = Main.rand.NextFloat(2f, 8f),
					rotation = Main.rand.NextFloat(6.283f),
					rotation2 = Main.rand.NextFloat(6.283f),
					omega = Main.rand.NextFloat(-10f, 10f),
					phi = Main.rand.NextFloat(6.283f),
					ai = new float[] { Main.rand.NextFloat(0f, 1f), Main.rand.NextFloat(0f, 1f), Main.rand.NextFloat(-0.005f, 0.005f) }
				};
				Ins.VFXManager.Add(smog);
			}
		}

		if (Projectile.position.X <= 320 || Projectile.position.X >= Main.maxTilesX * 16 - 320)
		{
			Projectile.Kill();
		}
		if (Projectile.position.Y <= 320 || Projectile.position.Y >= Main.maxTilesY * 16 - 320)
		{
			Projectile.Kill();
		}
	}
	public override bool PreDraw(ref Color lightColor)
	{
		return true;
	}
	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		return true;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		target.AddBuff(BuffID.Frostburn, 180);
	}
	public override void Kill(int timeLeft)
	{
		GenerateSmog(6);
		//for (int g = 0; g < 18; g++)
		//{
		//	Vector2 iceV = new Vector2(0, Main.rand.NextFloat(0, 0.9f)).RotatedByRandom(MathHelper.TwoPi);
		//	Dust ice = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.IceCrystal>(), iceV.X, iceV.Y, 150, default, Main.rand.NextFloat(0.2f, 0.6f));
		//	ice.velocity = iceV;
		//	ice.color.G = (byte)Main.rand.Next(240);
		//}
		//for (int g = 0; g < 18; g++)
		//{
		//	Vector2 iceV = new Vector2(0, Main.rand.NextFloat(0, 0.9f)).RotatedByRandom(MathHelper.TwoPi);
		//	Dust ice = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.IceCrystal>(), iceV.X, iceV.Y, 150, default, Main.rand.NextFloat(0.2f, 0.6f));
		//	ice.velocity = iceV;
		//	ice.color.G = (byte)Main.rand.Next(240);
		//}
	}
	public void GenerateSmog(int Frequency)
	{
		float mulVelocity = 1f;
		for (int g = 0; g < Frequency / 2 + 1; g++)
		{
			Vector2 newVelocity = new Vector2(0, mulVelocity * Main.rand.NextFloat(0f, 4f)).RotatedByRandom(MathHelper.TwoPi);
			var somg = new IceSmogDust
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) + newVelocity * 16,
				maxTime = Main.rand.Next(237, 345),
				scale = Main.rand.NextFloat(320f, 435f),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 }
			};
			Ins.VFXManager.Add(somg);
		}
		for (int g = 0; g < Frequency / 2 - 1; g++)
		{
			Vector2 newVelocity = new Vector2(0, mulVelocity * Main.rand.NextFloat(0f, 4f)).RotatedByRandom(MathHelper.TwoPi);
			var somg = new IceSmogDust2
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) + newVelocity * 16,
				maxTime = Main.rand.Next(237, 345),
				scale = Main.rand.NextFloat(220f, 235f),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 }
			};
			Ins.VFXManager.Add(somg);
		}
		for (int g = 0; g < Frequency * 6; g++)
		{
			Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(2f, 4.6f)).RotatedByRandom(MathHelper.TwoPi);
			var smog = new SnowPieceDust
			{
				velocity = newVelocity + Projectile.velocity * Main.rand.NextFloat(0f, 0.03f),
				Active = true,
				Visible = true,
				coord0 = new Vector2(Main.rand.NextFloat(0.1f, 0.2f), 0).RotatedByRandom(6.283),
				coord1 = new Vector2(Main.rand.NextFloat(0.1f, 0.2f), 0).RotatedByRandom(6.283),
				position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) + Projectile.velocity * Main.rand.NextFloat(-3f, 2f),
				maxTime = Main.rand.Next(47, 85),
				scale = Main.rand.NextFloat(2f, 12f),
				rotation = Main.rand.NextFloat(6.283f),
				rotation2 = Main.rand.NextFloat(6.283f),
				omega = Main.rand.NextFloat(-10f, 10f),
				phi = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(0f, 1f), Main.rand.NextFloat(0f, 1f), Main.rand.NextFloat(-0.005f, 0.005f) }
			};
			Ins.VFXManager.Add(smog);
		}
	}
}