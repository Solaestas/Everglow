using Everglow.Myth.LanternMoon.VFX;
using Terraria.DataStructures;

namespace Everglow.Myth.LanternMoon.Projectiles.Weapons;

public class MillionLightStaff_Proj : ModProjectile
{
	public int Style;

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

	public override void OnSpawn(IEntitySource source)
	{
		Style = Main.rand.Next(2);
		base.OnSpawn(source);
	}

	public override void AI()
	{
		if (Style == 0)
		{
			Projectile.rotation = 0;
			Projectile.velocity *= 0.995f;
		}
		if (Style == 1)
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
		}
	}

	public override void OnKill(int timeLeft)
	{
		for (int g = 0; g < 12; g++)
		{
			Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(17f, 30f) * Projectile.ai[0]).RotatedByRandom(MathHelper.TwoPi);
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

		base.OnKill(timeLeft);
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
	}

	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
		Rectangle frame = new Rectangle(0, 38 * Style, 38, 38);
		Color drawColor = new Color(1f, 1f, 1f, 0);
		if(Style == 1)
		{
			drawColor = lightColor;
		}
		if (Projectile.timeLeft < 60)
		{
			drawColor *= Projectile.timeLeft / 60f;
		}
		Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, frame, drawColor, Projectile.rotation, frame.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);
		return false;
	}
}