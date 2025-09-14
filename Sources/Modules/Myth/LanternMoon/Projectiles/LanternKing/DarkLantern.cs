using Everglow.Commons.Templates.Weapons;
using Everglow.Myth.LanternMoon.Gores;
using Terraria.DataStructures;

namespace Everglow.Myth.LanternMoon.Projectiles.LanternKing;

public class DarkLantern : TrailingProjectile
{
	public override void SetCustomDefaults()
	{
		Projectile.width = 20;
		Projectile.height = 20;
		Projectile.aiStyle = -1;
		Projectile.hostile = true;
		Projectile.friendly = false;
		Projectile.ignoreWater = true;
		Projectile.tileCollide = false;
		Projectile.extraUpdates = 3;
		Projectile.timeLeft = 3600;
		Projectile.alpha = 0;
		Projectile.penetrate = -1;
		Projectile.scale = 1f;

		TrailLength = 40;
		ProjectileID.Sets.PlayerHurtDamageIgnoresDifficultyScaling[Projectile.type] = true;
		TrailColor = new Color(1f, 0.7f, 0f, 0f);
		TrailWidth = 40f;
		SelfLuminous = true;
		TrailTexture = Commons.ModAsset.Trail_1.Value;
		TrailTextureBlack = Commons.ModAsset.Trail_black.Value;
		//TrailShader = ModAsset.TrailingDissolve.Value;
	}

	public override Color? GetAlpha(Color lightColor)
	{
		return new Color?(new Color(1f, 1f, 1f, 0.5f));
	}

	private bool shooted = false;
	private float colorValue = 0;
	private int startTimer = 0;
	private int startAtTime = 0;
	private int shootTimer = -1;
	private float scaleLimit = 0.8f;
	private float accelerationYParameter = 0;
	private float accelerationYCoefficient = 0;
	private float accelerationY = 0;

	public override void OnHitPlayer(Player target, Player.HurtInfo info)
	{
		var gore2 = new FloatLanternGore3
		{
			Active = true,
			Visible = true,
			velocity = new Vector2(Main.rand.NextFloat(0, 6), 0).RotatedByRandom(6.283),
			noGravity = false,
			position = Projectile.Center,
		};
		Ins.VFXManager.Add(gore2);
		var gore3 = new FloatLanternGore4
		{
			Active = true,
			Visible = true,
			velocity = new Vector2(Main.rand.NextFloat(0, 6), 0).RotatedByRandom(6.283),
			noGravity = false,
			position = Projectile.Center,
		};
		Ins.VFXManager.Add(gore3);
		var gore4 = new FloatLanternGore5
		{
			Active = true,
			Visible = true,
			velocity = new Vector2(Main.rand.NextFloat(0, 6), 0).RotatedByRandom(6.283),
			noGravity = false,
			position = Projectile.Center,
		};
		Ins.VFXManager.Add(gore4);
		var gore5 = new FloatLanternGore6
		{
			Active = true,
			Visible = true,
			velocity = new Vector2(Main.rand.NextFloat(0, 6), 0).RotatedByRandom(6.283),
			noGravity = false,
			position = Projectile.Center,
		};
		Ins.VFXManager.Add(gore5);
		base.OnHitPlayer(target, info);
	}

	public override void OnSpawn(IEntitySource source)
	{
		startTimer = Main.rand.Next(-60, 0);
		shootTimer = (int)Projectile.ai[0] * 4;
		scaleLimit = Main.rand.NextFloat(1.0f, 1.8f);
		accelerationYParameter = Main.rand.NextFloat(2.85f, 3.15f);
		Projectile.timeLeft = Main.rand.Next(3000, 4200);
		startAtTime = Projectile.timeLeft;
		Projectile.velocity = new Vector2(0, 0.0000006f).RotatedBy(Projectile.ai[1]);
		Projectile.frame = Main.rand.Next(4);
	}

	public override void Behaviors()
	{
		startTimer += 1;
		shootTimer -= 1;
		if (shootTimer == 0)
		{
			Projectile.velocity = new Vector2(0, 6).RotatedBy(Projectile.ai[1]);
			shooted = true;
		}
		Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) - (float)Math.PI * 0.5f;
		if (startTimer > 0 && startTimer <= 120)
		{
			colorValue = startTimer / 120f;
		}

		accelerationYCoefficient = startTimer / 400f;
		accelerationY = (accelerationYCoefficient * accelerationYCoefficient * accelerationYCoefficient - accelerationYCoefficient * accelerationYCoefficient * accelerationYParameter) / 1500f;
		if (accelerationY > 0.03f)
		{
			accelerationY = 0.03f;
		}

		if (shooted)
		{
			Projectile.velocity.Y += accelerationY;
			if (Projectile.scale < scaleLimit * 2 && Projectile.timeLeft > 800)
			{
				Projectile.scale += 0.005f;
			}
		}
		Projectile.velocity *= 0.99f;
		if (Projectile.velocity.Y > scaleLimit && accelerationY > 0)
		{
			Projectile.velocity.Y *= scaleLimit / Projectile.velocity.Y;
		}

		if (Projectile.timeLeft < 800)
		{
			Projectile.scale *= 0.96f;
		}

		if (Projectile.scale < 0.05f)
		{
			Projectile.Kill();
		}
	}

	public override void DrawSelf()
	{
		var texture2D = (Texture2D)ModContent.Request<Texture2D>(Texture);
		Projectile.frameCounter += 1;
		if (Projectile.frameCounter == 8)
		{
			Projectile.frameCounter = 0;
			Projectile.frame += 1;
		}
		if (Projectile.frame > 3)
		{
			Projectile.frame = 0;
		}

		float timeValue = (float)Main.time * 0.03f;
		var colorT = new Color(1f * colorValue * (float)(Math.Sin(timeValue) + 2) / 3f, 1f * colorValue * (float)(Math.Sin(timeValue) + 2) / 3f, 1f * colorValue * (float)(Math.Sin(timeValue) + 2) / 3f, 0.5f * colorValue * (float)(Math.Sin(timeValue) + 2) / 3f);

		Main.spriteBatch.Draw(texture2D, Projectile.Center - Main.screenPosition, null, colorT, Projectile.rotation, texture2D.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 1f);
		Main.spriteBatch.Draw(ModAsset.LanternFire.Value, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 30 * Projectile.frame, 20, 30)), colorT, 0, new Vector2(10, 15), Projectile.scale * 0.5f, SpriteEffects.None, 1f);
	}

	public override Color GetTrailColor(int style, Vector2 worldPos, int index, ref float factor, float extraValue0 = 0, float extraValue1 = 0)
	{
		if (style == 1)
		{
			float value = index / (float)SmoothedOldPos.Count;
			return Color.Lerp(TrailColor, new Color(0.5f, 0, 0.1f, 0), value);
		}
		if (style == 0)
		{
			float value = index / (float)SmoothedOldPos.Count;
			return Color.Lerp(Color.Transparent, Color.White, value);
		}
		return base.GetTrailColor(style, worldPos, index, ref factor, extraValue0, extraValue1);
	}

	public override Vector3 ModifyTrailTextureCoordinate(float factor, float timeValue, float phase, float widthValue) => base.ModifyTrailTextureCoordinate(factor, timeValue, phase, widthValue);
}