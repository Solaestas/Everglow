using Everglow.Myth.TheFirefly.VFXs;
using Terraria.DataStructures;

namespace Everglow.Myth.TheFirefly.Projectiles;

public class FallenDropFruit : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.friendly = true;
		Projectile.hostile = true;
		Projectile.timeLeft = 6000;
		Projectile.tileCollide = true;
		Projectile.aiStyle = -1;
		Projectile.width = 50;
		Projectile.height = 70;
	}
	public override void OnSpawn(IEntitySource source)
	{
	

		base.OnSpawn(source);
	}
	public override void AI()
	{
		Projectile.rotation = 0;
		Projectile.velocity.Y += 0.15f;

	}
	public void GenerateFire(int Frequency)
	{
		float mulVelocity = Projectile.ai[0] / 5f;
		for (int g = 0; g < Frequency; g++)
		{
			float sqrtRand = MathF.Pow(Main.rand.NextFloat(1), 0.4f);
			Vector2 newVelocity = new Vector2(0, mulVelocity * sqrtRand * 0.6f).RotatedByRandom(MathHelper.TwoPi);
			var fire = new MothBlueFireDust
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) + newVelocity,
				maxTime = Main.rand.Next(19, 35),
				scale = Main.rand.NextFloat(0.2f, 0.5f),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0, 0 }
			};
			Ins.VFXManager.Add(fire);
		}
	}
	public void GenerateSpark(int Frequency)
	{
		float mulVelocity = Projectile.ai[0] / 5f;
		for (int g = 0; g < Frequency; g++)
		{
			float sqrtRand = MathF.Pow(Main.rand.NextFloat(1), 0.4f);
			Vector2 newVelocity = new Vector2(0, mulVelocity * sqrtRand * 0.9f).RotatedByRandom(MathHelper.TwoPi);
			var smog = new MothShimmerScaleDust
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283),
				coord = new Vector2(Main.rand.NextFloat(1f), Main.rand.NextFloat(1f)),
				maxTime = Main.rand.Next(10, 95),
				scale = Main.rand.NextFloat(0.4f, 2.4f),
				rotation = Main.rand.NextFloat(6.283f),
				rotation2 = Main.rand.NextFloat(6.283f),
				omega = Main.rand.NextFloat(-30f, 30f),
				phi = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(-0.005f, 0.005f), 0, 0 }
			};
			Ins.VFXManager.Add(smog);
		}
	}
	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D glow = ModContent.Request<Texture2D>(Texture).Value;
		Main.EntitySpriteDraw(glow, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, glow.Size() / 2f, Projectile.scale, SpriteEffects.None);
		Main.EntitySpriteDraw(glow, Projectile.Center - Main.screenPosition, null, new Color(1f, 1f, 1f, 0), Projectile.rotation, glow.Size() / 2f, Projectile.scale, SpriteEffects.None);
		return false;
	}
}