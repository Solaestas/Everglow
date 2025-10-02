using Everglow.Commons.Templates.Weapons;
using Everglow.Commons.VFX.CommonVFXDusts;

namespace Everglow.Myth.LanternMoon.Projectiles.LanternKing;

public class GoldLanternLine : TrailingProjectile
{
	public override string Texture => ModAsset.GoldLaser_Mod;

	public override void SetCustomDefaults()
	{
		TrailLength = 40;
		TrailColor = new Color(1, 0.35f, 0, 0f);
		TrailWidth = 4f;
		TrailBackgroundDarkness = 0.1f;
		SelfLuminous = true;
		TrailTexture = Commons.ModAsset.Trail.Value;
		TrailTextureBlack = Commons.ModAsset.Trail_black.Value;
		TrailShader = Commons.ModAsset.Trailing.Value;
		Projectile.timeLeft = 300;
		Projectile.hostile = true;
		Projectile.friendly = false;
	}

	public override void Behaviors()
	{
		Player player = Main.player[Player.FindClosest(Projectile.Center, 0, 0)];
		if (Projectile.timeLeft > 200)
		{
			Projectile.velocity *= 0.97f;
		}
		if (Timer == 100)
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
		Main.spriteBatch.Draw(star, Projectile.Center - Main.screenPosition, null, c0, MathHelper.PiOver2, star.Size() / 2f, new Vector2(width / 1.5f, 0.5f), SpriteEffects.None, 0);
		Main.spriteBatch.Draw(star, Projectile.Center - Main.screenPosition, null, c0, 0, star.Size() / 2f, new Vector2(width / 1.5f, 0.5f), SpriteEffects.None, 0);

		Main.spriteBatch.Draw(star, Projectile.Center - Main.screenPosition, null, c0, MathHelper.PiOver2 + (float)Main.timeForVisualEffects * 0.04f, star.Size() / 2f, width / 10f, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(star, Projectile.Center - Main.screenPosition, null, c0, (float)Main.timeForVisualEffects * 0.04f, star.Size() / 2f, width / 10f, SpriteEffects.None, 0);

		if(Timer > 50 && Timer < 100)
		{
			Player player = Main.player[Player.FindClosest(Projectile.Center, 0, 0)];
			float rot = Vector2.Normalize(player.Center - Projectile.Center).ToRotationSafe() + MathHelper.PiOver2;
			Color c1 = Color.Lerp(new Color(0.3f, 0.05f, 0f, 0f), new Color(1f, 1f, 0.5f, 0f), (Timer - 50) / 50f);
			Main.spriteBatch.Draw(star, Projectile.Center - Main.screenPosition, null, c1, rot, star.Size() / 2f, new Vector2(width / 1.5f + 0.5f, 1f), SpriteEffects.None, 0);
		}
	}

	public override Color GetTrailColor(int style, Vector2 worldPos, int index, ref float factor, float extraValue0 = 0, float extraValue1 = 0) => base.GetTrailColor(style, worldPos, index, ref factor, extraValue0, extraValue1);

	public override Vector3 ModifyTrailTextureCoordinate(float factor, float timeValue, float phase, float widthValue) => base.ModifyTrailTextureCoordinate(factor, timeValue, phase, widthValue);

	public override void DestroyEntityEffect()
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
	}
}