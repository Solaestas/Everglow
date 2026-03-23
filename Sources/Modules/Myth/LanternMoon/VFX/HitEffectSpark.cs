namespace Everglow.Myth.LanternMoon.VFX;

[Pipeline(typeof(WCSPipeline))]
public class HitEffectSpark : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;

	public Vector2 Position;
	public Vector2 Velocity;
	public Queue<Vector2> TrailPos = new Queue<Vector2>();
	public Color DrawColor;
	public float Scale;
	public float Timer;
	public float MaxTime;
	public float GravityAcc;
	public float SpeedDecay;
	public float LightFlat;
	public bool SelfLight;
	public bool Collided = false;

	public float Fade => 1 - Timer / MaxTime;

	public override void Update()
	{
		Timer++;
		Position += Velocity;
		Velocity *= 0.8f;
		TrailPos.Enqueue(Position);
		if (TrailPos.Count > 6)
		{
			TrailPos.Dequeue();
		}
		Lighting.AddLight(Position, DrawColor.ToVector3() * LightFlat);
		Velocity += new Vector2(0, GravityAcc);
		if (Timer > MaxTime)
		{
			Active = false;
		}
	}

	public override void Draw()
	{
		Texture2D tex = Commons.ModAsset.SparkLight.Value;
		List<Vertex2D> bars = new List<Vertex2D>();
		float fade = 1;
		if (MaxTime - Timer < 30)
		{
			fade = (MaxTime - Timer) / 30f;
		}
		if (TrailPos.Count > 2)
		{
			for (int i = 0; i < TrailPos.Count; i++)
			{
				var pos = TrailPos.ToArray()[i];
				var dir = Vector2.One;
				if (i == 0)
				{
					dir = pos - TrailPos.ToArray()[i + 1];
				}
				else
				{
					dir = TrailPos.ToArray()[i - 1] - pos;
				}
				dir = dir.SafeNormalize(Vector2.Zero);
				dir = dir.RotatedBy(MathHelper.PiOver2) * Scale * fade;
				var value = i / (float)(TrailPos.Count - 1);
				var drawC = DrawColor;
				if(!SelfLight)
				{
					drawC = Lighting.GetColor(pos.ToTileCoordinates(), drawC);
					drawC.A = 0;
				}
				bars.Add(pos + dir, drawC, new Vector3(value, 0f, 0));
				bars.Add(pos - dir, drawC, new Vector3(value, 1f, 0));
			}
		}
		if (bars.Count > 0)
		{
			Ins.Batch.Draw(tex, bars, PrimitiveType.TriangleStrip);
		}
		else
		{
			Ins.Batch.Draw(tex, Position, null, new Color(0f, 0f, 0f, 0f), 0, Vector2.zeroVector, 0, SpriteEffects.None);
		}
	}
}