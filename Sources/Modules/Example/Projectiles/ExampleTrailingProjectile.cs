using Everglow.Commons.Templates.Weapons;
using Everglow.Commons.Utilities;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX;

namespace Everglow.Example.Projectiles;

public class ExampleTrailingProjectile : TrailingProjectile
{
	public override void SetDef()
	{
		TrailTexture = Commons.ModAsset.Trail_8.Value;
		TrailTextureBlack = Commons.ModAsset.Trail_8_black.Value;
		ProjectileID.Sets.TrailCacheLength[Type] = 30;
	}

	public override void Behaviors()
	{
		Vector2 offsetTarget = new Vector2(0, 150).RotatedBy(Main.time * 0.24f);
		offsetTarget.Y *= 0.4f;
		offsetTarget = offsetTarget.RotatedBy(-Main.time * 0.082f);
		Vector2 targetPos = Main.MouseWorld + offsetTarget;
		Vector2 toTarget = targetPos - Projectile.Center - Projectile.velocity;
		toTarget = toTarget.NormalizeSafe() * 45f;
		Projectile.velocity = Projectile.velocity * 0.85f + toTarget * 0.15f;
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		KillMainStructure();
	}

	public override void OnHitPlayer(Player target, Player.HurtInfo info)
	{
		KillMainStructure();
	}

	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		KillMainStructure();
		Projectile.tileCollide = false;
		return false;
	}

	public override void KillMainStructure()
	{
		Projectile.velocity = Projectile.oldVelocity;
		Projectile.friendly = false;
		if (TimeTokill < 0)
		{
			Explosion();
		}
		TimeTokill = ProjectileID.Sets.TrailCacheLength[Projectile.type];
	}

	public override void Explosion()
	{
	}

	/// <summary>
	/// Default to call this before DrawTrail.
	/// </summary>
	public override void DrawTrailDark()
	{
		base.DrawTrailDark();
	}

	public override void DrawTrail()
	{
		base.DrawTrail();
	}

	public override bool PreDraw(ref Color lightColor)
	{
		return base.PreDraw(ref lightColor);
	}

	public override void DrawSelf()
	{
		base.DrawSelf();
	}

	public new void DrawWarp(VFXBatch spriteBatch)
	{
		if (SmoothedOldPos.Count <= 0)
		{
			return;
		}
		Vector2 halfSize = new Vector2(Projectile.width, Projectile.height) / 2f;
		float width = TrailWidth;
		var bars = new List<Vertex2D>();
		var bars2 = new List<Vertex2D>();
		var bars3 = new List<Vertex2D>();
		for (int i = 1; i < SmoothedOldPos.Count; ++i)
		{
			if (SmoothedOldPos[i] == Vector2.Zero)
			{
				break;
			}
			var normalDir = SmoothedOldPos[i - 1] - SmoothedOldPos[i];
			float mulFac = Timer / (float)ProjectileID.Sets.TrailCacheLength[Projectile.type];
			if (mulFac > 1f)
			{
				mulFac = 1f;
			}
			float factor = i / (float)SmoothedOldPos.Count * mulFac;
			float widthZ = TrailWidthFunction(factor);
			var c0 = new Color(1 - (normalDir.X + 25f) / 50f, 1 - (normalDir.Y + 25f) / 50f, 0.1f * WarpStrength, 1);
			float x0 = factor * 1.3f + (float)(Main.time * 0.03f);
			Vector2 drawPos = SmoothedOldPos[i] - Main.screenPosition + halfSize;
			bars.Add(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 0f / 3f) * width, c0, new Vector3(x0, 1, widthZ));
			bars.Add(drawPos, c0, new Vector3(x0, 0.5f, widthZ));
			bars2.Add(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 1f / 3f) * width, c0, new Vector3(x0, 1, widthZ));
			bars2.Add(drawPos, c0, new Vector3(x0, 0.5f, widthZ));
			bars3.Add(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 2f / 3f) * width, c0, new Vector3(x0, 1, widthZ));
			bars3.Add(drawPos, c0, new Vector3(x0, 0.5f, widthZ));
		}
		if (bars.Count >= 2 && bars.Count >= 2 && bars.Count >= 3)
		{
			spriteBatch.Draw(TrailTexture, bars, PrimitiveType.TriangleStrip);
			spriteBatch.Draw(TrailTexture, bars2, PrimitiveType.TriangleStrip);
			spriteBatch.Draw(TrailTexture, bars3, PrimitiveType.TriangleStrip);
		}
	}
}