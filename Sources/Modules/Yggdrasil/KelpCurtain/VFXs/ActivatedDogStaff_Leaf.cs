using Terraria.Map;

namespace Everglow.Yggdrasil.KelpCurtain.VFXs;

[Pipeline(typeof(WCSPipeline))]
public class ActivatedDogStaff_Leaf : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawProjectiles;

	public Projectile ParentEneity;
	public Vector2 position;
	public Vector2 velocity;
	public float[] ai;
	public float timer;
	public float maxTime;
	public float scale;
	public float maxScale;
	public float rotation;
	public float Fade = 1f;

	public override void Update()
	{
		timer++;
		if (timer > maxTime)
		{
			Active = false;
		}
		if (maxTime - timer < 10)
		{
			Fade *= 0.9f;
		}
		if (ai[1] < 0)
		{
			velocity *= 0.9f;
			velocity.Y += 0.10f;
		}
		if (ParentEneity is null)
		{
			return;
		}

		Vector2 toTarget = ParentEneity.Center - position;
		float distanceValue = toTarget.Length();
		float power = 25f / (distanceValue + 1f);
		float vortexEyeRadius = ParentEneity.timeLeft;
		if (ParentEneity.active == false || (ParentEneity.Center - position).Length() > 100)
		{
			vortexEyeRadius = 1;
		}
		float deviate = vortexEyeRadius / (distanceValue + 1f) * MathHelper.PiOver2;
		Vector2 acclerate = toTarget.NormalizeSafe().RotatedBy(deviate) * power;
		velocity *= 0.9f;
		velocity += acclerate;

		position += velocity;
		rotation += MathF.Sin(ai[0]) * 0.05f;
	}

	public override void Draw()
	{
		float timeValue = (float)(Main.time * 0.24 + ai[0]);
		float frameCount = 8;
		float frameY = (int)timeValue % frameCount;
		Vector2 toCorner = new Vector2(0, scale).RotatedBy(rotation);
		Color drawColor = Lighting.GetColor(position.ToTileCoordinates()) * Fade;
		var bars = new List<Vertex2D>()
		{
			new Vertex2D(position + toCorner, drawColor, new Vector3(0, frameY / frameCount, 0)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 0.5), drawColor, new Vector3(1, frameY / frameCount, 0)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1.5), drawColor, new Vector3(0, (frameY + 1) / frameCount, 0)),

			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1.5), drawColor, new Vector3(0, (frameY + 1) / frameCount, 0)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 0.5), drawColor, new Vector3(1, frameY / frameCount, 0)),
			new Vertex2D(position + toCorner.RotatedBy(Math.PI * 1), drawColor, new Vector3(1, (frameY + 1) / frameCount, 0)),
		};
		Ins.Batch.Draw(ModAsset.ActivatedDogStaff_Leaf.Value, bars, PrimitiveType.TriangleList);
	}
}