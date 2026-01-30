namespace Everglow.Myth.LanternMoon.VFX;

[Pipeline(typeof(WCSPipeline))]
public class GreenLanternFragment : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;

	public Vector2 Position;
	public Vector2 Velocity;
	public float RotateSpeed;
	public float Rotation;
	public float Rotate2Speed;
	public float Rotation2;
	public float VelocityRotateSpeed;
	public float Scale;
	public float Timer;
	public float MaxTime;
	public bool Gravity;
	public int Frame;

	public float Fade => 1 - Timer / MaxTime;

	public override void Update()
	{
		Timer++;
		Position += Velocity;
		Rotation += RotateSpeed;
		Rotation2 += Rotate2Speed;
		Velocity *= 0.99f;
		int size = (int)(30 * Scale);
		Vector2 checkNextX = Position - new Vector2(size) * 0.5f + new Vector2(Velocity.X, 0);
		Vector2 checkNextY = Position - new Vector2(size) * 0.5f + new Vector2(0, Velocity.Y);
		bool collide = false;
		if (Collision.SolidCollision(checkNextX, size, size))
		{
			collide = true;
			Velocity.X *= -Main.rand.NextFloat(0.2f, 0.99f);
			RotateSpeed += Main.rand.NextFloat(-0.03f, 0.03f) * Velocity.Length();
		}
		if (Collision.SolidCollision(checkNextY, size, size))
		{
			collide = true;
			Velocity.Y *= -Main.rand.NextFloat(0.2f, 0.99f);
			RotateSpeed += Main.rand.NextFloat(-0.03f, 0.03f) * Velocity.Length();
			if (Velocity.Length() < 1f)
			{
				Velocity *= 0;
			}
		}
		if (!collide && Gravity)
		{
			Velocity += new Vector2(0, 0.25f * Scale);
		}
		Scale *= 0.99f;
		RotateSpeed *= 0.98f;
		Rotate2Speed *= 0.98f;
		if (Timer > MaxTime)
		{
			Active = false;
		}
	}

	public override void Draw()
	{
		Texture2D tex = ModAsset.LanternGoldenShieldFragiles.Value;
		Rectangle frame = new Rectangle(0, Frame * 50, 50, 50);
		Color c0 = new Color(0.6f, 0.0f, 0.09f, 0.3f);
		Color c1 = new Color(1f, 0.3f, 0.3f, 0.6f);
		Color drawC = Color.Lerp(c0, c1, MathF.Cos(Rotation2) * 0.5f + 0.5f);
		Ins.Batch.Draw(tex, Position, frame, drawC, Rotation, frame.Size() * 0.5f, new Vector2(Scale, Scale * MathF.Sin(Rotation2)), SpriteEffects.None);

		float point = 0.85f;
		float threthod = 0.05f;
		if (MathF.Abs(MathF.Cos(Rotation2) - point) < threthod)
		{
			int value = (int)((threthod - MathF.Abs(MathF.Cos(Rotation2) - point)) / threthod * 10f);
			drawC = new Color(1f, 1f, 1f, 0.5f);
			for (int i = 0; i < value; i++)
			{
				Ins.Batch.Draw(tex, Position, frame, drawC, Rotation, frame.Size() * 0.5f, new Vector2(Scale, Scale * MathF.Sin(Rotation2)), SpriteEffects.None);
			}
		}
	}
}