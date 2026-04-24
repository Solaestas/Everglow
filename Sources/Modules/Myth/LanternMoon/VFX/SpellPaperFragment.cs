namespace Everglow.Myth.LanternMoon.VFX;

[Pipeline(typeof(WCSPipeline))]
public class SpellPaperFragment : Visual
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
		Velocity *= 0.91f;
		Velocity = Velocity.RotatedBy(VelocityRotateSpeed);
		Scale *= 0.97f;
		RotateSpeed *= 0.98f;
		if (Timer > MaxTime)
		{
			Active = false;
		}
	}

	public override void Draw()
	{
		Texture2D tex = ModAsset.SpellPaperFragment.Value;
		Rectangle frame = new Rectangle(Frame * 7, 0, 7, 7);
		Ins.Batch.Draw(tex, Position, frame, Lighting.GetColor(Position.ToTileCoordinates()) * Fade, Rotation, frame.Size() * 0.5f, Scale, SpriteEffects.None);
	}
}