using Everglow.Commons.Templates.Weapons;
using Everglow.Commons.VFX.CommonVFXDusts;

namespace Everglow.Myth.LanternMoon.Projectiles.LanternKing;

public class GoldLanternLine : TrailingProjectile
{
	public override string Texture => "Everglow/" + ModAsset.GoldLaser_Path;

	public override void SetDef()
	{
		TrailLength = 40;
		TrailColor = new Color(1, 0.65f, 0, 0f);
		TrailWidth = 2f;
		SelfLuminous = true;
		TrailTexture = Commons.ModAsset.Trail.Value;
		TrailTextureBlack = Commons.ModAsset.Trail_black.Value;
		TrailShader = Commons.ModAsset.Trailing.Value;
		Projectile.timeLeft = 300;
		Projectile.hostile = true;
		Projectile.friendly = false;
	}

	public override void AI()
	{
		Player player = Main.player[Player.FindClosest(Projectile.Center, 0, 0)];
		if (Projectile.timeLeft > 200)
		{
			Projectile.velocity *= 0.97f;
		}
		if (Projectile.timeLeft == 200)
		{
			TrailWidth = 10f;
			Projectile.velocity = Vector2.Normalize(player.Center - Projectile.Center) * 15;
			for (int x = 0; x < 15; x++)
			{
				var spark = new RayDustDust
				{
					velocity = new Vector2(0, Main.rand.NextFloat(2, 3f)).RotateRandom(MathHelper.TwoPi),
					Active = true,
					Visible = true,
					position = Projectile.Center,
					maxTime = Main.rand.Next(67, 75),
					scale = Main.rand.NextFloat(0.1f, Main.rand.NextFloat(9f, 12.0f)),
					rotation = Main.rand.NextFloat(6.283f),
					ai = new float[] { 0 },
				};
				Ins.VFXManager.Add(spark);
			}
		}
		Lighting.AddLight(Projectile.Center, new Vector3(1f, 1f, 0) * TrailWidth / 7f);
		base.AI();
	}

	public override bool PreDraw(ref Color lightColor)
	{
		return base.PreDraw(ref lightColor);
	}

	public override void DrawSelf()
	{
		Texture2D star = Commons.ModAsset.StarSlash.Value;
		float width = 0.5f;
		if (Projectile.timeLeft < 200)
		{
			width = 2f;
		}
		if (Projectile.timeLeft < 150)
		{
			width *= Projectile.timeLeft / 150f;
		}
		width *= 1 + (MathF.Sin((float)Main.time * 0.23f + Projectile.whoAmI) + 0.5f) * 0.7f;
		float timeValue = MathF.Sin((float)Main.time * 0.07f + Projectile.whoAmI) * 0.5f + 0.5f;
		Color c0 = new Color(1f, 0.75f * timeValue, 0, 0) * timeValue;
		Main.spriteBatch.Draw(star, Projectile.Center - Main.screenPosition, null, c0, MathHelper.PiOver2, star.Size() / 2f, width / 4.5f, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(star, Projectile.Center - Main.screenPosition, null, c0, 0, star.Size() / 2f, new Vector2(3f, width / 4.5f), SpriteEffects.None, 0);
		Main.spriteBatch.Draw(star, Projectile.Center - Main.screenPosition, null, c0, MathHelper.PiOver2 + (float)Main.timeForVisualEffects * 0.04f, star.Size() / 2f, width / 10f, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(star, Projectile.Center - Main.screenPosition, null, c0, (float)Main.timeForVisualEffects * 0.04f, star.Size() / 2f, width / 10f, SpriteEffects.None, 0);
	}

	public override void DrawTrail()
	{
		base.DrawTrail();
	}

	public override void DrawTrailDark()
	{
		base.DrawTrailDark();
	}

	public override void KillMainStructure()
	{
		for (int x = 0; x < 25; x++)
		{
			var spark = new RayDustDust
			{
				velocity = new Vector2(0, Main.rand.NextFloat(2, 6f)).RotateRandom(MathHelper.TwoPi),
				Active = true,
				Visible = true,
				position = Projectile.Center,
				maxTime = Main.rand.Next(57, 255),
				scale = Main.rand.NextFloat(0.1f, Main.rand.NextFloat(8f, 17.0f)),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { 0 },
			};
			Ins.VFXManager.Add(spark);
		}
		base.KillMainStructure();
	}
}