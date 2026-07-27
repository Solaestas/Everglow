namespace Everglow.Myth.LanternMoon.VFX;

[Pipeline(typeof(WCSPipeline))]
public class LanternGhostKing_SmokeSpike : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawNPCs;

	public Vector2 StartPosition = Vector2.zeroVector;
	public Vector2 Position;
	public Vector2 Velocity;
	public float Timer;
	public float MaxTime;
	public float Scale;

	public float Fade => 1 - Timer / MaxTime;

	public override void Update()
	{
		Position += Velocity;
		Velocity *= 0.9f;
		if (StartPosition == Vector2.zeroVector)
		{
			StartPosition = Position;
			Position += Velocity * 6;
		}
		Timer++;
		if (Timer > MaxTime)
		{
			Active = false;
		}
	}

	public override void Draw()
	{
		Texture2D tex = ModAsset.LanternGhostKing_SmokeSpike.Value;
		if (StartPosition == Vector2.zeroVector || StartPosition == Position)
		{
			StartPosition = Position;
			Ins.Batch.Draw(tex, Rectangle.Empty, Color.Transparent);
			return;
		}
		Vector2 dir = (Position - StartPosition).SafeNormalize(Vector2.UnitY);
		dir = dir.RotatedBy(MathHelper.PiOver2) * Scale;
		float fade = 1f;
		float timeValue = MaxTime - Timer;
		if (timeValue < 20)
		{
			fade *= timeValue / 20f;
		}

		Color powerColor = Color.Lerp(new Color(1f, 0f, 0f, 0f), new Color(1f, 0.9f, 0.6f, 0), Fade);
		Color endColor = Lighting.GetColor(Position.ToTileCoordinates());
		Color startColor = Lighting.GetColor(StartPosition.ToTileCoordinates());
		endColor = Color.Lerp(endColor, powerColor, fade);
		startColor = Color.Lerp(startColor, powerColor, fade);

		var bars = new List<Vertex2D>();
		bars.Add(Position + dir, endColor * fade, new Vector3(0, 0, 0));
		bars.Add(Position - dir, endColor * fade, new Vector3(1, 0, 0));

		bars.Add(StartPosition + dir, startColor * fade * 0.2f, new Vector3(0, 1, 0));
		bars.Add(StartPosition - dir, startColor * 0.2f, new Vector3(1, 1, 0));

		Ins.Batch.Draw(tex, bars, PrimitiveType.TriangleStrip);
	}
}