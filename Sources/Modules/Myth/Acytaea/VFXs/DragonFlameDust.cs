namespace Everglow.Myth.Acytaea.VFXs;

[Pipeline(typeof(WCSPipeline), typeof(BloomPipeline))]
public class DragonFlameDust : Visual
{
	public float Timer;
	public float MaxTime;
	public float Scale;
	public float Rotation;
	public int Frame;
	public Vector2 Position;
	public Vector2 Velocity;
	public float[] ai;

	public override CodeLayer DrawLayer => CodeLayer.PostDrawNPCs;

	public override void Update()
	{
		Timer++;
		if (Timer > MaxTime)
		{
			Active = false;
			return;
		}
		Position += Velocity;
		Velocity *= 0.98f;
		Rotation += ai[1];
		float pocession = (MaxTime - Timer) / MaxTime - 0.2f;
		Color flame;
		if (pocession > 0.5f)
		{
			flame = Color.Lerp(new Color(1f, 0.6f, 0f, 0), new Color(1f, 1f, 1f, 0), (pocession - 0.5f) * 2f);
		}
		else
		{
			flame = Color.Lerp(new Color(0.5f, 0f, 0f, 0), new Color(1f, 0.6f, 0f, 0), pocession * 2f);
		}
		if (MaxTime - Timer < 10f)
		{
			flame *= (MaxTime - Timer) * 0.1f;
		}
		Lighting.AddLight(Position, new Vector3(flame.R / 255f, flame.G / 255f, flame.B / 255f));
	}

	public override void Draw()
	{
		Ins.Batch.BindTexture<Vertex2D>(ModAsset.DragonFlamePostDust.Value);
		float pocession = (MaxTime - Timer) / MaxTime - 0.3f;
		Color flame;
		if (pocession > 0.5f)
		{
			flame = Color.Lerp(new Color(1f, 0.6f, 0f, 0), new Color(1f, 1f, 1f, 0), (pocession - 0.5f) * 2f);
		}
		else
		{
			flame = Color.Lerp(new Color(0.5f, 0f, 0f, 0), new Color(1f, 0.6f, 0f, 0), pocession * 2f);
		}
		flame.A = 100;
		if(MaxTime - Timer < 10f)
		{
			flame *= (MaxTime - Timer) * 0.1f;
		}
		Vector2 toCorner = new Vector2(0, Scale).RotatedBy(Rotation);
		List<Vertex2D> bars = new List<Vertex2D>()
		{
			new Vertex2D(Position + toCorner, flame, new Vector3(0, Frame * 9 / 27f, 1)),
			new Vertex2D(Position + toCorner.RotatedBy(Math.PI * 0.5), flame, new Vector3(0, (Frame + 1) * 9 / 27f, 1)),

			new Vertex2D(Position + toCorner.RotatedBy(Math.PI * 1.5), flame, new Vector3(1, Frame * 9 / 27f, 1)),
			new Vertex2D(Position + toCorner.RotatedBy(Math.PI * 1), flame, new Vector3(1, (Frame + 1) * 9 / 27f, 1)),
		};
		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}
}