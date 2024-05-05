using Everglow.Commons.VFX.CommonVFXDusts;
using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class CyanVineStaff_proj_Explosion : ModProjectile, IWarpProjectile
{
	public override string Texture => "Everglow/Yggdrasil/YggdrasilTown/Projectiles/CyanVineStaff_proj";
	public override void SetDefaults()
	{
		Projectile.width = 120;
		Projectile.height = 120;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.aiStyle = -1;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 200;
		Projectile.tileCollide = false;
		Projectile.extraUpdates = 6;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 20;
		Projectile.DamageType = DamageClass.Magic;
	}
	public void GenerateCyanSpark()
	{
		Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(1.0f, 14f)).RotatedByRandom(MathHelper.TwoPi);
		var spark = new Spark_MoonBladeDust
		{
			velocity = newVelocity,
			Active = true,
			Visible = true,
			position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283),
			maxTime = Main.rand.Next(7, 45),
			scale = Main.rand.NextFloat(0.1f, Main.rand.NextFloat(4f, 27.0f)),
			rotation = Main.rand.NextFloat(6.283f),
			noGravity = true,
			ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), Main.rand.NextFloat(-0.03f, 0.03f) }
		};
		Ins.VFXManager.Add(spark);
	}
	public void GenerateOrangeSpark()
	{
		Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(1.0f, 14f)).RotatedByRandom(MathHelper.TwoPi);
		var spark = new FireSparkDust
		{
			velocity = newVelocity,
			Active = true,
			Visible = true,
			position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283),
			maxTime = Main.rand.Next(7, 45),
			scale = Main.rand.NextFloat(0.1f, Main.rand.NextFloat(4f, 17.0f)),
			rotation = Main.rand.NextFloat(6.283f),
			ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), Main.rand.NextFloat(-0.03f, 0.03f) }
		};
		Ins.VFXManager.Add(spark);
	}
	public override void OnSpawn(IEntitySource source)
	{
		for(int x = 0;x < 12;x++)
		{
			GenerateOrangeSpark();
			GenerateCyanSpark();
		}
	}
	public override void AI()
	{
		Projectile.velocity *= 0;
		if (Projectile.timeLeft <= 199)
			Projectile.friendly = false;
	}
	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
	{
		bool bool0 = (targetHitbox.TopLeft() - projHitbox.Center()).Length() < 9 * Projectile.ai[0];
		bool bool1 = (targetHitbox.TopRight() - projHitbox.Center()).Length() < 9 * Projectile.ai[0];
		bool bool2 = (targetHitbox.BottomLeft() - projHitbox.Center()).Length() < 9 * Projectile.ai[0];
		bool bool3 = (targetHitbox.BottomRight() - projHitbox.Center()).Length() < 9 * Projectile.ai[0];
		return bool0 || bool1 || bool2 || bool3;
	}
	public override bool PreDraw(ref Color lightColor)
	{
		float timeValue = (200 - Projectile.timeLeft) / 200f;
		float dark = Math.Max((Projectile.timeLeft - 150) / 50f, 0);
		Color c = new Color(0.2f * MathF.Sqrt(1 - timeValue), 0.6f * (1 - timeValue) * (1 - timeValue), 3f * (1 - timeValue), 0f);
		Texture2D light = Commons.ModAsset.Star.Value;
		Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, light.Width / 2, light.Height), c, MathF.Sin(Projectile.whoAmI - Projectile.position.X) * 6 + Projectile.ai[1], light.Size() / 2f, new Vector2(1f, dark * dark) * Projectile.ai[0] * 0.62f, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, light.Width / 2, light.Height), c, MathF.Sin(Projectile.whoAmI) * 6 + Projectile.ai[1], light.Size() / 2f, new Vector2(0.5f, dark) * Projectile.ai[0] * 0.62f, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, null, c, MathF.Sin(Projectile.whoAmI + Projectile.position.Y) * 6 + Projectile.ai[1], light.Size() / 2f, new Vector2(1f, dark * dark) * Projectile.ai[0] * 0.62f, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, light.Width / 2, light.Height), c, MathF.Sin(Projectile.type * 0.4f + Projectile.whoAmI) * 6 + Projectile.ai[1], light.Size() / 2f, new Vector2(1f, dark * dark) * Projectile.ai[0] * 0.62f, SpriteEffects.None, 0);
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
			spriteBatch.Draw(tex, circle, PrimitiveType.TriangleStrip);
	}
	public void DrawWarp(VFXBatch spriteBatch)
	{
		float value = (200 - Projectile.timeLeft) / 200f;
		float colorV = 0.9f * (1 - value);
		if (Projectile.ai[0] >= 10)
			colorV *= Projectile.ai[0] / 10f;
		Texture2D t = Commons.ModAsset.Trail.Value;
		DrawTexCircle_VFXBatch(spriteBatch, MathF.Sqrt(value) * 48f * Projectile.ai[0], 12 * (1 - value) * Projectile.ai[0], new Color(colorV, colorV * 0.6f, colorV, 0f), Projectile.Center - Main.screenPosition, t, Math.PI * 0.5);
	}
}