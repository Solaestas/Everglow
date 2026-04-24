using Everglow.Commons.DataStructures;

namespace Everglow.Myth.LanternMoon.VFX;

[Pipeline(typeof(WCSPipeline))]
public class ThunderSpellDust : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;

	public Vector2 Position;
	public Vector2 Velocity;
	public Queue<Vector2> TrailPos = new Queue<Vector2>();
	public float Scale;
	public float Timer;
	public float MaxTime;
	public bool Collided = false;

	public float Fade => 1 - Timer / MaxTime;

	public override void Update()
	{
		Timer++;
		Position += Velocity;
		Velocity *= 0.93f;
		TrailPos.Enqueue(Position + new Vector2(0, Main.rand.NextFloat(0, 0.8f)).RotatedByRandom(MathHelper.TwoPi));
		if(TrailPos.Count > 16)
		{
			TrailPos.Dequeue();
		}
		Lighting.AddLight(Position, Scale * 0.1f, Scale * 0.09f, Scale * 0.03f);
		if(!Collided && Collision.IsWorldPointSolid(Position))
		{
			Collided = true;
		}
		if(Collided)
		{
			Velocity *= 0;
			if(MaxTime - Timer > 30)
			{
				Timer = MaxTime - 30;
			}
		}
		else
		{
			Velocity += new Vector2(0, 0.15f);
		}
		if (Timer > MaxTime)
		{
			Active = false;
		}
	}

	public override void Draw()
	{
		Texture2D tex = Commons.ModAsset.StarSlash.Value;
		List<Vertex2D> bars = new List<Vertex2D>();
		var drawColor = new Color(1f, 0.9f, 0.3f, 0);
		float fade = 1;
		if(MaxTime - Timer < 30)
		{
			fade = (MaxTime - Timer) / 30f;
		}
		if(TrailPos.Count > 2)
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
				dir = dir.RotatedBy(MathHelper.TwoPi) * Scale * fade;
				var value = i / (float)(TrailPos.Count - 1);
				bars.Add(pos + dir, drawColor, new Vector3(0.3f, value, 0));
				bars.Add(pos - dir, drawColor, new Vector3(0.7f, value, 0));
			}
		}
		if(bars.Count > 0)
		{
			Ins.Batch.Draw(tex, bars, PrimitiveType.TriangleStrip);
		}
		else
		{
			Ins.Batch.Draw(tex, Position, null, new Color(0f, 0f, 0f, 0f), 0, Vector2.zeroVector, 0, SpriteEffects.None);
		}
	}
}