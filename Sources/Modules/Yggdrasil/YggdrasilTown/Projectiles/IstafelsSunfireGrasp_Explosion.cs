using Everglow.Commons.VFX.CommonVFXDusts;
using Everglow.Yggdrasil.CityOfMagicFlute.VFXs;
using Everglow.Yggdrasil.YggdrasilTown.Buffs;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class IstafelsSunfireGrasp_Explosion : ModProjectile, IWarpProjectile
{
	public override string Texture => ModAsset.TerraViewerHowitzer_grenade_fall_Mod;

	public override void SetDefaults()
	{
		Projectile.width = 40;
		Projectile.height = 40;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.aiStyle = -1;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 3;
		Projectile.tileCollide = false;
		Projectile.extraUpdates = 2;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 20;
		Projectile.DamageType = DamageClass.Magic;
		ProjectileID.Sets.DrawScreenCheckFluff[Type] = 30000;
	}

	public void Spark()
	{
		for (int g = 0; g < 120; g++)
		{
			Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(2f, 22f)).RotatedByRandom(MathHelper.TwoPi);
			var spark = new FireSparkDust
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center + new Vector2(Main.rand.NextFloat(10), 0).RotatedByRandom(6.283) + newVelocity * 3,
				maxTime = Main.rand.Next(20, 40),
				scale = Main.rand.NextFloat(0.1f, Main.rand.NextFloat(1f, 25.0f)),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), Main.rand.NextFloat(-0.01f, 0.01f) },
			};
			Ins.VFXManager.Add(spark);
		}
	}

	public void FlameII()
	{
		Vector2 newVelocity = new Vector2(0, -10);
		Vector2 addPos = new Vector2(Main.rand.NextFloat(20), 0).RotatedByRandom(MathHelper.TwoPi);
		var fire = new FireDust
		{
			velocity = newVelocity,
			Active = true,
			Visible = true,
			position = Projectile.Center + addPos - newVelocity,
			maxTime = Main.rand.Next(30, 45),
			scale = Main.rand.NextFloat(20f, 60f),
			rotation = Main.rand.NextFloat(6.283f),
			ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), addPos.X * 0.002f },
		};
		Ins.VFXManager.Add(fire);
	}

	public void LargeFlame()
	{
		Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(2f, 4f)).RotatedByRandom(MathHelper.TwoPi);
		var somg = new MissleFlameDust
		{
			velocity = newVelocity,
			Active = true,
			Visible = true,
			position = Projectile.Center + new Vector2(Main.rand.NextFloat(6), 0).RotatedByRandom(6.283),
			maxTime = Main.rand.Next(60, 75),
			scale = Main.rand.NextFloat(6f, 12f),
			rotation = Main.rand.NextFloat(6.283f),
			ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 },
		};
		Ins.VFXManager.Add(somg);
	}

	public void SmallFlame()
	{
		Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(2f, 4f)).RotatedByRandom(MathHelper.TwoPi);
		var somg = new MissleFlameDust
		{
			velocity = newVelocity,
			Active = true,
			Visible = true,
			position = Projectile.Center + new Vector2(Main.rand.NextFloat(4), 0).RotatedByRandom(6.283),
			maxTime = Main.rand.Next(30, 85),
			scale = Main.rand.NextFloat(6f, 12f),
			rotation = Main.rand.NextFloat(6.283f),
			ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 },
		};
		Ins.VFXManager.Add(somg);
	}

	public override void OnSpawn(IEntitySource source)
	{
		Projectile.ai[2] = 600;
		Projectile.velocity *= 0;

		for (int x = 0; x < 150; x++)
		{
			LargeFlame();
		}
		for (int x = 0; x < 600; x++)
		{
			SmallFlame();
		}

		Projectile.hide = true;
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		target.AddBuff(ModContent.BuffType<Charred>(), IstafelsSunfireGrasp_FireBall.BuffDuration);
	}

	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
	{
		bool bool0 = (targetHitbox.TopLeft() - projHitbox.Center()).Length() < 420;
		bool bool1 = (targetHitbox.TopRight() - projHitbox.Center()).Length() < 420;
		bool bool2 = (targetHitbox.BottomLeft() - projHitbox.Center()).Length() < 420;
		bool bool3 = (targetHitbox.BottomRight() - projHitbox.Center()).Length() < 420;
		return bool0 || bool1 || bool2 || bool3;
	}

	public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
	{
		behindNPCsAndTiles.Add(index);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		float timeValue = (200 - Projectile.timeLeft) / 200f;
		float dark = Math.Max((Projectile.timeLeft - 150) / 50f, 0);
		var c = new Color(0.3f * (1 - timeValue) * (1 - timeValue), 0.6f * (1 - timeValue) * (1 - timeValue), 3f * (1 - timeValue), 0f);
		Texture2D light = Commons.ModAsset.StarSlash.Value;
		for (int i = 0; i < 9; i++)
		{
			Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, light.Width / 2, light.Height), c, MathF.Sin(Projectile.whoAmI - Projectile.position.X) * 6 + Projectile.ai[1] + i * 0.6f, light.Size() / 2f, new Vector2(dark * dark, timeValue * 8) * Projectile.ai[0] * 1.62f, SpriteEffects.None, 0);
			Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, light.Width / 2, light.Height), c, MathF.Sin(Projectile.whoAmI) * 6 + Projectile.ai[1] + i * 0.6f, light.Size() / 2f, new Vector2(MathF.Sqrt(dark), timeValue * 6) * Projectile.ai[0] * 1.62f, SpriteEffects.None, 0);
			Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, null, c, MathF.Sin(Projectile.whoAmI + Projectile.position.Y) * 6 + Projectile.ai[1] + i * 0.6f, light.Size() / 2f, new Vector2(dark * dark, timeValue * 8) * Projectile.ai[0] * 1.62f, SpriteEffects.None, 0);
			Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, light.Width / 2, light.Height), c, MathF.Sin(Projectile.type * 0.4f + Projectile.whoAmI) * 6 + Projectile.ai[1] + i * 0.6f, light.Size() / 2f, new Vector2(dark * dark, timeValue * 8) * Projectile.ai[0] * 1.62f, SpriteEffects.None, 0);
		}
		return false;
	}

	private static void DrawTexCircle_VFXBatch(VFXBatch spriteBatch, float radius, float width, Color color, Vector2 center, Texture2D tex, double addRot = 0)
	{
		var circle = new List<Vertex2D>();

		Color c0 = color;
		c0.R = 0;
		for (int h = 0; h < radius / 2; h += 1)
		{
			c0.R = (byte)(h / radius * 2 * 255);
			circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radius - width, 0)).RotatedBy(h / radius * Math.PI * 4 + addRot), c0, new Vector3(h * 2 / radius, 1, 0)));
			circle.Add(new Vertex2D(center + new Vector2(0, radius).RotatedBy(h / radius * Math.PI * 4 + addRot), c0, new Vector3(h * 2 / radius, 0.5f, 0)));
		}
		circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radius - width, 0)).RotatedBy(addRot), c0, new Vector3(1, 1, 0)));
		circle.Add(new Vertex2D(center + new Vector2(0, radius).RotatedBy(addRot), c0, new Vector3(1, 0.5f, 0)));
		circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radius - width, 0)).RotatedBy(addRot), c0, new Vector3(0, 1, 0)));
		circle.Add(new Vertex2D(center + new Vector2(0, radius).RotatedBy(addRot), c0, new Vector3(0, 0.5f, 0)));
		if (circle.Count > 2)
		{
			spriteBatch.Draw(tex, circle, PrimitiveType.TriangleStrip);
		}
	}

	public void DrawWarp(VFXBatch spriteBatch)
	{
		float value = (200 - Projectile.timeLeft) / 200f;
		float colorV = 0.9f * (1 - value);
		if (Projectile.ai[0] >= 10)
		{
			colorV *= Projectile.ai[0] / 10f;
		}

		Texture2D t = Commons.ModAsset.Trail.Value;
		DrawTexCircle_VFXBatch(spriteBatch, MathF.Sqrt(value) * 150f * Projectile.ai[0], 15 * (1 - value) * Projectile.ai[0], new Color(colorV, colorV * 0.6f, colorV, 0f), Projectile.Center - Main.screenPosition, t, Math.PI * 0.5);
	}
}