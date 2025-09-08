namespace Everglow.Yggdrasil.YggdrasilTown.VFXs;

[Pipeline(typeof(WCSPipeline))]
public class BloodSwampDust : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;

	public Projectile ChasedProjectile;
	public Vector2 position;
	public Vector2 velocity;
	public float[] ai;
	public float timer;
	public float maxTime;
	public float scale;
	public float MaxScale;
	public float rotation;

	public override void Update()
	{
		position += velocity;
		if (position.X <= 320 || position.X >= Main.maxTilesX * 16 - 320)
		{
			timer = maxTime;
			Active = false;
			return;
		}
		if (position.Y <= 320 || position.Y >= Main.maxTilesY * 16 - 320)
		{
			timer = maxTime;
			Active = false;
			return;
		}
		var toProj = Vector2.zeroVector;
		if (ChasedProjectile != null && ChasedProjectile.active)
		{
			toProj = (ChasedProjectile.Center - position).NormalizeSafe() * 3f;
		}
		else
		{
			timer += 5;
		}
		Vector2 rotatedVel = velocity.NormalizeSafe().RotatedBy(ai[0]) * 0.3f;
		Vector2 accleration = Vector2.Lerp(rotatedVel, toProj, MathF.Sin(ai[1] + (float)Main.time * 0.12f) * 0.5f + 0.5f);
		velocity *= 0.7f;
		velocity += accleration;
		scale = MaxScale * (1 - MathF.Sin(timer / maxTime * MathF.PI * 0.5f));
		timer++;
		if (timer > maxTime)
		{
			Active = false;
			return;
		}
		Lighting.AddLight(position, scale * 0.02f, 0, 0);
	}

	public override void Draw()
	{
		Vector2 toCorner = new Vector2(0, scale);
		Color lightColor = new Color(0.5f, 0, 0, 0.5f);

		List<Vertex2D> bars = new List<Vertex2D>()
		{
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1 + rotation), lightColor, new Vector3(1, 0, 0)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 0.5 + rotation), lightColor, new Vector3(0, 0, 0)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 0 + rotation), lightColor, new Vector3(0, 1, 0)),

			new Vertex2D(position + toCorner.RotatedBy(Math.PI * -0.5 + rotation), lightColor, new Vector3(1, 1, 0)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 0 + rotation), lightColor, new Vector3(0, 1, 0)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1 + rotation), lightColor, new Vector3(1, 0, 0)),
		};
		Ins.Batch.Draw(ModAsset.BloodFlame_noise.Value, bars, PrimitiveType.TriangleList);
	}
}