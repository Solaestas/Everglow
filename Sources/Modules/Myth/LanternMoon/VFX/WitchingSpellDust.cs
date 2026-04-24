namespace Everglow.Myth.LanternMoon.VFX;

[Pipeline(typeof(WCSPipeline))]
public class WitchingSpellDust : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;

	public Vector2 Position;
	public Vector2 Velocity;
	public float RotateSpeed;
	public float Rotation;
	public float VelocityRotateSpeed;
	public float Scale;
	public float Timer;
	public float MaxTime;
	public int Frame;

	public float Fade => 1 - Timer / MaxTime;

	public override void Update()
	{
		Timer++;
		Position += Velocity;
		Rotation += RotateSpeed;
		Velocity *= 0.96f;
		Velocity = Velocity.RotatedBy(VelocityRotateSpeed);
		Scale *= 0.97f;
		RotateSpeed *= 0.98f;
		Lighting.AddLight(Position, Scale * 0.1f, Scale * 0.7f, Scale * 0.3f);
		if (Timer > MaxTime)
		{
			Active = false;
		}
	}

	public override void Draw()
	{
		Texture2D tex = ModAsset.WitchingSpellDust.Value;
		Rectangle frame = new Rectangle(Frame * 14, 0, 14, 14);
		Ins.Batch.Draw(tex, Position, frame, new Color(1f, 1f, 1f, 0.5f) * Fade, Rotation, frame.Size() * 0.5f, Scale, SpriteEffects.None);
	}
}